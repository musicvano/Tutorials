using System.Diagnostics;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Runtime;
using StackExchange.Redis;

Console.OutputEncoding = System.Text.Encoding.UTF8;
const string Redis = "localhost:6379";

switch (args)
{
    case ["silo", var n] when int.TryParse(n, out int number):
        await RunSiloAsync(number);
        break;
    case ["client"]:
        await RunClientAsync();
        break;
    default:
        Console.Error.WriteLine(
            "Використання: Sensors silo 1|2 | client");
        Environment.ExitCode = 1;
        break;
}

// Силос N: порти 11110+N (між силосами) і 30000+N (шлюз клієнтів).
static async Task RunSiloAsync(int number)
{
    HostApplicationBuilder builder = Host.CreateApplicationBuilder();
    builder.Logging.SetMinimumLevel(LogLevel.Warning);
    builder.UseOrleans(silo =>
    {
        silo.Configure<ClusterOptions>(o =>
        {
            o.ClusterId = "pro15";         // один кластер
            o.ServiceId = "sensors";       // ключ стану в сховищі
        });
        silo.UseRedisClustering(Redis);  // таблиця членства
        silo.ConfigureEndpoints(IPAddress.Loopback,
            siloPort: 11110 + number, gatewayPort: 30000 + number);
        silo.AddRedisGrainStorage("sensors", o =>
            o.ConfigurationOptions =
                ConfigurationOptions.Parse(Redis));
    });
    using IHost host = builder.Build();
    await host.StartAsync();
    Console.WriteLine($"Силос {number} запущено. Enter – коректна " +
        "зупинка, закриття вікна – аварія.");
    await Task.Run(Console.ReadLine);
    Stopwatch clock = Stopwatch.StartNew();
    await host.StopAsync();          // деактивація зерен, вихід
    Console.WriteLine($"Силос {number} зупинено за " +
        $"{clock.Elapsed.TotalSeconds:F1} с");
}

// Клієнт: щосекунди надсилає показники шести датчиків.
static async Task RunClientAsync()
{
    HostApplicationBuilder builder = Host.CreateApplicationBuilder();
    builder.Logging.SetMinimumLevel(LogLevel.None);
    builder.UseOrleansClient(client =>
    {
        client.Configure<ClusterOptions>(o =>
        {
            o.ClusterId = "pro15";
            o.ServiceId = "sensors";
        });
        client.UseRedisClustering(Redis);   // список шлюзів
    });
    using IHost host = builder.Build();
    await host.StartAsync();
    IClusterClient cluster =
        host.Services.GetRequiredService<IClusterClient>();

    HashSet<string> errors = [];
    Stopwatch clock = Stopwatch.StartNew();
    for (int tick = 1; tick <= 40; tick++)
    {
        List<string> cells = [];
        for (int d = 1; d <= 6; d++)
        {
            ISensorGrain sensor =
                cluster.GetGrain<ISensorGrain>($"s{d}");
            try
            {
                SensorReport r = await sensor
                    .Record(20 + d + tick % 3)
                    .WaitAsync(TimeSpan.FromSeconds(3));
                cells.Add($"s{d}:{r.Silo}/{r.Count,-2}");
            }
            catch (Exception ex)
            {
                cells.Add($"s{d}:збій");
                errors.Add(ex.GetType().Name);
            }
        }
        Console.WriteLine($"{clock.Elapsed.TotalSeconds,5:F1} с " +
            string.Join(" ", cells));
        await Task.Delay(1000);
    }
    Console.WriteLine($"Помилки: {string.Join(", ", errors)}");
    await host.StopAsync();
}

public interface ISensorGrain : IGrainWithStringKey
{
    Task<SensorReport> Record(double value);
}

[GenerateSerializer, Immutable]
public sealed record SensorReport(
    [property: Id(0)] int Silo,        // номер силосу з активацією
    [property: Id(1)] int Count,       // збережених показників
    [property: Id(2)] double Average);

[GenerateSerializer]
public sealed class SensorState
{
    [Id(0)] public int Count { get; set; }
    [Id(1)] public double Sum { get; set; }
}

public sealed class SensorGrain(
    [PersistentState("sensor", "sensors")]
    IPersistentState<SensorState> state,
    ILocalSiloDetails silo) : Grain, ISensorGrain
{
    public override Task OnActivateAsync(CancellationToken token)
    {
        string key = this.GetPrimaryKeyString();
        Console.WriteLine($"  активація {key}" +
            $", показників у сховищі: {state.State.Count}");
        return Task.CompletedTask;
    }

    public async Task<SensorReport> Record(double value)
    {
        state.State.Count++;
        state.State.Sum += value;
        await state.WriteStateAsync();
        int number = silo.SiloAddress.Endpoint.Port - 11110;
        return new SensorReport(number, state.State.Count,
            state.State.Sum / state.State.Count);
    }
}
