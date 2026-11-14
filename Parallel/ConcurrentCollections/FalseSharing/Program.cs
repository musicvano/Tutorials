using System.Diagnostics;
using System.Runtime.InteropServices;

Console.OutputEncoding = System.Text.Encoding.UTF8;

const int Size = 100_000_000;
const int Runs = 7;
int[] threadCounts = [1, 2, 4, 8];

// Дані: 100 млн псевдовипадкових чисел (фіксоване зерно).
int[] data = new int[Size];
Random random = new(42);
for (int i = 0; i < data.Length; i++) data[i] = random.Next();
int expected = data.Count(x => x % 2 == 0);
Console.WriteLine($"Парних чисел: {expected:N0}");
Console.WriteLine(
    "Потоків | Спільний масив | Локальна змінна | Доповнення");

foreach (int p in threadCounts)
{
    double shared = Median(() => SharedArray(data, p));
    double local = Median(() => LocalVariable(data, p));
    double padded = Median(() => PaddedStruct(data, p));
    Console.WriteLine(
        $"{p,7} | {shared,11:F0} мс | {local,12:F0} мс |" +
        $" {padded,8:F0} мс");
}

// Лічильники потоків – сусідні елементи одного масиву long[].
static long SharedArray(int[] data, int threads)
{
    long[] counters = new long[threads];
    Run(data.Length, threads, (i, from, to) =>
    {
        for (int j = from; j < to; j++)
        {
            if (data[j] % 2 == 0) counters[i]++;  // спільна лінія
        }
    });
    return counters.Sum();
}

// Потік рахує в локальній змінній і записує результат один раз.
static long LocalVariable(int[] data, int threads)
{
    long[] counters = new long[threads];
    Run(data.Length, threads, (i, from, to) =>
    {
        long count = 0;
        for (int j = from; j < to; j++)
        {
            if (data[j] % 2 == 0) count++;
        }
        counters[i] = count;
    });
    return counters.Sum();
}

// Лічильники у структурах по 128 байтів – у різних кеш-лініях.
static long PaddedStruct(int[] data, int threads)
{
    PaddedCounter[] counters = new PaddedCounter[threads];
    Run(data.Length, threads, (i, from, to) =>
    {
        for (int j = from; j < to; j++)
        {
            if (data[j] % 2 == 0) counters[i].Value++;
        }
    });
    return counters.Sum(c => c.Value);
}

// Ділить індекси 0..length на частини й запускає потоки.
static void Run(int length, int threads, Action<int, int, int> body)
{
    Thread[] workers = new Thread[threads];
    int chunk = length / threads;
    for (int i = 0; i < threads; i++)
    {
        int index = i;
        int from = i * chunk;
        int to = i == threads - 1 ? length : from + chunk;
        workers[i] = new Thread(() => body(index, from, to));
        workers[i].Start();
    }
    foreach (Thread t in workers) t.Join();
}

// Прогрівання й медіана кількох запусків.
double Median(Func<long> action)
{
    action();
    double[] times = new double[Runs];
    for (int r = 0; r < Runs; r++)
    {
        long start = Stopwatch.GetTimestamp();
        long result = action();
        times[r] = Stopwatch.GetElapsedTime(start).TotalMilliseconds;
        if (result != expected) throw new InvalidOperationException();
    }
    Array.Sort(times);
    return times[Runs / 2];
}

// Розмір 128 байтів: сусідні лічильники не діляться кеш-лінією.
[StructLayout(LayoutKind.Sequential, Size = 128)]
struct PaddedCounter
{
    public long Value;
}
