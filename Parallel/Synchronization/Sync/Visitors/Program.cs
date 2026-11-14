using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;

const int Total = 8_000_000;     // відвідувань на всі потоки
int[] threadCounts = [1, 2, 4, 8, 16];

long unsafeCount = 0, atomicCount = 0, monitorCount = 0,
    lockCount = 0, localCount = 0;
object monitorGate = new();
Lock gate = new();

// Спосіб: назва, робота потоку (n відвідувань), читання, скидання.
(string Name, Action<int> Work, Func<long> Read, Action Reset)[]
    methods =
[
    ("без синхронізації", n =>
        { for (int i = 0; i < n; i++) unsafeCount++; },
        () => unsafeCount, () => unsafeCount = 0),
    ("Interlocked", n =>
        { for (int i = 0; i < n; i++)
            Interlocked.Increment(ref atomicCount); },
        () => atomicCount, () => atomicCount = 0),
    ("lock(object)", n =>
        { for (int i = 0; i < n; i++)
            lock (monitorGate) monitorCount++; },
        () => monitorCount, () => monitorCount = 0),
    ("lock(Lock)", n =>
        { for (int i = 0; i < n; i++)
            lock (gate) lockCount++; },
        () => lockCount, () => lockCount = 0),
    ("локальний лічильник", n =>
        {
            long local = 0;                 // без спільного стану
            for (int i = 0; i < n; i++) local++;
            Interlocked.Add(ref localCount, local);
        },
        () => localCount, () => localCount = 0),
];

Console.WriteLine($"Процесорів: {Environment.ProcessorCount}, " +
    $"відвідувань: {Total:N0}");
Console.WriteLine(
    $"{"Спосіб",-20}{"Потоки",7}{"Результат",12}{"Час, мс",9}");
foreach (var m in methods)
{
    foreach (int p in threadCounts)
    {
        Run(m.Work, p);                     // прогрівання JIT
        double[] times = new double[5];
        long result = 0;
        for (int r = 0; r < times.Length; r++)
        {
            m.Reset();
            times[r] = Run(m.Work, p);
            result = m.Read();
        }
        Array.Sort(times);                  // медіана 5 запусків
        Console.WriteLine(
            $"{m.Name,-20}{p,7}{result,12:N0}{times[2],9:F1}");
    }
}

// Запускає p потоків, кожен виконує Total / p відвідувань.
static double Run(Action<int> work, int p)
{
    var threads = new Thread[p];
    for (int t = 0; t < p; t++)
        threads[t] = new Thread(() => work(Total / p));
    var sw = Stopwatch.StartNew();
    foreach (var t in threads) t.Start();
    foreach (var t in threads) t.Join();
    return sw.Elapsed.TotalMilliseconds;
}
