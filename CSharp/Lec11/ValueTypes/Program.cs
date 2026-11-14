Console.OutputEncoding = System.Text.Encoding.UTF8;

double[] temperatures = [12.5, 15.1, 9.8, 17.4, 14.0, 11.2, 16.3];

// Змінна кортежу з іменованими елементами.
var stats = GetStats(temperatures);
Console.WriteLine($"Кортеж: {stats}");
Console.WriteLine($"Середня: {stats.Average:F2} °C");

// Деконструкція в окремі змінні; середнє відкинуто.
var (min, max, _) = GetStats(temperatures);
Console.WriteLine($"Діапазон: {min} … {max}, розмах {max - min:F1}");

// Порівняння кортежів поелементне.
Console.WriteLine($"Збіг: {(min, max) == (9.8, 17.4)}");

// Обмін значень без тимчасової змінної.
(min, max) = (max, min);
Console.WriteLine($"Після обміну: min = {min}, max = {max}");

static (double Min, double Max, double Average) GetStats(
    double[] values)
{
    if (values.Length == 0)
    {
        throw new ArgumentException("масив порожній", nameof(values));
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
