using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;

const int Size = 100_000_000;
const int Runs = 7;
int[] data = new int[Size];
Random random = new(42);
for (int i = 0; i < Size; i++) data[i] = random.Next(1_000);

long expected = SumRange(data, 0, Size);  // послідовний еталон
SumParallel(data, Environment.ProcessorCount); // прогрівання JIT

Console.WriteLine($"Ядер (логічних): {Environment.ProcessorCount}");
Console.WriteLine("Потоки  Час, мс  Прискорення  Ефективність");
double t1 = 0;
for (int n = 1; n <= Environment.ProcessorCount * 2; n *= 2)
{
    double time = Median(Runs, () =>
    {
        if (SumParallel(data, n) != expected)
            throw new InvalidOperationException("Хибна сума");
    });
    if (n == 1) t1 = time;
    double s = t1 / time;
    Console.WriteLine($"{n,6} {time,8:F1} {s,12:F2} {s / n,13:F2}");
}

static long SumParallel(int[] data, int threadCount)
{
    long[] partial = new long[threadCount];   // свій елемент на потік
    Thread[] threads = new Thread[threadCount];
    int chunk = data.Length / threadCount;
    for (int t = 0; t < threadCount; t++)
    {
        int index = t;
        int from = t * chunk;
        int to = t == threadCount - 1 ? data.Length : from + chunk;
        threads[t] = new Thread(
            () => partial[index] = SumRange(data, from, to));
        threads[t].Start();
    }
    foreach (Thread thread in threads) thread.Join();
    return partial.Sum();
}

static long SumRange(int[] data, int from, int to)
{
    long sum = 0;
    for (int i = from; i < to; i++)
    {
        int x = data[i];
        sum += (long)x * x % 1_000_003;       // «важча» операція
    }
    return sum;
}

static double Median(int runs, Action action)
{
    double[] times = new double[runs];
    for (int r = 0; r < runs; r++)
    {
        long start = Stopwatch.GetTimestamp();
        action();
        times[r] = Stopwatch.GetElapsedTime(start).TotalMilliseconds;
    }
    Array.Sort(times);
    return times[runs / 2];
}
