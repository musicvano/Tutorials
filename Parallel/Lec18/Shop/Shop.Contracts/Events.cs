namespace Shop.Contracts;

// Контракти подій, якими обмінюються сервіси через RabbitMQ.
// Спільна бібліотека містить лише контракти, без логіки.
public record OrderLine(int ProductId, int Quantity);

public record OrderConfirmed(Guid OrderId, string Customer,
    decimal Total, OrderLine[] Lines);

public record OrderCancelled(Guid OrderId, string Reason);

public static class Events
{
    public const string Exchange = "shop.events";     // topic
    public const string Confirmed = "order.confirmed"; // ключі
    public const string Cancelled = "order.cancelled";
}
