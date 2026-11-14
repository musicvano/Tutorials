using RabbitMQ.Client;
using StackExchange.Redis;

namespace Primes;

// З’єднання з RabbitMQ і Redis встановлюються у фоні з
// повторами: процес стартує, навіть якщо брокер ще не готовий.
public sealed class Backend(IConfiguration config,
    ILogger<Backend> log) : BackgroundService
{
    public const string Queue = "primes";
    public IConnection? Rabbit { get; private set; }
    public IConnectionMultiplexer? Redis { get; private set; }

    public bool Ready =>
        Rabbit is { IsOpen: true } && Redis is { IsConnected: true };

    protected override async Task ExecuteAsync(CancellationToken stop)
    {
        string rabbit = config.GetConnectionString("rabbitmq")
            ?? "amqp://guest:guest@localhost:5672";
        string redis = config.GetConnectionString("redis")
            ?? "localhost:6379";
        while (!Ready && !stop.IsCancellationRequested)
        {
            try
            {
                Redis ??=
                    await ConnectionMultiplexer.ConnectAsync(redis);
                Rabbit ??= await new ConnectionFactory
                    { Uri = new Uri(rabbit) }
                    .CreateConnectionAsync(stop);
                await using IChannel ch = await Rabbit
                    .CreateChannelAsync(cancellationToken: stop);
                await ch.QueueDeclareAsync(Queue, durable: true,
                    exclusive: false, autoDelete: false,
                    cancellationToken: stop);
                log.LogInformation("RabbitMQ і Redis підключено");
            }
            catch (Exception ex) when (!stop.IsCancellationRequested)
            {
                log.LogWarning(
                    "Очікування залежностей: {Error}", ex.Message);
                await Task.Delay(2000, stop);
            }
        }
    }

    public override void Dispose()
    {
        Rabbit?.Dispose();
        Redis?.Dispose();
        base.Dispose();
    }
}
