using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Shop;

public class Category
{
    public int Id { get; set; }
    [MaxLength(100)]
    public required string Name { get; set; }
    public List<Product> Products { get; } = [];      // 1:N
}

public class Product
{
    public int Id { get; set; }
    [MaxLength(200)]
    public required string Name { get; set; }
    [Precision(10, 2)]
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public List<Tag> Tags { get; } = [];              // N:M
    [Timestamp]
    public uint Version { get; set; }                 // xmin
}

public class Tag
{
    public int Id { get; set; }
    [MaxLength(50)]
    public required string Name { get; set; }
    public List<Product> Products { get; } = [];      // N:M
}

public class ShopContext : DbContext
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Tag> Tags => Set<Tag>();

    // Рядок з’єднання: секрети користувача або змінна середовища
    private static readonly string? ConnectionString =
        new ConfigurationBuilder()
            .AddUserSecrets<ShopContext>()
            .AddEnvironmentVariables()
            .Build()
            .GetConnectionString("Shop");

    protected override void OnConfiguring(
        DbContextOptionsBuilder options) =>
        options.UseNpgsql(ConnectionString);

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<Tag>().HasIndex(t => t.Name).IsUnique();
        model.Entity<Product>().ToTable(t => t.HasCheckConstraint(
            "CK_Products_Stock", "\"Stock\" >= 0"));
    }
}
