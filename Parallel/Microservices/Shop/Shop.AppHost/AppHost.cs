// Модель мікросервісного застосунку Shop.
IDistributedApplicationBuilder builder =
    DistributedApplication.CreateBuilder(args);

// Один контейнер PostgreSQL, але окрема база для кожного сервісу.
var postgres = builder.AddPostgres("postgres")
    .WithContainerName("pro18-postgres");
var catalogDb = postgres.AddDatabase("catalogdb");
var stockDb = postgres.AddDatabase("stockdb");
var paymentsDb = postgres.AddDatabase("paymentsdb");
var ordersDb = postgres.AddDatabase("ordersdb");
var shippingDb = postgres.AddDatabase("shippingdb");
var rabbitmq = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin()
    .WithContainerName("pro18-rabbitmq");

var catalog = builder.AddProject<Projects.Shop_Catalog>("catalog")
    .WithReference(catalogDb).WaitFor(catalogDb);
var stock = builder.AddProject<Projects.Shop_Stock>("stock")
    .WithReference(stockDb).WaitFor(stockDb);
var payments = builder.AddProject<Projects.Shop_Payments>("payments")
    .WithReference(paymentsDb).WaitFor(paymentsDb)
    .WithEnvironment("Payments__Limit", "50000");

var orders = builder.AddProject<Projects.Shop_Orders>("orders")
    .WithReference(ordersDb).WaitFor(ordersDb)
    .WithReference(rabbitmq).WaitFor(rabbitmq)
    .WithReference(catalog).WithReference(stock)
    .WithReference(payments);

builder.AddProject<Projects.Shop_Shipping>("shipping")
    .WithReference(shippingDb).WaitFor(shippingDb)
    .WithReference(rabbitmq).WaitFor(rabbitmq);

builder.AddProject<Projects.Shop_Gateway>("gateway")
    .WithReference(catalog).WithReference(orders)
    .WithExternalHttpEndpoints();

builder.Build().Run();
