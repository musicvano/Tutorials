using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.OutputEncoding = Encoding.UTF8;
ConnectionFactory factory = new() { HostName = "localhost" };
await using IConnection connection =
    await factory.CreateConnectionAsync("hello-demo");
await using IChannel channel = await connection.CreateChannelAsync();

// Оголошення ідемпотентне: черга створюється, якщо її ще немає.
await channel.QueueDeclareAsync(queue: "hello", durable: true,
    exclusive: false, autoDelete: false);

if (args is ["send", .. string[] words])
{
    string text = words.Length > 0
        ? string.Join(' ', words) : "Привіт, RabbitMQ!";
    byte[] body = Encoding.UTF8.GetBytes(text);
    // Обмінник "" (default) доставляє в чергу з ім’ям routingKey.
    await channel.BasicPublishAsync(exchange: "",
        routingKey: "hello", body: body);
    Console.WriteLine($"надіслано «{text}»");
    return;
}

AsyncEventingBasicConsumer consumer = new(channel);
consumer.ReceivedAsync += (sender, ea) =>
{
    string text = Encoding.UTF8.GetString(ea.Body.Span);
    Console.WriteLine($"отримано «{text}» (тег {ea.DeliveryTag})");
    return Task.CompletedTask;
};
await channel.BasicConsumeAsync("hello", autoAck: true, consumer);
Console.WriteLine("очікування повідомлень, Enter – вихід");
Console.ReadLine();
