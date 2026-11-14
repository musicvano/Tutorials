using Microsoft.Extensions.Options;

public partial class WeatherWorker(
    ITemperatureSensor sensor,
    IOptions<WeatherOptions> options,
    ILogger<WeatherWorker> logger) : BackgroundService
{
    private int readings;

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        WeatherOptions o = options.Value;
        LogStarted(o.City, o.IntervalSeconds);
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                double t = sensor.Read();
                readings++;
                if (t >= o.AlertTemperature)
                    LogHeat(o.City, t, o.AlertTemperature);
                else
                    LogReading(o.City, t);
            }
            catch (TimeoutException ex)
            {
                LogSensorError(o.City, ex.Message);
            }
            await Task.Delay(TimeSpan.FromSeconds(o.IntervalSeconds),
                stoppingToken);
        }
    }

    public override Task StopAsync(CancellationToken token)
    {
        LogStopped(readings);
        return base.StopAsync(token);
    }

    [LoggerMessage(1, LogLevel.Information,
        "Моніторинг {City} кожні {Seconds} с")]
    private partial void LogStarted(string city, int seconds);

    [LoggerMessage(2, LogLevel.Information,
        "{City}: {Temperature:F1} °C")]
    private partial void LogReading(string city, double temperature);

    [LoggerMessage(3, LogLevel.Warning,
        "{City}: спека {Temperature:F1} °C (поріг {Limit} °C)")]
    private partial void LogHeat(string city, double temperature,
        double limit);

    [LoggerMessage(4, LogLevel.Error,
        "{City}: помилка датчика – {Error}")]
    private partial void LogSensorError(string city, string error);

    [LoggerMessage(5, LogLevel.Information,
        "Моніторинг зупинено, вимірювань: {Count}")]
    private partial void LogStopped(int count);
}
