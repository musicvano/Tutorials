using Shop.Shipping;

// Сервіс доставки: служба без HTTP, споживач подій order.confirmed.
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<ShippingDb>("shippingdb");
builder.AddRabbitMQClient("rabbitmq");        // IConnection
builder.Services.AddHostedService<OrderConfirmedConsumer>();
IHost host = builder.Build();

await using (AsyncServiceScope scope =
    host.Services.CreateAsyncScope())
    await scope.ServiceProvider.GetRequiredService<ShippingDb>()
        .Database.EnsureCreatedAsync();

host.Run();
