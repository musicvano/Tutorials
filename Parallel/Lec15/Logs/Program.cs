using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.OutputEncoding = Encoding.UTF8;
const string Exchange = "events";
ConnectionFactory factory = new() { HostName = "localhost" };
await using IConnection connection =
    await factory.CreateConnectionAsync("event-log");

// Ключ маршрутизації: <сервіс>.<рівень>, наприклад orders.error.
await using (IChannel setup = await connection.CreateChannelAsync())
{
    await setup.ExchangeDeclareAsync(Exchange, ExchangeType.Topic,
        durable: true);
}

// Три незалежні підписники, кожен зі своєю тимчасовою чергою.
await SubscribeAsync("чергування", "*.error", "*.critical");
await SubscribeAsync("замовлення", "orders.#");
await SubscribeAsync("архів", "#");

await using IChannel publisher =
    await connection.CreateChannelAsync();
(string Key, string Text)[] events =
[
    ("orders.info", "замовлення 17 створено"),
    ("payments.error", "банк не відповідає"),
    ("orders.error", "немає товару на складі"),
    ("auth.warning", "5 невдалих входів"),
    ("orders.db.critical", "втрачено з’єднання з БД"),
    ("payments.critical", "платіжний шлюз недоступний"),
];
foreach ((string key, string text) in events)
{
    await publisher.BasicPublishAsync(Exchange, key,
        Encoding.UTF8.GetBytes(text));
}
Console.ReadLine();                   // Enter – вихід

async Task SubscribeAsync(string name, params string[] patterns)
{
    IChannel channel = await connection.CreateChannelAsync();
    // Ім’я черги генерує брокер: amq.gen-…; exclusive – лише
    // для цього з’єднання, вилучається після його закриття.
    QueueDeclareOk queue = await channel.QueueDeclareAsync(
        queue: "", durable: false, exclusive: true, autoDelete: true);
    foreach (string pattern in patterns)
    {
        await channel.QueueBindAsync(queue.QueueName, Exchange,
            routingKey: pattern);
    }
    AsyncEventingBasicConsumer consumer = new(channel);
    consumer.ReceivedAsync += (_, ea) =>
    {
        string text = Encoding.UTF8.GetString(ea.Body.Span);
        Console.WriteLine($"{name,-11} ← [{ea.RoutingKey}] {text}");
        return Task.CompletedTask;
    };
    await channel.BasicConsumeAsync(queue.QueueName, autoAck: true,
        consumer);
}
