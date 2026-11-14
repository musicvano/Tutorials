using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<CounterBenchmarks>();

public class CounterBenchmarks
{
    private int[] data = [];

    [Params(8)]                      // кількість потоків
    public int Threads { get; set; }

    [GlobalSetup]                    // один раз перед вимірюваннями
    public void Setup()
    {
        // Дані: 100 млн псевдовипадкових чисел (фіксоване зерно).
        data = new int[100_000_000];
        Random random = new(42);
        for (int i = 0; i < data.Length; i++) data[i] = random.Next();
    }

    [Benchmark(Baseline = true)]     // базовий варіант
    public long SharedArray() => SharedArray(data, Threads);

    [Benchmark]
    public long LocalVariable() => LocalVariable(data, Threads);

    [Benchmark]
    public long PaddedStruct() => PaddedStruct(data, Threads);

    // Тіла методів – з прикладу «Хибне розділення».

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
}

// Розмір 128 байтів: сусідні лічильники не діляться кеш-лінією.
[StructLayout(LayoutKind.Sequential, Size = 128)]
struct PaddedCounter
{
    public long Value;
}
