using Microsoft.EntityFrameworkCore;

// Сервіс каталогу: власна база catalogdb, лише читання товарів.
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<CatalogDb>("catalogdb");
WebApplication app = builder.Build();
app.MapDefaultEndpoints();

await using (AsyncServiceScope scope =
    app.Services.CreateAsyncScope())
{
    CatalogDb db = scope.ServiceProvider
        .GetRequiredService<CatalogDb>();
    if (await db.Database.EnsureCreatedAsync())   // перший запуск
    {
        db.Products.AddRange(
            new Product { Id = 1, Name = "Ноутбук", Price = 32000m },
            new Product { Id = 2, Name = "Миша", Price = 650m },
            new Product { Id = 3, Name = "Кабель USB-C",
                Price = 250m });
        await db.SaveChangesAsync();
    }
}

app.MapGet("/products", (CatalogDb db) =>
    db.Products.AsNoTracking().OrderBy(p => p.Id).ToListAsync());
app.MapGet("/products/{id:int}", async (int id, CatalogDb db) =>
    await db.Products.FindAsync(id) is Product p
        ? Results.Ok(p) : Results.NotFound());

app.Run();

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
}

public class CatalogDb(DbContextOptions<CatalogDb> options)
    : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
}
