Console.OutputEncoding = System.Text.Encoding.UTF8;

var boiling = Temperature.FromCelsius(100);
var body = Temperature.FromFahrenheit(98.6);
var space = Temperature.FromKelvin(2.7);

Console.WriteLine(boiling);
Console.WriteLine(body);
Console.WriteLine(space);
Console.WriteLine($"Absolute zero: {Temperature.AbsoluteZero}");

string[] inputs = ["36,6C", "451F", "-300C", "abc"];
foreach (string text in inputs)
{
    Console.WriteLine(Temperature.TryParse(text, out Temperature? t)
        ? $"{text,-6} → {t}"
        : $"{text,-6} → invalid value");
}

class Temperature
{
    public static readonly Temperature AbsoluteZero = new(-273.15);

    private readonly double celsius;

    // Private constructor: objects are created only by class methods.
    private Temperature(double celsius)
    {
        if (celsius < -273.15)
        {
            throw new ArgumentOutOfRangeException(nameof(celsius),
                "Below absolute zero.");
        }
        this.celsius = celsius;
    }

    public double Celsius => celsius;
    public double Fahrenheit => celsius * 9 / 5 + 32;
    public double Kelvin => celsius + 273.15;

    public static Temperature FromCelsius(double c) => new(c);
    public static Temperature FromFahrenheit(double f) =>
        new((f - 32) * 5 / 9);
    public static Temperature FromKelvin(double k) => new(k - 273.15);

    // Parses "36,6C" or "451F"; returns false instead of throwing.
    public static bool TryParse(string text, out Temperature? result)
    {
        result = null;
        if (text.Length < 2
            || !double.TryParse(text[..^1], out double value))
        {
            return false;
        }

        double c = char.ToUpper(text[^1]) switch
        {
            'C' => value,
            'F' => (value - 32) * 5 / 9,
            'K' => value - 273.15,
            _ => double.NaN,
        };
        if (double.IsNaN(c) || c < -273.15)
        {
            return false;
        }
        result = new Temperature(c);
        return true;
    }

    public override string ToString() =>
        $"{celsius:F2} °C = {Fahrenheit:F2} °F = {Kelvin:F2} K";
}
