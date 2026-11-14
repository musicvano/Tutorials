using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using Shop.Core;

namespace Shop.Tests;

[TestClass]
public sealed class SalesReportTests
{
    private static readonly string[] Input =
    [
        "Ноутбук;32999,00;1",   // понад 5000 – знижка 3 %
        "Миша;449,50;2",
        "некоректний рядок",
        "Кабель;199;30",        // 5970 – знижка
    ];

    // Очікуваний звіт отримано зі старого коду.
    private const string Expected =
        "Ноутбук       32009,03\n" +
        "Миша            899,00\n" +
        "Кабель         5790,90\n" +
        "Разом: 38698,93 (3 поз.)";

    [ClassInitialize]
    public static void UseUkrainianCulture(TestContext _) =>
        CultureInfo.CurrentCulture = new CultureInfo("uk-UA");

    [TestMethod]
    public void Old_Report_MatchesSnapshot() =>
        Assert.AreEqual(Expected, SalesReportOld.Make(Input));

    [TestMethod]
    public void New_Report_SameAsOld() =>
        Assert.AreEqual(
            SalesReportOld.Make(Input), SalesReport.Make(Input));

    [TestMethod]
    public void New_EmptyInput_OnlyTotal() =>
        Assert.AreEqual("Разом: 0,00 (0 поз.)", SalesReport.Make([]));
}
