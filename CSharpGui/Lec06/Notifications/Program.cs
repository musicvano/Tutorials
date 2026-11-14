using Microsoft.Extensions.DependencyInjection;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// 1. Ручне впровадження: залежність передається в конструктор.
var manual = new OrderService(new EmailSender("shop@example.com"));
manual.PlaceOrder(1045, "olena@example.com");

// 2. Контейнер: реєстрація сервісів і автоматичне створення.
var services = new ServiceCollection();
services.AddSingleton<IMessageSender, SmsSender>();
services.AddTransient<OrderService>();

using ServiceProvider provider = services.BuildServiceProvider();
var orders = provider.GetRequiredService<OrderService>();
orders.PlaceOrder(1046, "+380671234567");

// 3. Фейкова реалізація для перевірки без реальних листів.
var fake = new FakeSender();
new OrderService(fake).PlaceOrder(1047, "test@example.com");
Console.WriteLine($"Фейк зберіг: {fake.Sent[0]}");

public interface IMessageSender
{
    void Send(string to, string text);
}

public class EmailSender(string from) : IMessageSender
{
    public void Send(string to, string text) =>
        Console.WriteLine($"E-mail {from} → {to}: {text}");
}

public class SmsSender : IMessageSender
{
    public void Send(string to, string text) =>
        Console.WriteLine($"SMS → {to}: {text}");
}

public class FakeSender : IMessageSender
{
    public List<string> Sent { get; } = [];
    public void Send(string to, string text) =>
        Sent.Add($"{to}: {text}");
}

// Сервіс залежить лише від інтерфейсу, а не від класу.
public class OrderService(IMessageSender sender)
{
    public void PlaceOrder(int id, string customer)
    {
        // ... збереження замовлення ...
        sender.Send(customer, $"Замовлення {id} прийнято");
    }
}
