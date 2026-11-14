using Grpc.Core;
using Orders;

namespace Orders.Server;

public class OrderApi : OrderService.OrderServiceBase
{
    static readonly Order[] Orders =
    [
        Create(1, "ТОВ «Сигма»", OrderStatus.Paid,
            ("Папір А4", 10, 245)),
        Create(2, "Іван Коваль", OrderStatus.New,
            ("Флешка 64 ГБ", 2, 320), ("Миша", 1, 450)),
        Create(3, "ФОП Мельник", OrderStatus.Shipped,
            ("Кабель", 3, 60)),
        Create(4, "Школа № 5", OrderStatus.Paid,
            ("Проєктор", 1, 18900)),
    ];

    public override Task<Order> GetOrder(OrderRequest request,
        ServerCallContext context)
    {
        Order? order = Orders.FirstOrDefault(o => o.Id == request.Id);
        if (order is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound,
                $"замовлення {request.Id} не знайдено"));
        }
        return Task.FromResult(order);
    }

    public override async Task ListOrders(ListRequest request,
        IServerStreamWriter<Order> responseStream,
        ServerCallContext context)
    {
        string client = context.RequestHeaders
            .GetValue("client-name") ?? "невідомий";
        int sent = 0;
        try
        {
            foreach (Order order in Orders)
            {
                // Повільне сховище; токен спрацьовує після дедлайну
                // або відключення клієнта.
                await Task.Delay(400, context.CancellationToken);
                if (Total(order) >= request.MinTotal)
                {
                    await responseStream.WriteAsync(order);
                    sent++;
                }
            }
            Console.WriteLine($"  [сервер] {client}: усього {sent}");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine(
                $"  [сервер] {client}: скасовано після {sent}");
            throw;
        }
    }

    static double Total(Order order) =>
        order.Lines.Sum(line => line.Quantity * line.Price);

    static Order Create(int id, string customer, OrderStatus status,
        params (string Product, int Quantity, double Price)[] lines)
    {
        Order order = new() { Id = id, Customer = customer,
            Status = status };
        foreach (var (product, quantity, price) in lines)
        {
            order.Lines.Add(new OrderLine
            {
                Product = product, Quantity = quantity, Price = price,
            });
        }
        return order;
    }
}
