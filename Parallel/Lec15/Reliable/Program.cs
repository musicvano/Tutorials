using System.Diagnostics;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

Console.OutputEncoding = Encoding.UTF8;
const string Main = "payments", Retry = "payments.retry",
    Parking = "payments.parking-lot";
const int MaxAttempts = 3;
Stopwatch clock = Stopwatch.StartNew();

ConnectionFactory factory = new() { HostName = "localhost" };
await using IConnection connection =
    await factory.CreateConnectionAsync("reliable-demo");
// Канал з підтвердженнями видавця: BasicPublishAsync чекає ack.
CreateChannelOptions confirms = new(
    publisherConfirmationsEnabled: true,
    publisherConfirmationTrackingEnabled: true);
await using IChannel channel =
    await connection.CreateChannelAsync(confirms);

// Відхилене з payments → payments.retry; через 2 с (TTL) → назад.
await channel.QueueDeclareAsync(Main, durable: true, exclusive: false,
    autoDelete: false, arguments: new Dictionary<string, object?>
    {
        ["x-queue-type"] = "quorum",
        ["x-dead-letter-exchange"] = "",
        ["x-dead-letter-routing-key"] = Retry,
    });
await channel.QueueDeclareAsync(Retry, durable: true,
    exclusive: false, autoDelete: false,
    arguments: new Dictionary<string, object?>
    {
        ["x-message-ttl"] = 2000,
        ["x-dead-letter-exchange"] = "",
        ["x-dead-letter-routing-key"] = Main,
    });
await channel.QueueDeclareAsync(Parking, durable: true,
    exclusive: false, autoDelete: false);

AsyncEventingBasicConsumer consumer = new(channel);
consumer.ReceivedAsync += async (_, ea) =>
{
    Payment p = JsonSerializer.Deserialize<Payment>(ea.Body.Span)!;
    long attempt = 1 + Rejections(ea.BasicProperties, Main);
    if (TryCharge(p, attempt))
    {
        Log($"{p.Id}: оплачено {p.Amount} грн (спроба {attempt})");
        await channel.BasicAckAsync(ea.DeliveryTag, false);
    }
    else if (attempt < MaxAttempts)
    {
        Log($"{p.Id}: помилка, спроба {attempt}, повтор через 2 с");
        await channel.BasicNackAsync(ea.DeliveryTag, false,
            requeue: false);                   // → dead letter
    }
    else
    {
        BasicProperties props = new()
        {
            Persistent = true,
            Headers = ea.BasicProperties.Headers,  // разом з x-death
        };
        await channel.BasicPublishAsync("", Parking, true, props,
            ea.Body);                          // чекає підтвердження
        await channel.BasicAckAsync(ea.DeliveryTag, false);
        Log($"{p.Id}: {attempt} невдалі спроби → {Parking}");
    }
};
await channel.BasicConsumeAsync(Main, autoAck: false, consumer);

Payment[] payments =
[
    new("P-1", 250, "ok"), new("P-2", 1200, "timeout"),
    new("P-3", 90, "declined"),
];
foreach (Payment p in payments)
{
    await PublishAsync(Main, p);
}
await PublishAsync("paymnts", new("P-4", 10, "ok"));  // описка
await Task.Delay(6000);                 // дочекатися повторів

async Task PublishAsync(string queue, Payment p)
{
    BasicProperties props = new()
    {
        Persistent = true, MessageId = p.Id,
        ContentType = "application/json",
    };
    try
    {
        // mandatory: true – неможливо маршрутизувати → повернення.
        await channel.BasicPublishAsync("", queue, true, props,
            JsonSerializer.SerializeToUtf8Bytes(p));
        Log($"{p.Id}: брокер підтвердив прийом");
    }
    catch (PublishException ex)
    {
        Log($"{p.Id}: не прийнято (повернення: {ex.IsReturn})");
    }
}

// Скільки разів повідомлення відхилено з черги queue (x-death).
static long Rejections(IReadOnlyBasicProperties props, string queue)
{
    if (props.Headers?.TryGetValue("x-death", out object? value)
        != true || value is not List<object> deaths)
    {
        return 0;
    }
    foreach (IDictionary<string, object?> death in
        deaths.Cast<IDictionary<string, object?>>())
    {
        string q = Text(death["queue"]);
        string reason = Text(death["reason"]);
        if (q == queue && reason == "rejected")
        {
            return (long)death["count"]!;
        }
    }
    return 0;
}

static string Text(object? bytes) =>
    Encoding.UTF8.GetString((byte[])bytes!);

// Імітація платіжного шлюзу.
static bool TryCharge(Payment p, long attempt) => p.Card switch
{
    "timeout" => attempt >= 2,          // тимчасовий збій
    "declined" => false,                // постійна відмова
    _ => true,
};

void Log(string text) =>
    Console.WriteLine($"{clock.Elapsed.TotalSeconds,5:F1} с  {text}");

record Payment(string Id, decimal Amount, string Card);
