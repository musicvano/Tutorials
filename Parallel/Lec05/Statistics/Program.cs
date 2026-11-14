using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;

double[] data = new double[20_000_000];
Random random = new(42);
for (int i = 0; i < data.Length; i++) data[i] = random.NextDouble();

Stats seq = null!, par = null!;
double tSeq = await MedianAsync(() =>
{
    seq = Compute(data, 0, data.Length);
    return Task.CompletedTask;
});
double tPar = await MedianAsync(async () =>
    par = await ComputeParallelAsync(data, parts: 8));
Console.WriteLine($"Послідовно: {seq}");
Console.WriteLine($"Паралельно: {par}");
Console.WriteLine($"Різниця сум: {Math.Abs(seq.Sum - par.Sum):E1}");
Console.WriteLine(
    $"Час: {tSeq:F1} мс проти {tPar:F1} мс, S = {tSeq / tPar:F2}");

// Дві частини з трьох містять некоректні значення.
data[3] = double.NaN;
data[^1] = double.PositiveInfinity;
int third = data.Length / 3;
Task<Stats>[] tasks =
[
    Task.Run(() => Compute(data, 0, third)),
    Task.Run(() => Compute(data, third, 2 * third)),
    Task.Run(() => Compute(data, 2 * third, data.Length)),
];
Task<Stats[]> all = Task.WhenAll(tasks);
try
{
    await all;                        // кидає лише перший виняток
}
catch (InvalidDataException ex)
{
    Console.WriteLine($"await: {ex.Message}");
    foreach (Exception inner in all.Exception!.InnerExceptions)
    {
        Console.WriteLine($"  AggregateException: {inner.Message}");
    }
}
for (int i = 0; i < tasks.Length; i++)
{
    Console.WriteLine($"Задача {i}: {tasks[i].Status}");
}

static async Task<Stats> ComputeParallelAsync(double[] a, int parts)
{
    Task<Stats>[] tasks = new Task<Stats>[parts];
    int chunk = a.Length / parts;
    for (int p = 0; p < parts; p++)
    {
        int from = p * chunk;                  // копії для замикання
        int to = p == parts - 1 ? a.Length : from + chunk;
        tasks[p] = Task.Run(() => Compute(a, from, to));
    }
    Stats[] partial = await Task.WhenAll(tasks);   // порядок задач
    return partial.Aggregate(Stats.Combine);
}

static Stats Compute(double[] a, int from, int to)
{
    double sum = 0, squares = 0, max = double.MinValue;
    for (int i = from; i < to; i++)
    {
        double x = a[i];
        if (!double.IsFinite(x))
        {
            throw new InvalidDataException($"a[{i}] = {x}");
        }
        sum += x;
        squares += x * x;
        max = Math.Max(max, x);
    }
    return new(to - from, sum, squares, max);
}

// Прогрів і медіана п’яти вимірювань.
static async Task<double> MedianAsync(Func<Task> action)
{
    await action();
    double[] times = new double[5];
    for (int i = 0; i < times.Length; i++)
    {
        long start = Stopwatch.GetTimestamp();
        await action();
        times[i] = Stopwatch.GetElapsedTime(start).TotalMilliseconds;
    }
    Array.Sort(times);
    return times[2];
}

record Stats(long Count, double Sum, double Squares, double Max)
{
    public double Mean => Sum / Count;
    public double StdDev => Math.Sqrt(Squares / Count - Mean * Mean);

    public static Stats Combine(Stats x, Stats y) => new(
        x.Count + y.Count, x.Sum + y.Sum, x.Squares + y.Squares,
        Math.Max(x.Max, y.Max));

    public override string ToString() =>
        $"n = {Count:N0}, x̄ = {Mean:F5}, σ = {StdDev:F5}";
}
