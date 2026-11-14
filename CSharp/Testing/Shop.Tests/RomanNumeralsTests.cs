using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shop.Core;

namespace Shop.Tests;

[TestClass]
public sealed class RomanNumeralsTests
{
    // Порядок, у якому тести додавалися під час TDD.
    [TestMethod]
    [DataRow(1, "I")]          // 1: найпростіший випадок
    [DataRow(3, "III")]        // 2: повторення символу
    [DataRow(5, "V")]          // 3: новий символ
    [DataRow(4, "IV")]         // 4: віднімання
    [DataRow(9, "IX")]
    [DataRow(14, "XIV")]       // 5: комбінації
    [DataRow(40, "XL")]
    [DataRow(1994, "MCMXCIV")] // 6: усі правила разом
    [DataRow(3999, "MMMCMXCIX")]
    public void ToRoman_ValidNumber_ReturnsRoman(
        int number, string expected)
    {
        Assert.AreEqual(expected, RomanNumerals.ToRoman(number));
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-7)]
    [DataRow(4000)]
    public void ToRoman_OutOfRange_Throws(int number)
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => RomanNumerals.ToRoman(number));
    }
}
