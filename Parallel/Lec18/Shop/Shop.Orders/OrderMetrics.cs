using System.Diagnostics.Metrics;

namespace Shop.Orders;

// Власні метрики сервісу замовлень (лічильник Shop.Orders).
public class OrderMetrics
{
    public const string Name = "Shop.Orders";
    readonly Counter<long> sagas;
    readonly Histogram<double> duration;
    public int OutboxPending;          // оновлює OutboxRelay

    public OrderMetrics(IMeterFactory factory)
    {
        Meter meter = factory.Create(Name);
        sagas = meter.CreateCounter<long>("shop.sagas",
            description: "Завершені саги за результатом");
        duration = meter.CreateHistogram<double>("shop.saga.duration",
            unit: "ms", description: "Тривалість саги");
        meter.CreateObservableGauge("shop.outbox.pending",
            () => OutboxPending, description: "Неопубліковані події");
    }

    public void Finished(OrderStatus status, double ms)
    {
        KeyValuePair<string, object?> tag = new("status",
            status.ToString());
        sagas.Add(1, tag);
        duration.Record(ms, tag);
    }
}
