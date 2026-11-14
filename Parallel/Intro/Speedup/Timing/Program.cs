using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;

const int N = 200_000_000;
const int Runs = 10;

// Перший («холодний») запуск містить JIT-компіляцію.
Stopwatch sw = Stopwatch.StartNew();
double sum = HarmonicSum(N);
sw.Stop();
double first = sw.Elapsed.TotalMilliseconds;
Console.WriteLine($"H(n) = {sum:F6}");
Console.WriteLine($"Перший запуск: {first:F1} мс");

// Прогрівання, потім серія вимірювань.
for (int i = 0; i < 3; i++) HarmonicSum(N);

double[] times = new double[Runs];
for (int i = 0; i < Runs; i++)
{
    long start = Stopwatch.GetTimestamp();
    HarmonicSum(N);
    times[i] = Stopwatch.GetElapsedTime(start).TotalMilliseconds;
}

Array.Sort(times);
double median = (times[Runs / 2 - 1] + times[Runs / 2]) / 2;
double mean = times.Average();
double sd = Math.Sqrt(times.Sum(t => (t - mean) * (t - mean))
                      / (Runs - 1));
Console.WriteLine($"Запусків: {Runs}");
Console.WriteLine($"Мінімум:  {times[0],8:F1} мс");
Console.WriteLine($"Медіана:  {median,8:F1} мс");
Console.WriteLine($"Максимум: {times[^1],8:F1} мс");
Console.WriteLine($"Відхилення: {sd / mean,6:P1}");

static double HarmonicSum(int n)
{
    double sum = 0;
    for (int k = 1; k <= n; k++)
    {
        sum += 1.0 / k;
    }
    return sum;
}
