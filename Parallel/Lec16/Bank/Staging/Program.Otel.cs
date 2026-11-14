// Staging copy of Bank.Silo/Program.cs for screenshot 16/06 (not a lecture listing):
// the lecture's silo plus the three steps of «Моніторинг: OpenTelemetry і дашборд Aspire».
// Built only with: dotnet run -c Release -p:Otel=true -- --redis
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using StackExchange.Redis;

Console.OutputEncoding = System.Text.Encoding.UTF8;
bool redis = args.Contains("--redis");

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Logging.SetMinimumLevel(LogLevel.Warning);
builder.UseOrleans(silo =>
{
    silo.UseLocalhostClustering();   // порти 11111 і 30000
    silo.AddActivityPropagation();
    if (redis)
    {
        silo.AddRedisGrainStorage("bank", options =>
            options.ConfigurationOptions =
                ConfigurationOptions.Parse("localhost:6379"));
    }
    else
    {
        silo.AddMemoryGrainStorage("bank");
    }
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r.AddService("bank-silo"))
    .WithMetrics(m => m.AddMeter("Microsoft.Orleans"))
    .WithTracing(t => t
        .AddSource("Microsoft.Orleans.Application")
        .AddSource("Microsoft.Orleans.Lifecycle"))
    .UseOtlpExporter();            // http://localhost:4317 (gRPC)
builder.Logging.AddOpenTelemetry(o => o.IncludeScopes = true);

using IHost host = builder.Build();
await host.StartAsync();
Console.WriteLine($"Силос запущено, сховище: " +
    (redis ? "Redis" : "пам’ять") + ". Ctrl+C – зупинка.");
await host.WaitForShutdownAsync();
