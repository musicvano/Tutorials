Console.WriteLine("Order service dependencies");

class OrderService
{
    private readonly IOrderRepository repository;
    private readonly INotifier notifier;

    public OrderService(IOrderRepository repository, INotifier notifier)
    {
        this.repository = repository;
        this.notifier = notifier;
    }
}

interface IOrderRepository { void Save(Order order); }
interface INotifier { void Send(string message); }
record Order(int Id, decimal Total);