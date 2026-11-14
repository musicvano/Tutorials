Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

long numerator = 0, denominator = 1;
Console.WriteLine("Enter fractions a/b (empty line to finish):");

while (true)
{
    string? line = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(line))
    {
        break;
    }
    try
    {
        (long a, long b) = ParseFraction(line);
        numerator = checked(numerator * b + a * denominator);
        denominator = checked(denominator * b);
        long gcd = Gcd(Math.Abs(numerator), denominator);
        numerator /= gcd;
        denominator /= gcd;
        Console.WriteLine($"  sum = {numerator}/{denominator}");
    }
    catch (FormatException ex)
    {
        Console.WriteLine($"  {ex.Message} Try again.");
    }
    catch (DivideByZeroException)
    {
        Console.WriteLine("  The denominator cannot be zero.");
    }
    catch (OverflowException)
    {
        Console.WriteLine("  The sum is too large; fraction skipped.");
    }
}

Console.WriteLine($"Result: {numerator}/{denominator}"
    + $" ≈ {(double)numerator / denominator:F4}");

// Parses “a/b”; moves the sign to the numerator.
static (long, long) ParseFraction(string text)
{
    string[] parts = text.Split('/');
    if (parts.Length != 2
        || !long.TryParse(parts[0], out long a)
        || !long.TryParse(parts[1], out long b))
    {
        throw new FormatException($"“{text}” is not an a/b fraction.");
    }

    if (b == 0)
    {
        throw new DivideByZeroException();
    }
    return b < 0 ? (-a, -b) : (a, b);
}

static long Gcd(long a, long b) =>
    b == 0 ? Math.Max(a, 1) : Gcd(b, a % b);
