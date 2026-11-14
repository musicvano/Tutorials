using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shop.Core;

namespace Shop.Tests;

[TestClass]
public sealed class PayrollTests
{
    // Характеризаційні тести фіксують поведінку старого коду.
    [TestMethod]
    [DataRow("hourly", 200, 120, 1, 19320.00)]
    [DataRow("hourly", 200, 160, 5, 28336.00)]
    [DataRow("hourly", 200, 180, 2, 30590.00)]
    [DataRow("salary", 30000, 0, 7, 26565.00)]
    [DataRow("salary", 30000, 0, 4, 24150.00)]
    [DataRow("unknown", 30000, 100, 1, 0.00)]
    public void Old_KnownCases(
        string type, double rate, int hours, int years,
        double expected)
    {
        decimal pay =
            PayrollOld.Calc(type, (decimal)rate, hours, years);
        Assert.AreEqual((decimal)expected, pay);
    }

    // Той самий набір для нового коду: поведінка не змінилася.
    [TestMethod]
    [DataRow(PayType.Hourly, 200, 120, 1, 19320.00)]
    [DataRow(PayType.Hourly, 200, 160, 5, 28336.00)]
    [DataRow(PayType.Hourly, 200, 180, 2, 30590.00)]
    [DataRow(PayType.Salary, 30000, 0, 7, 26565.00)]
    [DataRow(PayType.Salary, 30000, 0, 4, 24150.00)]
    public void New_SameResults(
        PayType type, double rate, int hours, int years,
        double expected)
    {
        decimal pay =
            Payroll.NetPay(type, (decimal)rate, hours, years);
        Assert.AreEqual((decimal)expected, pay);
    }
}
