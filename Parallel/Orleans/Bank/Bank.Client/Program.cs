using System.Diagnostics;
using Bank;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

Console.OutputEncoding = System.Text.Encoding.UTF8;
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Logging.SetMinimumLevel(LogLevel.Warning);
builder.UseOrleansClient(client => client.UseLocalhostClustering());
using IHost host = builder.Build();
await host.StartAsync();               // з’єднання зі шлюзом силосу
IClusterClient cluster =
    host.Services.GetRequiredService<IClusterClient>();

if (args is ["balance", var number])   // лише переглянути рахунок
{
    Print(await cluster.GetGrain<IAccountGrain>(number).GetInfo());
}
else
{
    await RunDemoAsync(cluster);
}
await host.StopAsync();                // коректне від’єднання

static async Task RunDemoAsync(IClusterClient cluster)
{
    IAccountGrain olena = cluster.GetGrain<IAccountGrain>("UA-001");
    IAccountGrain bohdan = cluster.GetGrain<IAccountGrain>("UA-002");
    await olena.Deposit(1000);

    // 1000 одночасних викликів одного зерна без блокувань.
    Stopwatch clock = Stopwatch.StartNew();
    await Task.WhenAll(Enumerable.Range(0, 1000)
        .Select(_ => bohdan.Deposit(1)));
    Console.WriteLine($"1000 поповнень по 1 грн: " +
        $"{clock.ElapsedMilliseconds} мс");

    await olena.Transfer("UA-002", 250);
    try
    {
        await bohdan.Transfer("UA-001", 5000);
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Відмова: {ex.Message}");
    }
    Print(await olena.GetInfo());
    Print(await bohdan.GetInfo());
}

static void Print(AccountInfo a) => Console.WriteLine(
    $"{a.Number}: {a.Balance,8:N2} грн, операцій {a.Operations}, " +
    $"силос {a.Silo}");
