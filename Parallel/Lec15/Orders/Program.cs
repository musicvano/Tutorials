using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.OutputEncoding = Encoding.UTF8;
const string Queue = "orders";
ConnectionFactory factory = new() { HostName = "localhost" };
await using IConnection connection =
    await factory.CreateConnectionAsync($"orders-{args[0]}");
await using IChannel channel = await connection.CreateChannelAsync();

// Стійка кворумна черга: переживає перезапуск брокера.
await channel.QueueDeclareAsync(Queue, durable: true,
    exclusive: false, autoDelete: false,
    arguments: new Dictionary<string, object?>
    {
        ["x-queue-type"] = "quorum",
    });

if (args is ["publish", string countText])
{
    int count = int.Parse(countText);
    for (int id = 1; id <= count; id++)
    {
        Order order = new(id, 1 + id * 7 % 4);   // 1–4 позиції
        BasicProperties props = new()
        {
            Persistent = true,                  // записати на диск
            MessageId = $"order-{id}",
            ContentType = "application/json",
        };
        await channel.BasicPublishAsync(exchange: "",
            routingKey: Queue, mandatory: false,
            basicProperties: props,
            body: JsonSerializer.SerializeToUtf8Bytes(order));
    }
    Console.WriteLine($"опубліковано замовлень: {count}");
    return;
}

if (args is not ["work", string name, string prefetchText, ..])
{
    Console.Error.WriteLine(
        "orders publish N | orders work ім’я prefetch [аварія]");
    return;
}
ushort prefetch = ushort.Parse(prefetchText);
int crashAfter = args.Length > 3 ? int.Parse(args[3]) : int.MaxValue;

// Не більше prefetch невідтверджених повідомлень на споживача.
await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: prefetch,
    global: false);
int done = 0;
AsyncEventingBasicConsumer consumer = new(channel);
consumer.ReceivedAsync += async (_, ea) =>
{
    Order order = JsonSerializer.Deserialize<Order>(ea.Body.Span)!;
    string again = ea.Redelivered ? " (повторно)" : "";
    Console.WriteLine($"{name}: замовлення {order.Id}, " +
        $"позицій {order.Items}{again}");
    await Task.Delay(order.Items * 250);      // «обробка»
    if (++done == crashAfter)
    {
        Console.WriteLine($"{name}: аварійне завершення!");
        Environment.Exit(1);                  // без BasicAck
    }
    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
};
await channel.BasicConsumeAsync(Queue, autoAck: false, consumer);
Console.WriteLine($"{name}: prefetch = {prefetch}, очікування…");
Console.ReadLine();

record Order(int Id, int Items);
