using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Shop.Contracts;
using Shop.Orders;
using Shop.Stock;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<OrdersDb>("ordersdb");
builder.AddRabbitMQClient("rabbitmq");        // IConnection

// Синхронні залежності: адреси розв’язує виявлення сервісів.
builder.Services.AddHttpClient("catalog",
    c => c.BaseAddress = new Uri("http://catalog"));
builder.Services.AddGrpcClient<Inventory.InventoryClient>(
    o => o.Address = new Uri("http://stock"));
// Для платежів – власні параметри стійкості замість типових.
#pragma warning disable EXTEXP0001  // API поки експериментальне
builder.Services.AddHttpClient("payments",
        c => c.BaseAddress = new Uri("http://payments"))
    .RemoveAllResilienceHandlers()
    .AddStandardResilienceHandler(PaymentsPolicy.Configure);
#pragma warning restore EXTEXP0001

builder.Services.ConfigureHttpJsonOptions(o => o.SerializerOptions
    .Converters.Add(new JsonStringEnumConverter())); // "Confirmed"
builder.Services.AddScoped<OrderSaga>();
builder.Services.AddSingleton<OrderMetrics>();
builder.Services.AddOpenTelemetry()
    .WithMetrics(m => m.AddMeter(OrderMetrics.Name));
builder.Services.AddHostedService<OutboxRelay>();
builder.Services.AddHostedService<SagaRecovery>();
WebApplication app = builder.Build();
app.MapDefaultEndpoints();

await using (AsyncServiceScope scope =
    app.Services.CreateAsyncScope())
    await scope.ServiceProvider.GetRequiredService<OrdersDb>()
        .Database.EnsureCreatedAsync();

app.MapPost("/orders", async (NewOrder req, HttpRequest http,
    OrdersDb db, OrderSaga saga, IHttpClientFactory clients) =>
{
    if (req.Lines is not { Length: > 0 }
        || req.Lines.Any(l => l.Quantity <= 0))
        return Results.BadRequest("потрібні рядки з кількістю > 0");
    // Ключ ідемпотентності: повтор запиту не створює дубліката.
    string? key = http.Headers["Idempotency-Key"];
    if (key is not null && await db.Orders.AsNoTracking()
            .FirstOrDefaultAsync(o => o.IdempotencyKey == key)
        is Order existing)
        return Results.Ok(existing);

    // Ціни – із сервісу каталогу (синхронний виклик).
    HttpClient catalog = clients.CreateClient("catalog");
    decimal total = 0;
    foreach (OrderLine line in req.Lines)
    {
        Product? p = await catalog.GetFromJsonAsync<Product>(
            $"/products/{line.ProductId}");
        total += p!.Price * line.Quantity;
    }
    Order order = new()
    {
        Id = Guid.CreateVersion7(), IdempotencyKey = key,
        Customer = req.Customer, Lines = [.. req.Lines],
        Total = total,
        Status = OrderStatus.Pending, UpdatedAt = DateTime.UtcNow,
    };
    db.Orders.Add(order);
    await db.SaveChangesAsync();
    await saga.RunAsync(order);                // оркестрація саги
    return Results.Created($"/orders/{order.Id}", order);
});

app.MapGet("/orders/{id:guid}", async (Guid id, OrdersDb db) =>
    await db.Orders.FindAsync(id) is Order o
        ? Results.Ok(o) : Results.NotFound());

app.MapGet("/orders", (OrdersDb db) => db.Orders.AsNoTracking()
    .GroupBy(o => o.Status)
    .Select(g => new { status = g.Key.ToString(), count = g.Count() })
    .ToListAsync());

app.Run();

record NewOrder(string Customer, OrderLine[] Lines);
record Product(int Id, string Name, decimal Price);
