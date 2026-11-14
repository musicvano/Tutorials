using System.Diagnostics;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Shop.Contracts;

namespace Shop.Orders;

public enum OrderStatus { Pending, Reserved, Confirmed, Compensating,
    Cancelled }

public class Order
{
    public Guid Id { get; set; }
    public string? IdempotencyKey { get; set; }
    public required string Customer { get; set; }
    public List<OrderLine> Lines { get; set; } = [];
    public decimal Total { get; set; }
    public OrderStatus Status { get; set; }
    public string? Reason { get; set; }
    public DateTime UpdatedAt { get; set; }
}

// Рядок вихідної скриньки: подія, яку ще треба опублікувати.
public class OutboxMessage
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public required string Type { get; set; }   // routing key
    public required string Payload { get; set; } // JSON події
    public string? TraceParent { get; set; }    // W3C traceparent
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SentAt { get; set; }

    public static OutboxMessage From<T>(string type, T message) =>
        new()
    {
        Type = type,
        Payload = JsonSerializer.Serialize(message),
        TraceParent = Activity.Current?.Id,
    };
}

public class OrdersDb(DbContextOptions<OrdersDb> options)
    : DbContext(options)
{
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OutboxMessage> Outbox => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Order>().Property(o => o.Status)
            .HasConversion<string>();
        b.Entity<Order>().HasIndex(o => o.IdempotencyKey).IsUnique();
        b.Entity<Order>().ComplexCollection(o => o.Lines,
            l => l.ToJson());                       // стовпець jsonb
        b.Entity<OutboxMessage>().HasIndex(m => m.SentAt);
    }
}
