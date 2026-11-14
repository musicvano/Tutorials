Console.OutputEncoding = System.Text.Encoding.UTF8;

Product[] cart =
[
    new Product { Name = "Laptop", Price = 32_999m, Quantity = 1,
                  DiscountPercent = 7 },
    new Product { Name = "Mouse", Price = 449.5m, Quantity = 2 },
    new Product { Name = "USB-C cable", Price = 199m, Quantity = 3,
                  DiscountPercent = 15 },
];

decimal total = 0;
Console.WriteLine($"{"Product",-14}{"Price",10}{"Qty",6}"
    + $"{"Discount",8}{"Amount",12}");
foreach (Product p in cart)
{
    Console.WriteLine($"{p.Name,-14}{p.Price,10:N2}{p.Quantity,6}"
        + $"{p.DiscountPercent,7}%{p.Cost,12:N2}");
    total += p.Cost;
}
Console.WriteLine($"{"Total",-38}{total,12:N2}");

class Product
{
    public required string Name { get; init; }
    public required decimal Price { get; init; }
    public int Quantity { get; init; } = 1;
    public int DiscountPercent { get; init; }

    // Cost including the discount, rounded to kopiykas.
    public decimal Cost =>
        Math.Round(Price * Quantity * (100 - DiscountPercent) / 100,
            2);
}
