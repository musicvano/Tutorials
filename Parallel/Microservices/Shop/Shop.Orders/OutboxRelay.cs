using System.Diagnostics;
using System.Text;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry;
using RabbitMQ.Client;
using Shop.Contracts;

namespace Shop.Orders;

// Ретранслятор вихідної скриньки: читає неопубліковані події з
// таблиці Outbox, публікує їх у RabbitMQ з підтвердженням
// брокера і лише після цього позначає як надіслані.
public class OutboxRelay(IServiceScopeFactory scopes,
    IConnection rabbit, OrderMetrics metrics, IConfiguration config,
    ILogger<OutboxRelay> log) : BackgroundService
{
    static readonly ActivitySource Source = new("Shop.Orders");
    bool failOnce = config.GetValue("Outbox:FailAfterPublish", false);

    protected override async Task ExecuteAsync(CancellationToken stop)
    {
        IChannel? ch = null;
        while (!stop.IsCancellationRequested)
        {
            try
            {
                ch ??= await OpenChannelAsync(stop);
                if (await PublishBatchAsync(ch, stop) == 0)
                    await Task.Delay(500, stop); // нових подій немає
            }
            catch (Exception ex) when (!stop.IsCancellationRequested)
            {
                // Брокер недоступний: події лишаються в таблиці.
                log.LogWarning("Outbox: {Error}", ex.Message);
                ch?.Dispose();
                ch = null;
                await Task.Delay(2000, stop);
            }
        }
    }

    async Task<IChannel> OpenChannelAsync(CancellationToken stop)
    {
        IChannel ch = await rabbit.CreateChannelAsync(
            new CreateChannelOptions(
                publisherConfirmationsEnabled: true,
                publisherConfirmationTrackingEnabled: true),
            stop);
        await ch.ExchangeDeclareAsync(Events.Exchange,
            ExchangeType.Topic, durable: true,
            cancellationToken: stop);
        return ch;
    }

    async Task<int> PublishBatchAsync(IChannel ch,
        CancellationToken stop)
    {
        await using var scope = scopes.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<OrdersDb>();
        List<OutboxMessage> batch;
        // Опитування не трасуємо: інакше 2 трасування за секунду.
        using (SuppressInstrumentationScope.Begin())
            batch = await db.Outbox.Where(m => m.SentAt == null)
                .OrderBy(m => m.CreatedAt).Take(50).ToListAsync(stop);
        metrics.OutboxPending = batch.Count;
        foreach (OutboxMessage m in batch)
        {
            // Продовжити трасування запиту, що створив подію.
            ActivityContext.TryParse(m.TraceParent, null,
                out ActivityContext parent);
            using Activity? a = Source.StartActivity("outbox relay",
                ActivityKind.Internal, parent);
            BasicProperties props = new()
            {
                MessageId = m.Id.ToString(),   // для Inbox споживача
                Type = m.Type,
                ContentType = "application/json",
                Persistent = true,
            };
            // У 7.x await завершується після підтвердження брокера.
            await ch.BasicPublishAsync(Events.Exchange, m.Type, false,
                props, Encoding.UTF8.GetBytes(m.Payload), stop);
            if (failOnce)
            {
                // Імітація збою між публікацією і позначкою SentAt.
                failOnce = false;
                throw new InvalidOperationException(
                    "збій після публікації");
            }
            m.SentAt = DateTime.UtcNow;
            await db.SaveChangesAsync(stop);
            log.LogInformation("Outbox: {Type} {Id} опубліковано",
                m.Type, m.Id);
        }
        return batch.Count;
    }
}
