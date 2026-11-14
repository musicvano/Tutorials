using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StackExchange.Redis;

namespace Primes;

// Споживач черги primes: рахує прості числа в діапазоні
// й записує результат частини завдання в Redis.
public sealed class PrimeWorker(Backend backend,
    ILogger<PrimeWorker> log) : BackgroundService
{
    readonly SemaphoreSlim busy = new(1, 1);  // обробка частини

    protected override async Task ExecuteAsync(CancellationToken stop)
    {
        while (!backend.Ready)                // чекати з’єднань
            await Task.Delay(500, stop);
        IChannel ch = await backend.Rabbit!
            .CreateChannelAsync(cancellationToken: stop);
        await ch.BasicQosAsync(0, 1, false, stop);   // prefetch 1
        IDatabase db = backend.Redis!.GetDatabase();
        string host = Environment.MachineName;

        AsyncEventingBasicConsumer consumer = new(ch);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            await busy.WaitAsync();
            try
            {
                PrimeTask t = JsonSerializer
                    .Deserialize<PrimeTask>(ea.Body.Span)!;
                long primes = PrimeMath.Count(t.From, t.To);
                string key = $"job:{t.JobId}";
                // Ідемпотентно: повторна доставка не змінить суму.
                await db.HashSetAsync($"{key}:parts", t.From,
                    primes, When.NotExists);
                long done = await db.HashLengthAsync($"{key}:parts");
                long all = (long)await db.HashGetAsync(key, "chunks");
                long now = DateTimeOffset.UtcNow
                    .ToUnixTimeMilliseconds();
                if (done == all)
                    await db.HashSetAsync(key, "finished", now,
                        When.NotExists);
                await ch.BasicAckAsync(ea.DeliveryTag, false);
                log.LogInformation(
                    "{Host}: [{From}, {To}) -> {Primes}",
                    host, t.From, t.To, primes);
            }
            finally
            {
                busy.Release();
            }
        };
        string tag = await ch.BasicConsumeAsync(Backend.Queue,
            autoAck: false, consumer, stop);
        log.LogInformation("Робітник {Host} слухає чергу", host);

        try
        {
            await Task.Delay(Timeout.Infinite, stop);
        }
        catch (OperationCanceledException)
        {
            // SIGTERM: не брати нових повідомлень, дочекатися
            // підтвердження поточного й лише тоді закрити канал.
            await ch.BasicCancelAsync(tag);
            await busy.WaitAsync();
            await ch.CloseAsync();
            log.LogInformation("Робітник {Host} зупинився", host);
        }
    }
}
