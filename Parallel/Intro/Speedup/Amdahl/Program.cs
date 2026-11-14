using System.Globalization;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// Частка паралельного коду f задається аргументом: 0,9 або 90%.
double f = args.Length > 0 ? ParseFraction(args[0]) : 0.9;
if (double.IsNaN(f))
{
    Console.Error.WriteLine("Частка f: від 0 до 1 або 0–100%.");
    return 1;
}

Console.WriteLine($"Частка паралельного коду f = {f:P0}");
Console.WriteLine($"{"p",4} {"S(p)",8} {"E(p)",8}");
for (int p = 1; p <= 64; p *= 2)
{
    double speedup = Amdahl(f, p);
    double efficiency = speedup / p;
    Console.WriteLine($"{p,4} {speedup,8:F2} {efficiency,8:P0}");
}

string limit = f < 1 ? $"{1 / (1 - f):F1}" : "∞";
Console.WriteLine($"Межа прискорення 1/(1 - f) = {limit}");
return 0;

// Закон Амдала: S = 1 / ((1 - f) + f / p).
static double Amdahl(double f, int p) => 1 / ((1 - f) + f / p);

static double ParseFraction(string text)
{
    bool percent = text.EndsWith('%');
    string number = text.TrimEnd('%').Replace(',', '.');
    if (!double.TryParse(number, NumberStyles.Float,
            CultureInfo.InvariantCulture, out double value))
    {
        return double.NaN;
    }
    if (percent) value /= 100;
    return value is >= 0 and <= 1 ? value : double.NaN;
}
