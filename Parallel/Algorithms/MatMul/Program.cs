using System.Diagnostics;
using System.Numerics.Tensors;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// n ділиться на 1, 2, 3 і 4, тому решітки 1×1 … 4×4 рівні.
int n = args.Length > 0 ? int.Parse(args[0]) : 1152;
const int Cores = 8;                     // фізичні ядра i9-11900KF
double[] a = Random(n, seed: 1), b = Random(n, seed: 2);
double[] expected = new double[n * n], c = new double[n * n];

Mul.Stripes(a, b, expected, n, 1);      // еталон
Console.WriteLine($"n = {n}, фізичних ядер {Cores}");
double t1 = 0;                           // перший рядок таблиці
Console.WriteLine("Схема       p  Час, мс     S  Прогноз S     E" +
    "  max|ΔC|");
foreach (string scheme in new[] { "смуги", "шахова", "Кеннон" })
{
    foreach (int q in new[] { 1, 2, 3, 4 })
    {
        int p = q * q;
        Action run = scheme switch
        {
            "смуги" => () => Mul.Stripes(a, b, c, n, p),
            "шахова" => () => Mul.Blocks2D(a, b, c, n, q),
            _ => () => Mul.Cannon(a, b, c, n, q),
        };
        double t = Median(run);
        if (t1 == 0) t1 = t;
        // Робота ділиться на p, але одночасно працюють лише 8 ядер.
        double predicted = t1 / Math.Min(p, Cores);
        if (scheme == "Кеннон")          // + обміни й бар’єри
            predicted += Model.CannonOverhead(n, q);
        double s = t1 / t;
        Console.WriteLine($"{scheme,-10}{p,3}{t,9:F1}{s,6:F1}" +
            $"{t1 / predicted,11:F1}{s / p,6:F2}" +
            $"  {MaxDiff(c, expected):G2}");
    }
}

static double[] Random(int n, int seed)
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

static double Median(Action action, int runs = 7)
{
    action();
    Thread.Sleep(200);
    action();                            // прогрівання
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

static class Mul
{
    // C[rows × cols] += A[rows × inner] · B[inner × cols]; кожна
    // матриця задана масивом, зміщенням і довжиною рядка (ld).
    public static void Kernel(double[] a, int aOff, int lda,
        double[] b, int bOff, int ldb, double[] c, int cOff, int ldc,
        int rows, int inner, int cols)
    {
        for (int i = 0; i < rows; i++)
        {
            Span<double> ci = c.AsSpan(cOff + i * ldc, cols);
            for (int k = 0; k < inner; k++)
            {
                double aik = a[aOff + i * lda + k];
                var bk = b.AsSpan(bOff + k * ldb, cols);
                TensorPrimitives.MultiplyAdd(bk, aik, ci, ci);
            }
        }
    }

    // Горизонтальні смуги: потік t обчислює свої рядки C.
    public static void Stripes(double[] a, double[] b, double[] c,
        int n, int p)
    {
        Array.Clear(c);
        Parallel.For(0, p, new ParallelOptions
            { MaxDegreeOfParallelism = p }, t =>
        {
            int lo = t * n / p, hi = (t + 1) * n / p;
            Kernel(a, lo * n, n, b, 0, n, c, lo * n, n,
                hi - lo, n, n);
        });
    }

    // Шахова схема: потік (i, j) обчислює блок C_ij = Σ A_ik·B_kj,
    // читаючи рядок блоків A і стовпець блоків B зі спільної пам’яті.
    public static void Blocks2D(double[] a, double[] b, double[] c,
        int n, int q)
    {
        Array.Clear(c);
        int bs = n / q;
        Parallel.For(0, q * q, new ParallelOptions
            { MaxDegreeOfParallelism = q * q }, t =>
        {
            int i = t / q, j = t % q;
            for (int k = 0; k < q; k++)
                Kernel(a, i * bs * n + k * bs, n,
                    b, k * bs * n + j * bs, n,
                    c, i * bs * n + j * bs, n, bs, bs, bs);
        });
    }

    // Алгоритм Кеннона: у потоку (i, j) власні копії блоків A і B.
    // Після зсуву-вирівнювання q кроків: множення, бар’єр, зсув A
    // ліворуч і B угору (копіювання блоку сусіда), бар’єр.
    public static void Cannon(double[] a, double[] b, double[] c,
        int n, int q)
    {
        int bs = n / q, p = q * q;
        double[][] blockA = new double[p][], blockB = new double[p][];
        double[][] nextA = new double[p][], nextB = new double[p][];
        double[][] blockC = new double[p][];
        using Barrier barrier = new(p);
        Thread[] threads = new Thread[p];
        for (int t = 0; t < p; t++)
        {
            int i = t / q, j = t % q;
            threads[t] = new Thread(() =>
            {
                int me = i * q + j;
                // Вирівнювання: A_ij ← A_i,(i+j), B_ij ← B_(i+j),j.
                int s = (i + j) % q;
                blockA[me] = Copy(a, n, i, s, bs);
                blockB[me] = Copy(b, n, s, j, bs);
                nextA[me] = new double[bs * bs];
                nextB[me] = new double[bs * bs];
                blockC[me] = new double[bs * bs];
                barrier.SignalAndWait();
                for (int step = 0; step < q; step++)
                {
                    Kernel(blockA[me], 0, bs, blockB[me], 0, bs,
                        blockC[me], 0, bs, bs, bs, bs);
                    if (step == q - 1) break;
                    int right = i * q + (j + 1) % q;
                    int below = (i + 1) % q * q + j;
                    blockA[right].CopyTo(nextA[me], 0);   // «прийом»
                    blockB[below].CopyTo(nextB[me], 0);
                    barrier.SignalAndWait();   // усі скопіювали
                    (blockA[me], nextA[me]) = (nextA[me], blockA[me]);
                    (blockB[me], nextB[me]) = (nextB[me], blockB[me]);
                    barrier.SignalAndWait();   // усі обміняли буфери
                }
                Paste(blockC[me], c, n, i, j, bs);
            });
            threads[t].Start();
        }
        foreach (Thread thread in threads) thread.Join();
    }

    // Блок (bi, bj) розміром bs × bs – в окремий масив і назад.
    static double[] Copy(double[] m, int n, int bi, int bj, int bs)
    {
        double[] block = new double[bs * bs];
        for (int r = 0; r < bs; r++)
            m.AsSpan((bi * bs + r) * n + bj * bs, bs)
                .CopyTo(block.AsSpan(r * bs, bs));
        return block;
    }

    static void Paste(double[] block, double[] m, int n, int bi,
        int bj, int bs)
    {
        for (int r = 0; r < bs; r++)
            block.AsSpan(r * bs, bs)
                .CopyTo(m.AsSpan((bi * bs + r) * n + bj * bs, bs));
    }
}

// Прогноз накладних витрат Кеннона з двох вимірювань:
// копіювання блоку й фаза бар’єра для p потоків.
static class Model
{
    public static double CannonOverhead(int n, int q)
    {
        if (q == 1) return 0;
        int bs = n / q, p = q * q;
        double[] from = new double[bs * bs], to = new double[bs * bs];
        double copy = Time(() => from.CopyTo(to, 0), 50);
        double phase = BarrierPhase(p);
        // вирівнювання (2 блоки) + (q − 1) зсувів по 2 блоки
        // + 2 бар’єри на зсув; створення p потоків не враховано.
        return 2 * copy + (q - 1) * (2 * copy + 2 * phase);
    }

    static double BarrierPhase(int p)
    {
        const int Phases = 2000;
        using Barrier barrier = new(p);
        Thread[] threads = new Thread[p];
        long start = Stopwatch.GetTimestamp();
        for (int t = 0; t < p; t++)
        {
            threads[t] = new Thread(() =>
            {
                for (int k = 0; k < Phases; k++)
                    barrier.SignalAndWait();
            });
            threads[t].Start();
        }
        foreach (Thread thread in threads) thread.Join();
        return Stopwatch.GetElapsedTime(start).TotalMilliseconds
            / Phases;
    }

    static double Time(Action action, int repeats)
    {
        action();
        long start = Stopwatch.GetTimestamp();
        for (int k = 0; k < repeats; k++) action();
        return Stopwatch.GetElapsedTime(start).TotalMilliseconds
            / repeats;
    }
}
