using Microsoft.EntityFrameworkCore;

namespace Shop.Shipping;

// Оброблене повідомлення (Inbox): ключ – MessageId.
public class InboxMessage
{
    public Guid MessageId { get; set; }
    public DateTime ProcessedAt { get; set; }
}

// Відправлення підтвердженого замовлення.
public class Shipment
{
    public int Id { get; set; }
    public Guid OrderId { get; set; }
    public required string Customer { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ShippingDb(DbContextOptions<ShippingDb> options)
    : DbContext(options)
{
    public DbSet<InboxMessage> Inbox => Set<InboxMessage>();
    public DbSet<Shipment> Shipments => Set<Shipment>();

    protected override void OnModelCreating(ModelBuilder b) =>
        b.Entity<InboxMessage>().HasKey(m => m.MessageId);
}
