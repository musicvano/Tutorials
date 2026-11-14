using System.Diagnostics;
using System.Globalization;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

// Аргумент – кількості потоків через кому, наприклад 1,2,4,8.
int[] threads = args.Length > 0
    ? args[0].Split(',').Select(int.Parse).ToArray()
    : [1, 2, 4, 8, 16];
int[] sizes = [1_000_000, 10_000_000];

StringBuilder csv = new("n,p,time_ms,speedup,efficiency\n");
Console.WriteLine("         n   p   Час, мс      S      E");
foreach (int n in sizes)
{
    int[] source = new int[n];
    Random random = new(2026);
    for (int i = 0; i < n; i++) source[i] = random.Next();
    int[] expected = (int[])source.Clone();
    Array.Sort(expected);

    int[] data = new int[n];
    double Measure(int p) => Median(() =>
    {
        source.CopyTo(data, 0);
        ParallelMergeSort.Sort(data, p);
    });
    double t1 = Measure(1);                // еталонний час T1
    foreach (int p in threads)
    {
        double t = p == 1 ? t1 : Measure(p);
        if (!data.AsSpan().SequenceEqual(expected))
        {
            Console.Error.WriteLine($"Невірний результат, p = {p}");
            return 2;
        }
        double s = t1 / t;
        Console.WriteLine($"{n,10:N0} {p,3} {t,9:F1} {s,6:F2} " +
            $"{s / p,6:F2}");
        csv.AppendLine(string.Create(CultureInfo.InvariantCulture,
            $"{n},{p},{t:F1},{s:F3},{s / p:F3}"));
    }
}
File.WriteAllText("speedup.csv", csv.ToString());
Console.WriteLine("Таблицю збережено у файлі speedup.csv");
return 0;

static double Median(Action action, int runs = 5)
{
    action();
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

static class ParallelMergeSort
{
    // Менші фрагменти вигідніше сортувати послідовно.
    const int Threshold = 16_384;

    public static void Sort(int[] data, int threads)
    {
        int[] buffer = new int[data.Length];
        // Глибина розгалуження: 2^depth гілок ≈ threads.
        int depth = (int)Math.Ceiling(Math.Log2(threads));
        Sort(data, buffer, 0, data.Length, depth);
    }

    static void Sort(int[] a, int[] buffer, int lo, int hi, int depth)
    {
        if (hi - lo <= Threshold)
        {
            Array.Sort(a, lo, hi - lo);
            return;
        }
        int mid = lo + (hi - lo) / 2;
        if (depth > 0)
        {
            Parallel.Invoke(
                () => Sort(a, buffer, lo, mid, depth - 1),
                () => Sort(a, buffer, mid, hi, depth - 1));
        }
        else
        {
            Sort(a, buffer, lo, mid, 0);
            Sort(a, buffer, mid, hi, 0);
        }
        Merge(a, buffer, lo, mid, hi);
    }

    // Злиття двох відсортованих половин через буфер.
    static void Merge(int[] a, int[] buffer, int lo, int mid, int hi)
    {
        int i = lo, j = mid, k = lo;
        while (i < mid && j < hi)
            buffer[k++] = a[i] <= a[j] ? a[i++] : a[j++];
        while (i < mid) buffer[k++] = a[i++];
        while (j < hi) buffer[k++] = a[j++];
        Array.Copy(buffer, lo, a, lo, hi - lo);
    }
}
