Console.OutputEncoding = System.Text.Encoding.UTF8;

// Client code works only with its own interface.
ICurrencyConverter converter =
    new LegacyConverterAdapter(new LegacyRatesService());
PriceList prices = new(converter);

prices.Print(["Laptop", "Headphones"], [32_999m, 2_499m], "USD");
prices.Print(["Laptop"], [32_999m], "EUR");
prices.Print(["Laptop"], [32_999m], "GBP");

// The interface our program expects.
interface ICurrencyConverter
{
    decimal Convert(decimal amount, string from, string to);
}

// A third-party class: it cannot be changed, and its interface is incompatible.
class LegacyRatesService
{
    private readonly Dictionary<string, double> ratesToUah = new()
    {
        ["USD"] = 41.25, ["EUR"] = 48.10, ["UAH"] = 1.0,
    };

    // Returns -1 if a currency is unknown; the pair is in the "UAH>USD" format.
    public double Exchange(string pair, double value)
    {
        string[] codes = pair.Split('>');
        if (!ratesToUah.TryGetValue(codes[0], out double from)
            || !ratesToUah.TryGetValue(codes[1], out double to))
        {
            return -1;
        }

        return value * from / to;
    }
}

// The adapter converts calls and errors into the format of our interface.
class LegacyConverterAdapter(LegacyRatesService service)
    : ICurrencyConverter
{
    public decimal Convert(decimal amount, string from, string to)
    {
        double result =
            service.Exchange($"{from}>{to}", (double)amount);
        if (result < 0)
        {
            throw new ArgumentException($"no rate for {from} → {to}");
        }
        return Math.Round((decimal)result, 2);
    }
}

class PriceList(ICurrencyConverter converter)
{
    public void Print(
        string[] names, decimal[] pricesUah, string currency)
    {
        try
        {
            for (int i = 0; i < names.Length; i++)
            {
                decimal price =
                    converter.Convert(pricesUah[i], "UAH", currency);
                Console.WriteLine(
                    $"{names[i],-10} {price,9:N2} {currency}");
            }
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }
}
