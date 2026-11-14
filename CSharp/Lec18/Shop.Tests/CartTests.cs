using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shop.Core;

namespace Shop.Tests;

[TestClass]
public sealed class CartTests
{
    // Фейк: фіксовані ціни без бази даних і мережі.
    private sealed class FakePriceProvider : IPriceProvider
    {
        private readonly Dictionary<string, decimal> prices = new()
        {
            ["pen"] = 25m,
            ["book"] = 450m,
        };

        public int Calls { get; private set; }

        public decimal GetPrice(string sku)
        {
            Calls++;
            return prices[sku];
        }
    }

    [TestMethod]
    public void Total_EmptyCart_IsZero()
    {
        Cart cart = new(new FakePriceProvider());
        Assert.AreEqual(0m, cart.Total);
    }

    [TestMethod]
    public void Total_BelowThreshold_NoDiscount()
    {
        Cart cart = new(new FakePriceProvider());
        cart.Add("book", 2);
        cart.Add("pen", 2);

        Assert.AreEqual(950m, cart.Total);
    }

    [TestMethod]
    public void Total_AtThreshold_TenPercentDiscount()
    {
        Cart cart = new(new FakePriceProvider());
        cart.Add("book", 2);
        cart.Add("pen", 4);                 // рівно 1000

        Assert.AreEqual(900m, cart.Total);
    }

    [TestMethod]
    public void Add_SameSkuTwice_AsksPriceOnce()
    {
        FakePriceProvider fake = new();
        Cart cart = new(fake);
        cart.Add("pen", 1);
        cart.Add("pen", 3);

        Assert.AreEqual(100m, cart.Total);
        Assert.AreEqual(1, fake.Calls);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public void Add_NonPositiveQuantity_Throws(int quantity)
    {
        Cart cart = new(new FakePriceProvider());
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => cart.Add("pen", quantity));
    }
}
