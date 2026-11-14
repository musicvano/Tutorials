using Microsoft.EntityFrameworkCore;

// Платіжний сервіс: власна база paymentsdb; операції ідемпотентні
// за номером замовлення (первинний ключ OrderId).
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<PaymentsDb>("paymentsdb");
WebApplication app = builder.Build();
app.MapDefaultEndpoints();
decimal limit = app.Configuration.GetValue("Payments:Limit", 50000m);
double failureRate =
    app.Configuration.GetValue("Payments:FailureRate", 0.0);

await using (AsyncServiceScope scope =
    app.Services.CreateAsyncScope())
    await scope.ServiceProvider.GetRequiredService<PaymentsDb>()
        .Database.EnsureCreatedAsync();

app.MapPost("/payments", async (PaymentRequest req, PaymentsDb db,
    ILogger<Program> log) =>
{
    // Імітація збою інфраструктури (для перевірки повторів).
    if (Random.Shared.NextDouble() < failureRate)
        return Results.StatusCode(503);
    // Повторний запит повертає збережений результат.
    Payment? p = await db.Payments.FindAsync(req.OrderId);
    if (p is null)
    {
        p = new Payment { OrderId = req.OrderId, Amount = req.Amount,
            Status = req.Amount <= limit ? "Charged" : "Declined" };
        db.Payments.Add(p);
        await db.SaveChangesAsync();
        log.LogInformation("Платіж {Order}: {Amount} грн, {Status}",
            p.OrderId, p.Amount, p.Status);
    }
    return p.Status == "Declined"
        ? Results.Json(p, statusCode: 402)   // Payment Required
        : Results.Ok(p);
});

// Компенсація: повернути кошти (ідемпотентно).
app.MapPost("/payments/{orderId:guid}/refund", async (Guid orderId,
    PaymentsDb db, ILogger<Program> log) =>
{
    Payment? p = await db.Payments.FindAsync(orderId);
    if (p is { Status: "Charged" })
    {
        p.Status = "Refunded";
        await db.SaveChangesAsync();
        log.LogInformation("Платіж {Order}: повернуто", orderId);
    }
    return Results.Ok(new { orderId, status = p?.Status ?? "None" });
});

app.Run();

public record PaymentRequest(Guid OrderId, string Customer,
    decimal Amount);

public class Payment
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public required string Status { get; set; }
}

public class PaymentsDb(DbContextOptions<PaymentsDb> options)
    : DbContext(options)
{
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder b) =>
        b.Entity<Payment>().HasKey(p => p.OrderId);
}
