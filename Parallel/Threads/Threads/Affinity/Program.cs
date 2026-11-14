using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;

if (!OperatingSystem.IsWindows() && !OperatingSystem.IsLinux())
{
    Console.Error.WriteLine("Спорідненість: лише Windows і Linux");
    return 1;
}

using Process self = Process.GetCurrentProcess();
nint allCores = self.ProcessorAffinity;
Console.WriteLine($"Маска за замовчуванням: {Mask(allCores)}");
Console.WriteLine($"Клас пріоритету: {self.PriorityClass}");

const int Threads = 8;
CountPrimes(Threads, 200_000);                  // прогрівання JIT

(string Name, nint Mask)[] configs =
[
    ("усі ядра", allCores),
    ("ЛП 0, 2", 0b101),
    ("ЛП 0, 1", 0b11),
    ("ЛП 0", 0b1),
];
foreach (var (name, mask) in configs)
{
    self.ProcessorAffinity = mask;              // обмеження процесу
    long start = Stopwatch.GetTimestamp();
    int primes = CountPrimes(Threads, 3_000_000);
    double ms = Stopwatch.GetElapsedTime(start).TotalMilliseconds;
    Console.WriteLine(
        $"{name,-8} {Mask(mask),17} {ms,6:F0} мс  простих: {primes}");
}

self.ProcessorAffinity = allCores;              // відновлення
self.PriorityClass = ProcessPriorityClass.BelowNormal;
self.Refresh();
Console.WriteLine($"Новий клас пріоритету: {self.PriorityClass}");
return 0;

static string Mask(nint mask) =>
    Convert.ToString((long)mask, 2).PadLeft(16, '0');

static int CountPrimes(int threadCount, int limit)
{
    int[] counts = new int[threadCount];
    Thread[] threads = new Thread[threadCount];
    for (int t = 0; t < threadCount; t++)
    {
        int index = t;
        threads[t] = new Thread(() =>
        {
            // Потік t перевіряє числа виду t + k·threadCount.
            for (int n = index; n < limit; n += threadCount)
                if (IsPrime(n)) counts[index]++;
        });
        threads[t].Start();
    }
    foreach (Thread thread in threads) thread.Join();
    return counts.Sum();
}

static bool IsPrime(int n)
{
    if (n < 2) return false;
    for (int d = 2; (long)d * d <= n; d++)
        if (n % d == 0) return false;
    return true;
}
