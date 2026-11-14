using System.Text.Json;
using Primes;
using RabbitMQ.Client;
using StackExchange.Redis;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<Backend>();
builder.Services.AddHostedService(
    sp => sp.GetRequiredService<Backend>());
WebApplication app = builder.Build();
string version = app.Configuration["APP_VERSION"] ?? "1.0";

// Хто відповів: версія застосунку та ім’я контейнера (пода).
app.MapGet("/", () => new
{
    service = "primes-api", version, host = Environment.MachineName,
});

// Проби: процес живий; залежності готові приймати запити.
app.MapGet("/health/live", () => Results.Ok("live"));
app.MapGet("/health/ready", (Backend b) => b.Ready
    ? Results.Ok("ready") : Results.StatusCode(503));

// Розбити діапазон [2, to) на частини й покласти їх у чергу.
app.MapPost("/jobs", async (long to, int chunks, Backend b) =>
{
    if (!b.Ready) return Results.StatusCode(503);
    if (to < 3 || chunks < 1 || chunks > 10_000)
        return Results.BadRequest("to >= 3, 1 <= chunks <= 10000");
    string id = Guid.NewGuid().ToString("N")[..8];
    IDatabase db = b.Redis!.GetDatabase();
    await db.HashSetAsync($"job:{id}", [
        new("to", to), new("chunks", chunks), new("started", Now()),
    ]);
    await using IChannel ch = await b.Rabbit!.CreateChannelAsync();
    long step = (to - 2 + chunks - 1) / chunks;
    for (long from = 2; from < to; from += step)
    {
        PrimeTask task = new(id, from, Math.Min(from + step, to));
        await ch.BasicPublishAsync("", Backend.Queue, false,
            new BasicProperties { Persistent = true },
            JsonSerializer.SerializeToUtf8Bytes(task));
    }
    return Results.Accepted($"/jobs/{id}", new { id });
});

// Стан завдання: скільки частин готово і скільки простих.
app.MapGet("/jobs/{id}", async (string id, Backend b) =>
{
    if (!b.Ready) return Results.StatusCode(503);
    IDatabase db = b.Redis!.GetDatabase();
    HashEntry[] job = await db.HashGetAllAsync($"job:{id}");
    if (job.Length == 0) return Results.NotFound();
    Dictionary<string, long> v = job.ToDictionary(
        e => e.Name.ToString(), e => (long)e.Value);
    // Результати частин: поле – початок діапазону, значення –
    // кількість простих чисел у частині.
    HashEntry[] parts = await db.HashGetAllAsync($"job:{id}:parts");
    double? seconds = v.TryGetValue("finished", out long end)
        ? (end - v["started"]) / 1000.0 : null;
    return Results.Ok(new
    {
        id, to = v["to"], chunks = v["chunks"], done = parts.Length,
        primes = parts.Sum(p => (long)p.Value), seconds,
    });
});

// Синхронне обчислення в самому API: навантаження на процесор.
app.MapGet("/primes", (long to) => new
{
    to, primes = PrimeMath.Count(2, to),
    host = Environment.MachineName,
});

app.Run();

static long Now() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
