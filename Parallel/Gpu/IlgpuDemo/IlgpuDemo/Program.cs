// ILGPU: перелік пристроїв, SAXPY і розмиття зображення на GPU.
// Аргумент --cpu – CPU-акселератор ILGPU замість відеокарти.
using System.Diagnostics;
using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.CPU;

using Context context = Context.CreateDefault(); // CUDA, OpenCL, CPU
Console.WriteLine("Пристрої ILGPU:");
foreach (Device d in context)
    Console.WriteLine($"  {d.AcceleratorType,-6} {d.Name}" +
        (d.AcceleratorType == AcceleratorType.CPU ? ""
            : $", {d.MemorySize / 1073741824.0:F1} ГБ"));

Device device = args.Contains("--cpu")
    ? context.GetCPUDevice(0)
    : context.GetPreferredDevice(preferCPU: false);
using Accelerator accelerator = device.CreateAccelerator(context);
Console.WriteLine($"Обрано: {accelerator.Name}");

// SAXPY: y = a·x + y для мільйона елементів.
const int N = 1_000_000;
float[] x = Enumerable.Range(0, N).Select(i => (float)i).ToArray();
float[] y = Enumerable.Repeat(1.0f, N).ToArray();
using MemoryBuffer1D<float, Stride1D.Dense> dx =
    accelerator.Allocate1D(x);             // виділення й копіювання
using MemoryBuffer1D<float, Stride1D.Dense> dy =
    accelerator.Allocate1D(y);
var saxpy = accelerator.LoadAutoGroupedStreamKernel<
    Index1D, float, ArrayView<float>, ArrayView<float>>(Saxpy);
saxpy((int)dx.Length, 2.0f, dx.View, dy.View);  // асинхронний запуск
accelerator.Synchronize();                      // чекати на ядро
float[] result = dy.GetAsArray1D();             // копіювання на хост
Console.WriteLine($"SAXPY: y[0] = {result[0]}, y[{N - 1}] = " +
                  $"{result[N - 1]}");

// Розмиття: середнє у квадраті (2r + 1)×(2r + 1) навколо пікселя.
var blur = accelerator.LoadAutoGroupedStreamKernel<Index2D,
    ArrayView<float>, ArrayView<float>, int, int, int>(Blur);
Console.WriteLine(
    $"{"Розмір",-10}{"Спосіб",-24}{"Час, мс",9}{"S",7}");
foreach ((int w, int h) in new[] { (3840, 2160), (7680, 4320) })
{
    float[] image = new float[w * h];
    var random = new Random(10);
    for (int i = 0; i < image.Length; i++)
        image[i] = random.NextSingle() * 255;
    float[] cpu = new float[image.Length];

    double seq = Median(() => BlurCpu(image, cpu, w, h, 3, false));
    double par = Median(() => BlurCpu(image, cpu, w, h, 3, true));
    using var src = accelerator.Allocate1D<float>(image.Length);
    using var dst = accelerator.Allocate1D<float>(image.Length);
    float[] gpu = new float[image.Length];
    double kernel = Median(() =>
    {
        blur(new Index2D(w, h), src.View, dst.View, w, h, 3);
        accelerator.Synchronize();
    });
    double total = Median(() =>
    {
        src.CopyFromCPU(image);
        blur(new Index2D(w, h), src.View, dst.View, w, h, 3);
        dst.CopyToCPU(gpu);                     // чекає на ядро
    });
    float diff = 0;
    for (int i = 0; i < gpu.Length; i++)
        diff = Math.Max(diff, Math.Abs(gpu[i] - cpu[i]));

    string size = $"{w}x{h}";
    Row(size, "CPU, послідовно", seq, seq);
    Row(size, "CPU, Parallel.For", par, seq);
    Row(size, "ILGPU, лише ядро", kernel, seq);
    Row(size, "ILGPU, з копіюваннями", total, seq);
    Console.WriteLine($"{"",-10}найбільша різниця з CPU: {diff}");
}

static void Row(string size, string name, double ms, double seq) =>
    Console.WriteLine(
        $"{size,-10}{name,-24}{ms,9:F2}{seq / ms,7:F1}");

// Медіана 5 вимірювань після прогрівання.
static double Median(Action action)
{
    var warm = Stopwatch.StartNew();         // прогрівання: JIT,
    do action();                             // частоти GPU
    while (warm.ElapsedMilliseconds < 300);
    double[] t = new double[5];
    for (int r = 0; r < t.Length; r++)
    {
        long start = Stopwatch.GetTimestamp();
        action();
        t[r] = Stopwatch.GetElapsedTime(start).TotalMilliseconds;
    }
    Array.Sort(t);
    return t[2];
}

static void Saxpy(Index1D i, float a, ArrayView<float> x,
                  ArrayView<float> y) => y[i] = a * x[i] + y[i];

// Ядро: потік p = (X, Y) обчислює один піксель.
static void Blur(Index2D p, ArrayView<float> src,
                 ArrayView<float> dst, int w, int h, int r)
{
    float sum = 0;
    int count = 0;
    for (int yy = Math.Max(p.Y - r, 0);
         yy <= Math.Min(p.Y + r, h - 1); yy++)
        for (int xx = Math.Max(p.X - r, 0);
             xx <= Math.Min(p.X + r, w - 1); xx++)
        {
            sum += src[yy * w + xx];
            count++;
        }
    dst[p.Y * w + p.X] = sum / count;
}

// Та сама формула на CPU: послідовно або Parallel.For за рядками.
static void BlurCpu(float[] src, float[] dst, int w, int h, int r,
                    bool parallel)
{
    void RowBlur(int y)
    {
        for (int x = 0; x < w; x++)
        {
            float sum = 0;
            int count = 0;
            for (int yy = Math.Max(y - r, 0);
                 yy <= Math.Min(y + r, h - 1); yy++)
                for (int xx = Math.Max(x - r, 0);
                     xx <= Math.Min(x + r, w - 1); xx++)
                {
                    sum += src[yy * w + xx];
                    count++;
                }
            dst[y * w + x] = sum / count;
        }
    }
    if (parallel) Parallel.For(0, h, RowBlur);
    else for (int y = 0; y < h; y++) RowBlur(y);
}
