using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;

const long Steps = 400_000_000;
int maxThreads = Environment.ProcessorCount;

ComputePi(Steps, 1);                        // прогрівання
double t1 = MedianTime(() => ComputePi(Steps, 1));
Console.WriteLine($"π ≈ {ComputePi(Steps, 1):F10}");
Console.WriteLine($"{"p",3} {"Tp, мс",9} {"S",6} {"E",6} {"e",7}");
for (int p = 1; p <= maxThreads; p *= 2)
{
    double tp = p == 1 ? t1 : MedianTime(() => ComputePi(Steps, p));
    double s = t1 / tp;
    string e = p == 1 ? "–" : $"{KarpFlatt(s, p):F3}";
    Console.WriteLine($"{p,3} {tp,9:F1} {s,6:F2} {s / p,6:P0} {e,7}");
}

// Метрика Карпа–Флатта: e = (1/S - 1/p) / (1 - 1/p).
static double KarpFlatt(double s, int p) =>
    (1 / s - 1.0 / p) / (1 - 1.0 / p);

// π як інтеграл 4/(1 + x²) на [0; 1] методом середніх прямокутників.
// Інтервал ділиться на p частин, кожна частина – окремий потік
// (Parallel.For детально розглядається в темі 6).
static double ComputePi(long steps, int p)
{
    double h = 1.0 / steps;
    double[] parts = new double[p];
    Parallel.For(0, p,
        new ParallelOptions { MaxDegreeOfParallelism = p },
        part =>
        {
            long from = steps / p * part;
            long to = part == p - 1 ? steps : from + steps / p;
            double sum = 0;
            for (long i = from; i < to; i++)
            {
                double x = (i + 0.5) * h;
                sum += 4 / (1 + x * x);
            }
            parts[part] = sum;
        });
    return parts.Sum() * h;
}

static double MedianTime(Action action)
{
    double[] times = new double[5];
    for (int i = 0; i < times.Length; i++)
    {
        long start = Stopwatch.GetTimestamp();
        action();
        times[i] = Stopwatch.GetElapsedTime(start).TotalMilliseconds;
    }
    Array.Sort(times);
    return times[times.Length / 2];
}
