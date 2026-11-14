using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

Console.OutputEncoding = System.Text.Encoding.UTF8;
bool redis = args.Contains("--redis");

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Logging.SetMinimumLevel(LogLevel.Warning);
builder.UseOrleans(silo =>
{
    silo.UseLocalhostClustering();   // порти 11111 і 30000
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

using IHost host = builder.Build();
await host.StartAsync();
Console.WriteLine($"Силос запущено, сховище: " +
    (redis ? "Redis" : "пам’ять") + ". Ctrl+C – зупинка.");
await host.WaitForShutdownAsync();
