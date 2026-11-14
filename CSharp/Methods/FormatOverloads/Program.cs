using System.Globalization;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var date = new DateTime(2026, 9, 1);
Console.WriteLine(Formatter.Format(date));
Console.WriteLine(Formatter.Format(date, withWeekday: true));
Console.WriteLine(Formatter.Format(1299.5m));
Console.WriteLine(Formatter.Format(49.99m, currency: "$"));
Console.WriteLine(Formatter.Format(0.157));
Console.WriteLine(Formatter.Format(-0.0345, decimals: 2,
    showSign: true));
Console.WriteLine(Formatter.Format(fraction: 0.5, showSign: true));

static class Formatter
{
    static readonly CultureInfo Ukrainian = new("uk-UA");

    // Date: “01.09.2026” or “вівторок, 01.09.2026”.
    public static string Format(DateTime value,
        bool withWeekday = false)
    {
        string text = value.ToString("dd.MM.yyyy");
        return withWeekday
            ? $"{value.ToString("dddd", Ukrainian)}, {text}"
            : text;
    }

    // Money: digit grouping, two decimal places, currency.
    public static string Format(decimal amount,
        string currency = "UAH") =>
        $"{amount.ToString("N2", Ukrainian)} {currency}";

    // Fraction as a percentage: 0.157 → “15,7 %”.
    public static string Format(double fraction, int decimals = 1,
        bool showSign = false)
    {
        string number = (fraction * 100).ToString("F" + decimals,
            Ukrainian);
        string sign = showSign && fraction > 0 ? "+" : "";
        return $"{sign}{number} %";
    }
}
