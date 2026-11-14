using System.Text;

namespace Shop.Core;

public static class RomanNumerals
{
    private static readonly (int Value, string Symbol)[] Table =
    [
        (1000, "M"), (900, "CM"), (500, "D"), (400, "CD"),
        (100, "C"), (90, "XC"), (50, "L"), (40, "XL"),
        (10, "X"), (9, "IX"), (5, "V"), (4, "IV"), (1, "I"),
    ];

    public static string ToRoman(int number)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(number, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(number, 3999);

        StringBuilder result = new();
        foreach (var (value, symbol) in Table)
        {
            while (number >= value)
            {
                result.Append(symbol);
                number -= value;
            }
        }
        return result.ToString();
    }
}
