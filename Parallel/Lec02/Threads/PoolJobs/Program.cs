Console.OutputEncoding = System.Text.Encoding.UTF8;

ThreadPool.GetMinThreads(out int minWorker, out int minIo);
ThreadPool.GetMaxThreads(out int maxWorker, out int maxIo);
Console.WriteLine($"Мінімум: робочих {minWorker}, I/O {minIo}");
Console.WriteLine($"Максимум: робочих {maxWorker}, I/O {maxIo}");

const int Jobs = 100;
int[] threadIds = new int[Jobs];    // робота пише у свій елемент
long[] results = new long[Jobs];
using CountdownEvent done = new(Jobs);  // готовий засіб очікування

for (int i = 0; i < Jobs; i++)
{
    ThreadPool.QueueUserWorkItem(job =>
    {
        threadIds[job] = Environment.CurrentManagedThreadId;
        results[job] = SumOfDivisors(1_000_000 + job);
        done.Signal();                // «ще одна робота виконана»
    }, i, preferLocal: false);
}
done.Wait();                          // очікування всіх 100 робіт

Console.WriteLine($"Сума результатів: {results.Sum()}");
Console.WriteLine($"Потоків пулу зараз: {ThreadPool.ThreadCount}");
Console.WriteLine(
    $"Виконано робіт: {ThreadPool.CompletedWorkItemCount}");
var groups = threadIds.GroupBy(id => id)
    .OrderByDescending(g => g.Count()).ToList();
Console.WriteLine($"Різних потоків виконали роботи: {groups.Count}");
foreach (var g in groups.Take(5))
    Console.WriteLine($"  потік {g.Key,3}: {g.Count(),3} робіт");

static long SumOfDivisors(int n)
{
    long sum = 0;
    for (int d = 1; d <= n; d++)
        if (n % d == 0) sum += d;
    return sum;
}
