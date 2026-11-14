using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;

const int Width = 2400, Height = 1600, MaxIter = 1000;
byte[] image = new byte[Width * Height];

// Послідовний варіант – еталон для перевірки та T1.
long checkSeq = 0;
double tSeq = Median(() => checkSeq = RenderSequential(image));
Console.WriteLine($"Послідовно: {tSeq,8:F1} мс, " +
    $"ітерацій {checkSeq:N0}");

Console.WriteLine(" p   Час, мс   S      E      Перевірка");
foreach (int p in new[] { 1, 2, 4, 8, 16 })
{
    long check = 0;
    double t = Median(() => check = RenderParallel(image, p));
    double s = tSeq / t;
    string ok = check == checkSeq ? "так" : "НІ";
    Console.WriteLine(
        $"{p,2} {t,9:F1} {s,5:F2} {s / p,6:F2}    {ok}");
}

WritePgm("mandelbrot.pgm", image);
Console.WriteLine("Зображення: mandelbrot.pgm");

static long RenderSequential(byte[] pixels)
{
    long total = 0;
    for (int y = 0; y < Height; y++)
    {
        total += RenderRow(pixels, y);
    }
    return total;
}

static long RenderParallel(byte[] pixels, int threads)
{
    ParallelOptions options = new()
    {
        MaxDegreeOfParallelism = threads
    };
    long total = 0;
    Parallel.For(0, Height, options, y =>
    {
        long rowIterations = RenderRow(pixels, y);
        Interlocked.Add(ref total, rowIterations);
    });
    return total;
}

// Рядок y незалежний від інших: пише лише у свої пікселі.
static long RenderRow(byte[] pixels, int y)
{
    long iterations = 0;
    double ci = -1.0 + 2.0 * y / Height;
    for (int x = 0; x < Width; x++)
    {
        double cr = -2.2 + 3.0 * x / Width;
        double zr = 0, zi = 0;
        int n = 0;
        while (n < MaxIter && zr * zr + zi * zi <= 4.0)
        {
            double t = zr * zr - zi * zi + cr;
            zi = 2 * zr * zi + ci;
            zr = t;
            n++;
        }
        iterations += n;
        pixels[y * Width + x] =
            (byte)(n == MaxIter ? 0 : 255 - n % 256);
    }
    return iterations;
}

// Прогрівання JIT, потім медіана п’яти запусків.
static double Median(Action action, int runs = 5)
{
    action();
    double[] times = new double[runs];
    for (int i = 0; i < runs; i++)
    {
        long start = Stopwatch.GetTimestamp();
        action();
        times[i] = Stopwatch.GetElapsedTime(start).TotalMilliseconds;
    }
    Array.Sort(times);
    return times[runs / 2];
}

static void WritePgm(string path, byte[] pixels)
{
    using FileStream file = File.Create(path);
    string header = $"P5\n{Width} {Height}\n255\n";
    file.Write(System.Text.Encoding.ASCII.GetBytes(header));
    file.Write(pixels);
}
