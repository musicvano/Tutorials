Console.OutputEncoding = System.Text.Encoding.UTF8;

double[] times = [12.4, 9.8, 15.1, 11.0, 10.6, 30.2, 11.9];

// Custom operators are called in a chain, like standard ones.
var usual = times
    .WhereNot(t => t > 20)          // discard anomalies
    .OrderBy(t => t);
Console.WriteLine($"Without anomalies: {string.Join("; ", usual)}");
Console.WriteLine($"Median of all: {times.Median():F2}");
Console.WriteLine($"Median without anomalies: {usual.Median():F2}");
Console.WriteLine($"Average of all: {times.Average():F2}");

try
{
    Array.Empty<double>().Median();
}
catch (InvalidOperationException e)
{
    Console.WriteLine($"Error: {e.Message}");
}

static class LinqExtensions
{
    // A deferred operator: an iterator with yield return.
    public static IEnumerable<T> WhereNot<T>(
        this IEnumerable<T> source, Func<T, bool> predicate)
    {
        foreach (T item in source)
        {
            if (!predicate(item))
            {
                yield return item;
            }
        }
    }

    // An immediate operator: calculates the value right away.
    public static double Median(this IEnumerable<double> source)
    {
        double[] sorted = source.Order().ToArray();
        if (sorted.Length == 0)
        {
            throw new InvalidOperationException("no elements");
        }
        int middle = sorted.Length / 2;
        return sorted.Length % 2 == 1
            ? sorted[middle]
            : (sorted[middle - 1] + sorted[middle]) / 2;
    }
}
