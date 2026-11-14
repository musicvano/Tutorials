using System.ComponentModel.DataAnnotations;

Console.OutputEncoding = System.Text.Encoding.UTF8;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddOptions<WeatherOptions>()
    .Bind(builder.Configuration.GetSection(WeatherOptions.Section))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddSingleton<ITemperatureSensor, FakeSensor>();
builder.Services.AddHostedService<WeatherWorker>();

using IHost host = builder.Build();
await host.RunAsync();

public class WeatherOptions
{
    public const string Section = "Weather";

    [Required]
    public string City { get; set; } = "";

    [Range(1, 3600)]
    public int IntervalSeconds { get; set; } = 60;

    [Range(-50, 60)]
    public double AlertTemperature { get; set; } = 35;
}

public interface ITemperatureSensor
{
    double Read();
}

// Імітація датчика: кожне п’яте вимірювання – збій.
public class FakeSensor : ITemperatureSensor
{
    private readonly Random random = new(2026);
    private int count;

    public double Read()
    {
        if (++count % 5 == 0)
            throw new TimeoutException("немає відповіді");
        return 24 + random.NextDouble() * 10;
    }
}
