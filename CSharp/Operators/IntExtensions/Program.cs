Console.OutputEncoding = System.Text.Encoding.UTF8;

while (true)
{
    Console.Write("Number (empty line to exit): ");
    string? input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
    {
        break;
    }
    if (!int.TryParse(input, out int n) || n <= 0)
    {
        Console.WriteLine("  a natural number is required");
        continue;
    }

    // Extension methods are called like instance methods of int.
    Console.WriteLine($"  digits: {string.Join(" ", n.Digits())}");
    Console.WriteLine($"  prime: {(n.IsPrime() ? "yes" : "no")}");
    Console.WriteLine(n <= 3999
        ? $"  Roman: {n.ToRoman()}"
        : "  Roman: only up to 3999");
}

static class IntExtensions
{
    public static bool IsPrime(this int n)
    {
        if (n < 2) return false;
        for (int d = 2; d * d <= n; d++)
        {
            if (n % d == 0) return false;
        }
        return true;
    }

    public static int[] Digits(this int n)
    {
        string text = Math.Abs(n).ToString();
        int[] digits = new int[text.Length];
        for (int i = 0; i < text.Length; i++)
        {
            digits[i] = text[i] - '0';
        }
        return digits;
    }

    public static string ToRoman(this int n)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(n, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(n, 3999);
        int[] values =
            [1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1];
        string[] symbols =
            ["M", "CM", "D", "CD", "C", "XC", "L", "XL",
             "X", "IX", "V", "IV", "I"];
        var roman = new System.Text.StringBuilder();
        for (int i = 0; i < values.Length; i++)
        {
            while (n >= values[i])
            {
                roman.Append(symbols[i]);
                n -= values[i];
            }
        }
        return roman.ToString();
    }
}
