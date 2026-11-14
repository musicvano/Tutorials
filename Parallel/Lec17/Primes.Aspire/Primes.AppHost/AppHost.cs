// Модель розподіленого застосунку: ресурси та зв’язки між ними.
IDistributedApplicationBuilder builder =
    DistributedApplication.CreateBuilder(args);

// Контейнери інфраструктури (Aspire запускає їх у Docker).
var rabbitmq = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin()                  // вебконсоль
    .WithContainerName("pro16-rabbitmq");
var redis = builder.AddRedis("redis")
    .WithContainerName("pro16-redis");

// Проєкти .NET: рядки підключення передає WithReference.
var api = builder.AddProject<Projects.Primes_Api>("api")
    .WithReference(rabbitmq).WaitFor(rabbitmq)
    .WithReference(redis).WaitFor(redis)
    .WithHttpHealthCheck("/health/ready", endpointName: "http")
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.Primes_Worker>("worker")
    .WithReference(rabbitmq).WaitFor(rabbitmq)
    .WithReference(redis).WaitFor(redis)
    .WithReplicas(3);                        // три процеси

builder.Build().Run();
