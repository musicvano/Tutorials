using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shop.Contracts;

namespace Shop.Shipping;

// Ідемпотентний споживач подій order.confirmed з Inbox.
public class OrderConfirmedConsumer(IConnection rabbit,
    IServiceScopeFactory scopes,
    ILogger<OrderConfirmedConsumer> log) : BackgroundService
{
    const string Queue = "shipping.order-confirmed";

    protected override async Task ExecuteAsync(CancellationToken stop)
    {
        IChannel ch = await rabbit.CreateChannelAsync(
            cancellationToken: stop);
        await ch.ExchangeDeclareAsync(Events.Exchange,
            ExchangeType.Topic, durable: true,
            cancellationToken: stop);
        await ch.QueueDeclareAsync(Queue, durable: true,
            exclusive: false, autoDelete: false,
            arguments: new Dictionary<string, object?>
                { ["x-queue-type"] = "quorum" },
            cancellationToken: stop);
        await ch.QueueBindAsync(Queue, Events.Exchange,
            Events.Confirmed, cancellationToken: stop);
        await ch.BasicQosAsync(0, 10, false, stop);

        AsyncEventingBasicConsumer consumer = new(ch);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                await HandleAsync(ea, stop);
                await ch.BasicAckAsync(ea.DeliveryTag, false, stop);
            }
            catch (Exception ex) when (!stop.IsCancellationRequested)
            {
                // Помилка БД тощо: повернути повідомлення в чергу.
                log.LogWarning("Помилка: {Error}", ex.Message);
                await Task.Delay(1000, stop);
                await ch.BasicNackAsync(ea.DeliveryTag, false, true,
                    stop);
            }
        };
        await ch.BasicConsumeAsync(Queue, autoAck: false, consumer,
            stop);
        await Task.Delay(Timeout.Infinite, stop);
    }

    async Task HandleAsync(BasicDeliverEventArgs ea,
        CancellationToken stop)
    {
        Guid id = Guid.Parse(ea.BasicProperties.MessageId!);
        var e = JsonSerializer.Deserialize<OrderConfirmed>(
            ea.Body.Span)!;
        await using var scope = scopes.CreateAsyncScope();
        var db = scope.ServiceProvider
            .GetRequiredService<ShippingDb>();
        if (await db.Inbox.AnyAsync(m => m.MessageId == id, stop))
        {
            log.LogWarning("Дублікат {Id} пропущено", id);
            return;
        }
        // Запис в Inbox і дія – в одній транзакції (SaveChanges).
        db.Inbox.Add(new InboxMessage { MessageId = id,
            ProcessedAt = DateTime.UtcNow });
        db.Shipments.Add(new Shipment { OrderId = e.OrderId,
            Customer = e.Customer, CreatedAt = DateTime.UtcNow });
        await db.SaveChangesAsync(stop);
        log.LogInformation("Доставку {Order} створено", e.OrderId);
    }
}
