using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

Console.OutputEncoding = System.Text.Encoding.UTF8;

int n = args.Length > 0 ? int.Parse(args[0]) : 1024;
int block = args.Length > 1 ? int.Parse(args[1]) : 64;
double[] a = RandomMatrix(n, seed: 1);
double[] b = RandomMatrix(n, seed: 2);
double[] expected = new double[n * n];

(string Name, Action<double[]> Run)[] methods =
[
    ("ijk", c => Mul.Ijk(a, b, c, n)),
    ("ikj", c => Mul.Ikj(a, b, c, n)),
    ("блочне", c => Mul.Blocked(a, b, c, n, block, false, 1)),
    ("блочне + SIMD", c => Mul.Blocked(a, b, c, n, block, true, 1)),
    ("блочне + SIMD + потоки",
        c => Mul.Blocked(a, b, c, n, block, true, -1)),
];

Console.WriteLine($"n = {n}, блок {block}×{block}, " +
    $"потоків {Environment.ProcessorCount}");
Console.WriteLine($"{"Спосіб",-24}{"Час, мс",9}{"GFLOPS",8}" +
    $"{"S",7}  max|ΔC|");
double t0 = 0;
foreach (var (name, run) in methods)
{
    double[] c = t0 == 0 ? expected : new double[n * n];
    int runs = t0 == 0 ? 1 : 3;          // ijk дуже повільний
    double t = Median(() => { Array.Clear(c); run(c); }, runs);
    if (t0 == 0) t0 = t;
    double gflops = 2.0 * n * n * n / t / 1e9;
    double error = MaxDiff(c, expected);
    Console.WriteLine($"{name,-24}{t * 1000,9:F1}{gflops,8:F2}" +
        $"{t0 / t,7:F1}  {error:G3}");
}

static double[] RandomMatrix(int n, int seed)
{
    Random random = new(seed);
    double[] m = new double[n * n];
    for (int i = 0; i < m.Length; i++) m[i] = random.NextDouble();
    return m;
}

static double MaxDiff(double[] x, double[] y)
{
    double max = 0;
    for (int i = 0; i < x.Length; i++)
        max = Math.Max(max, Math.Abs(x[i] - y[i]));
    return max;
}

// Медіана запусків у секундах; прогрівання лише для швидких.
static double Median(Action action, int runs)
{
    if (runs > 1) action();
    double[] times = new double[runs];
    for (int i = 0; i < runs; i++)
    {
        long start = Stopwatch.GetTimestamp();
        action();
        times[i] = Stopwatch.GetElapsedTime(start).TotalSeconds;
    }
    Array.Sort(times);
    return times[runs / 2];
}

// Матриці зберігаються за рядками: елемент (i, j) – m[i * n + j].
static class Mul
{
    public static void Ijk(double[] a, double[] b, double[] c, int n)
    {
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
            {
                double sum = 0;
                for (int k = 0; k < n; k++)
                    sum += a[i * n + k] * b[k * n + j]; // стовпець B
                c[i * n + j] = sum;
            }
    }

    public static void Ikj(double[] a, double[] b, double[] c, int n)
    {
        for (int i = 0; i < n; i++)
            for (int k = 0; k < n; k++)
            {
                double aik = a[i * n + k];
                for (int j = 0; j < n; j++)
                    c[i * n + j] += aik * b[k * n + j];  // по рядку
            }
    }

    // threads = 1 – послідовно, -1 – усі ядра (рядки плиток C).
    public static void Blocked(double[] a, double[] b, double[] c,
        int n, int bs, bool simd, int threads)
    {
        ParallelOptions options = new()
        {
            MaxDegreeOfParallelism = threads
        };
        int rows = (n + bs - 1) / bs;
        Parallel.For(0, rows, options,
            r => MulBlockRow(a, b, c, n, bs, r * bs, simd));
    }

    // Рядки i0..i0+bs результату: плитки bs×bs для k і j.
    static void MulBlockRow(double[] a, double[] b, double[] c,
        int n, int bs, int i0, bool simd)
    {
        int iEnd = Math.Min(i0 + bs, n);
        for (int k0 = 0; k0 < n; k0 += bs)
        for (int j0 = 0; j0 < n; j0 += bs)
        {
            int kEnd = Math.Min(k0 + bs, n);
            int len = Math.Min(j0 + bs, n) - j0;
            for (int i = i0; i < iEnd; i++)
            for (int k = k0; k < kEnd; k++)
            {
                double aik = a[i * n + k];
                Span<double> ci = c.AsSpan(i * n + j0, len);
                ReadOnlySpan<double> bk = b.AsSpan(k * n + j0, len);
                if (simd)
                {
                    AddScaled(ci, bk, aik);
                    continue;
                }
                for (int j = 0; j < len; j++) ci[j] += aik * bk[j];
            }
        }
    }

    // ci += aik * bk: 4 double за крок у Vector256.
    static void AddScaled(Span<double> ci, ReadOnlySpan<double> bk,
        double aik)
    {
        ref double pc = ref MemoryMarshal.GetReference(ci);
        ref double pb = ref MemoryMarshal.GetReference(bk);
        Vector256<double> va = Vector256.Create(aik);
        int j = 0;
        for (; j <= ci.Length - 4; j += 4)
        {
            nuint k = (nuint)j;
            Vector256<double> r = Vector256.LoadUnsafe(ref pc, k)
                + va * Vector256.LoadUnsafe(ref pb, k);
            r.StoreUnsafe(ref pc, k);
        }
        for (; j < ci.Length; j++) ci[j] += aik * bk[j];
    }
}
