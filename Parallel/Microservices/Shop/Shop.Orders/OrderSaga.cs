using System.Diagnostics;
using System.Net;
using Shop.Contracts;
using Shop.Stock;

namespace Shop.Orders;

// Оркестратор саги оформлення замовлення:
// резерв товару (gRPC) → оплата (HTTP) → підтвердження.
// Стан саги – статус замовлення в базі, тому сагу можна
// продовжити після збою процесу (SagaRecovery).
public class OrderSaga(OrdersDb db, Inventory.InventoryClient stock,
    IHttpClientFactory http, OrderMetrics metrics,
    ILogger<OrderSaga> log)
{
    static readonly ActivitySource Source = new("Shop.Orders");

    public async Task RunAsync(Order order)
    {
        using Activity? saga = Source.StartActivity("order saga");
        saga?.SetTag("order.id", order.Id);
        long start = Stopwatch.GetTimestamp();
        if (order.Status == OrderStatus.Compensating)  // відновлення
            await CompensateAsync(order, order.Reason!, refund: true);
        try
        {
            if (order.Status == OrderStatus.Pending)
            {
                ReserveRequest req = new()
                    { OrderId = order.Id.ToString() };
                req.Lines.AddRange(order.Lines.Select(l => new Line
                {
                    ProductId = l.ProductId, Quantity = l.Quantity,
                }));
                ReserveReply r = await stock.ReserveAsync(req);
                if (r.Ok)
                    await SetStatusAsync(order, OrderStatus.Reserved);
                else
                    await CancelAsync(order, $"склад: {r.Reason}");
            }
            if (order.Status == OrderStatus.Reserved)
            {
                HttpResponseMessage pay = await http
                    .CreateClient("payments").PostAsJsonAsync(
                        "/payments", new { orderId = order.Id,
                            order.Customer, amount = order.Total });
                if (pay.StatusCode == HttpStatusCode.PaymentRequired)
                    await CompensateAsync(order, "оплату відхилено",
                        refund: false);
                else
                    await ConfirmAsync(order, pay);
            }
        }
        catch (Exception ex) when (order.Status is OrderStatus.Pending
            or OrderStatus.Reserved)
        {
            // Склад чи оплата недоступні або результат невідомий:
            // відкотити все, що могло бути зроблено.
            log.LogWarning("Замовлення {Id}: {Error}", order.Id,
                ex.Message);
            await CompensateAsync(order,
                $"сервіс недоступний: {ex.GetType().Name}",
                refund: order.Status == OrderStatus.Reserved);
        }
        metrics.Finished(order.Status,
            Stopwatch.GetElapsedTime(start).TotalMilliseconds);
    }

    // Компенсувальні дії у зворотному порядку: повернути кошти,
    // зняти резерв. Обидві операції ідемпотентні.
    async Task CompensateAsync(Order order, string reason,
        bool refund)
    {
        order.Reason = reason;
        await SetStatusAsync(order, OrderStatus.Compensating);
        try
        {
            if (refund)
                (await http.CreateClient("payments").PostAsync(
                    $"/payments/{order.Id}/refund", null))
                    .EnsureSuccessStatusCode();
            await stock.ReleaseAsync(new ReleaseRequest
                { OrderId = order.Id.ToString() });
            await CancelAsync(order, reason);
        }
        catch (Exception ex)
        {
            // Лишається Compensating: SagaRecovery повторить пізніше.
            log.LogWarning("Компенсація {Id} відкладена: {Error}",
                order.Id, ex.Message);
        }
    }

    async Task ConfirmAsync(Order order, HttpResponseMessage pay)
    {
        pay.EnsureSuccessStatusCode();  // 5xx після повторів: виняток
        // Підтвердження і подія – в одній транзакції БД.
        order.Status = OrderStatus.Confirmed;
        order.UpdatedAt = DateTime.UtcNow;
        db.Outbox.Add(OutboxMessage.From(Events.Confirmed,
            new OrderConfirmed(order.Id, order.Customer, order.Total,
                [.. order.Lines])));
        await db.SaveChangesAsync();
        log.LogInformation("Замовлення {Id} підтверджено", order.Id);
    }

    async Task CancelAsync(Order order, string reason)
    {
        order.Status = OrderStatus.Cancelled;
        order.Reason = reason;
        order.UpdatedAt = DateTime.UtcNow;
        db.Outbox.Add(OutboxMessage.From(Events.Cancelled,
            new OrderCancelled(order.Id, reason)));
        await db.SaveChangesAsync();
        log.LogInformation("Замовлення {Id} скасовано: {Reason}",
            order.Id, reason);
    }

    async Task SetStatusAsync(Order order, OrderStatus status)
    {
        order.Status = status;
        order.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }
}
