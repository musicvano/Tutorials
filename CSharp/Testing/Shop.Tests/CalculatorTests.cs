using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shop.Core;

namespace Shop.Tests;

[TestClass]
public sealed class CalculatorTests
{
    private readonly Calculator calculator = new();

    [TestMethod]
    public void Add_TwoPositiveNumbers_ReturnsSum()
    {
        // Arrange
        int a = 2, b = 3;

        // Act
        int result = calculator.Add(a, b);

        // Assert
        Assert.AreEqual(5, result);
    }

    [TestMethod]
    [DataRow(0, 0, 0)]
    [DataRow(-5, 5, 0)]
    [DataRow(int.MaxValue - 1, 1, int.MaxValue)]
    public void Add_BoundaryValues_ReturnsSum(
        int a, int b, int expected) =>
        Assert.AreEqual(expected, calculator.Add(a, b));

    [TestMethod]
    public void Add_Overflow_Throws() =>
        Assert.ThrowsExactly<OverflowException>(
            () => calculator.Add(int.MaxValue, 1));

    [TestMethod]
    public void Divide_OneByThree_ReturnsApproximately()
    {
        double result = calculator.Divide(1, 3);
        Assert.AreEqual(0.3333, result, delta: 0.0001);
    }

    [TestMethod]
    public void Divide_ByZero_ThrowsWithMessage()
    {
        var e = Assert.ThrowsExactly<DivideByZeroException>(
            () => calculator.Divide(1, 0));
        StringAssert.Contains(e.Message, "0");
    }
}
