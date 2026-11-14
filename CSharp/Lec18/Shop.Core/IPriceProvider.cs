namespace Shop.Core;

// Залежність, яку в тестах замінює фейк.
public interface IPriceProvider
{
    decimal GetPrice(string sku);
}

public class Cart(IPriceProvider prices)
{
    private readonly Dictionary<string, int> items = [];

    public void Add(string sku, int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        items[sku] = items.GetValueOrDefault(sku) + quantity;
    }

    // Знижка 10 % на суму від 1000 грн.
    public decimal Total
    {
        get
        {
            decimal sum = 0;
            foreach (var (sku, quantity) in items)
            {
                sum += prices.GetPrice(sku) * quantity;
            }
            return sum >= 1000m ? sum * 0.9m : sum;
        }
    }
}
