Console.OutputEncoding = System.Text.Encoding.UTF8;

double[] temperatures = [12.5, 15.1, 9.8, 17.4, 14.0, 11.2, 16.3];

// A tuple variable with named elements.
var stats = GetStats(temperatures);
Console.WriteLine($"Tuple: {stats}");
Console.WriteLine($"Average: {stats.Average:F2} °C");

// Deconstruction into separate variables; the average is discarded.
var (min, max, _) = GetStats(temperatures);
Console.WriteLine($"Range: {min} … {max}, spread {max - min:F1}");

// Tuples are compared element by element.
Console.WriteLine($"Match: {(min, max) == (9.8, 17.4)}");

// Swapping values without a temporary variable.
(min, max) = (max, min);
Console.WriteLine($"After swapping: min = {min}, max = {max}");

static (double Min, double Max, double Average) GetStats(
    double[] values)
{
    if (values.Length == 0)
    {
        throw new ArgumentException("the array is empty", nameof(values));
    }

    double min = values[0], max = values[0], sum = 0;
    foreach (double v in values)
    {
        min = Math.Min(min, v);
        max = Math.Max(max, v);
        sum += v;
    }
    return (min, max, sum / values.Length);
}
