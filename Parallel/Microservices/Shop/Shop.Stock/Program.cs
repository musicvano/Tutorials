using Shop.Stock;

// Сервіс складу: власна база stockdb, резервування через gRPC.
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<StockDb>("stockdb");
builder.Services.AddGrpc();
WebApplication app = builder.Build();
app.MapDefaultEndpoints();

await using (AsyncServiceScope scope =
    app.Services.CreateAsyncScope())
{
    StockDb db = scope.ServiceProvider.GetRequiredService<StockDb>();
    if (await db.Database.EnsureCreatedAsync())   // перший запуск
    {
        db.Items.AddRange(
            new StockItem { ProductId = 1, Available = 10 },
            new StockItem { ProductId = 2, Available = 5 },
            new StockItem { ProductId = 3, Available = 100000 });
        await db.SaveChangesAsync();
    }
}

app.MapGrpcService<InventoryService>();
app.Run();
