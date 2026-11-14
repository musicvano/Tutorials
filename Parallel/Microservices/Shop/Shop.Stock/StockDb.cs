using Microsoft.EntityFrameworkCore;

namespace Shop.Stock;

// Залишок товару на складі.
public class StockItem
{
    public int ProductId { get; set; }
    public int Available { get; set; }
}

// Резерв одного рядка замовлення.
public class Reservation
{
    public int Id { get; set; }
    public Guid OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public bool Released { get; set; }
}

public class StockDb(DbContextOptions<StockDb> options)
    : DbContext(options)
{
    public DbSet<StockItem> Items => Set<StockItem>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<StockItem>().HasKey(i => i.ProductId);
        b.Entity<StockItem>().Property(i => i.ProductId)
            .ValueGeneratedNever();
        b.Entity<Reservation>().HasIndex(r => r.OrderId);
    }
}
