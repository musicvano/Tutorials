using Microsoft.EntityFrameworkCore;
using Shop;

// Два користувачі одночасно купують одну гру
await using var alice = new ShopContext();
await using var bob = new ShopContext();
var forAlice = await alice.Products
    .SingleAsync(p => p.Name == "Chess");
var forBob = await bob.Products
    .SingleAsync(p => p.Name == "Chess");
Console.WriteLine($"Both read stock: {forBob.Stock}");

forAlice.Stock -= 2;
await alice.SaveChangesAsync();
Console.WriteLine($"Alice bought 2, stock: {forAlice.Stock}");

int wanted = 11;
forBob.Stock -= wanted;
while (true)
{
    try
    {
        await bob.SaveChangesAsync();
        Console.WriteLine($"Bob bought {wanted}");
        break;
    }
    catch (DbUpdateConcurrencyException ex)
    {
        var entry = ex.Entries.Single();
        var current = await entry.GetDatabaseValuesAsync();
        if (current is null)
        {
            Console.WriteLine("Product was deleted");
            break;
        }
        int stock = current.GetValue<int>(nameof(Product.Stock));
        Console.WriteLine($"Conflict: stock is now {stock}");
        entry.OriginalValues.SetValues(current);   // новий xmin
        wanted = Math.Min(wanted, stock);
        forBob.Stock = stock - wanted;             // повторна спроба
    }
}
