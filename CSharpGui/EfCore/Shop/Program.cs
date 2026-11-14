using Microsoft.EntityFrameworkCore;
using Shop;

await using var db = new ShopContext();
if (!await db.Categories.AnyAsync())
{
    var sale = new Tag { Name = "sale" };
    var hit = new Tag { Name = "hit" };
    var books = new Category { Name = "Books" };
    var games = new Category { Name = "Games" };
    Product P(string name, decimal price, int stock, Category c,
        params Tag[] tags)
    {
        var p = new Product
        {
            Name = name, Price = price, Stock = stock, Category = c
        };
        p.Tags.AddRange(tags);
        return p;
    }
    db.Products.AddRange(
        P("C# in Depth", 1250m, 4, books, hit),
        P("Clean Code", 890.5m, 0, books, sale),
        P("Pro EF Core", 1490m, 7, books),
        P("Chess", 640m, 12, games, sale, hit),
        P("Carcassonne", 1720m, 3, games));
    await db.SaveChangesAsync();
}

// 1:N – категорії з товарами, відсортованими за ціною
var categories = await db.Categories
    .OrderBy(c => c.Name)
    .Include(c => c.Products.OrderBy(p => p.Price))
    .ThenInclude(p => p.Tags.OrderBy(t => t.Name))
    .AsNoTracking()
    .ToListAsync();
foreach (var c in categories)
{
    Console.WriteLine($"{c.Name}:");
    foreach (var p in c.Products)
    {
        var tags = string.Join(", ", p.Tags.Select(t => t.Name));
        Console.WriteLine($"  {p.Name,-12} {p.Price,9:N2} {tags}");
    }
}

// Проєкція в DTO і пагінація: друга сторінка по 2 товари
const int PageSize = 2;
int pageNo = 2;
List<ProductRow> page = await db.Products
    .OrderBy(p => p.Price)
    .Skip((pageNo - 1) * PageSize)
    .Take(PageSize)
    .Select(p => new ProductRow(p.Name, p.Category.Name, p.Price))
    .ToListAsync();
Console.WriteLine($"Page {pageNo}: " +
    string.Join("; ", page.Select(r => $"{r.Name} ({r.Category})")));

// Групування й агрегати виконуються в базі даних
var stats = await db.Products
    .GroupBy(p => p.Category.Name)
    .Select(g => new
    {
        Category = g.Key,
        Count = g.Count(),
        InStock = g.Sum(p => p.Stock),
        MaxPrice = g.Max(p => p.Price)
    })
    .OrderBy(s => s.Category)
    .ToListAsync();
foreach (var s in stats)
    Console.WriteLine($"{s.Category}: {s.Count} products, " +
        $"{s.InStock} in stock, max {s.MaxPrice:N2}");

record ProductRow(string Name, string Category, decimal Price);
