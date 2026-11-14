using Microsoft.EntityFrameworkCore;
using Shop;

await using var db = new ShopContext();

var chess = await db.Products.SingleAsync(p => p.Name == "Chess");
Console.WriteLine($"Loaded: {db.Entry(chess).State}");

chess.Price = 590m;                                  // Modified
var puzzle = new Product
    { Name = "Puzzle", Price = 310m, Stock = 5,
      CategoryId = chess.CategoryId };
db.Products.Add(puzzle);                             // Added
var old = await db.Products
    .SingleAsync(p => p.Name == "Carcassonne");
db.Products.Remove(old);                             // Deleted

db.ChangeTracker.DetectChanges();       // оновити стани
Console.Write(db.ChangeTracker.DebugView.ShortView);
var price = db.Entry(chess).Property(p => p.Price);
Console.WriteLine($"Price: {price.OriginalValue} -> " +
    $"{price.CurrentValue}");

int saved = await db.SaveChangesAsync();
Console.WriteLine($"Saved: {saved}; Chess: {db.Entry(chess).State}" +
    $", Carcassonne: {db.Entry(old).State}, Puzzle Id: {puzzle.Id}");

// Запит без відстеження: об’єкти не потрапляють у ChangeTracker
db.ChangeTracker.Clear();
var all = await db.Products.AsNoTracking().ToListAsync();
Console.WriteLine($"Read {all.Count} products, tracked: " +
    $"{db.ChangeTracker.Entries().Count()}");

// Пакетні операції: один SQL-запит, без завантаження об’єктів
int updated = await db.Products
    .Where(p => p.Category.Name == "Books")
    .ExecuteUpdateAsync(s =>
        s.SetProperty(p => p.Price, p => p.Price * 0.9m));
int deleted = await db.Products
    .Where(p => p.Stock == 0)
    .ExecuteDeleteAsync();
Console.WriteLine($"Updated: {updated}, deleted: {deleted}");
