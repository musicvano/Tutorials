using System.Buffers.Binary;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

Console.OutputEncoding = Encoding.UTF8;
const int Port = 5001;

if (args is ["server"])          // лише сервер, до Ctrl+C
{
    await RunServerAsync(CancellationToken.None);
    return;
}
if (args is ["client"])          // лише клієнт
{
    await RunClientAsync();
    return;
}

// Демонстрація: сервер і клієнт в одному процесі.
using CancellationTokenSource stop = new();
Task server = RunServerAsync(stop.Token);
await RunClientAsync();
await stop.CancelAsync();
await server;

// Кадр: довжина тіла (4 байти, big-endian) + JSON.
static async Task WriteFrameAsync<T>(Stream stream, T message)
{
    byte[] body = JsonSerializer.SerializeToUtf8Bytes(
        message, JsonSerializerOptions.Web);
    byte[] frame = new byte[4 + body.Length];
    BinaryPrimitives.WriteInt32BigEndian(frame, body.Length);
    body.CopyTo(frame, 4);
    await stream.WriteAsync(frame);
}

// null – співрозмовник закрив з’єднання між кадрами.
static async Task<T?> ReadFrameAsync<T>(Stream stream,
    CancellationToken token = default)
{
    const int MaxBody = 64 * 1024;
    byte[] header = new byte[4];
    int got = await stream.ReadAtLeastAsync(header, 4,
        throwOnEndOfStream: false, token);
    if (got == 0) return default;
    if (got < 4) throw new EndOfStreamException("обрив");
    int length = BinaryPrimitives.ReadInt32BigEndian(header);
    if (length is <= 0 or > MaxBody)       // до виділення буфера
        throw new InvalidDataException($"довжина {length}");
    byte[] body = new byte[length];
    await stream.ReadExactlyAsync(body, token);
    return JsonSerializer.Deserialize<T>(body,
        JsonSerializerOptions.Web);
}

static async Task RunServerAsync(CancellationToken token)
{
    TcpListener listener = new(IPAddress.Loopback, Port);
    listener.Start();
    Console.WriteLine($"сервер слухає {listener.LocalEndpoint}");
    List<Task> sessions = [];
    try
    {
        while (true)
        {
            TcpClient client =
                await listener.AcceptTcpClientAsync(token);
            sessions.Add(ServeAsync(client, token));
        }
    }
    catch (OperationCanceledException) { }
    finally
    {
        listener.Stop();
        await Task.WhenAll(sessions);
    }
}

static async Task ServeAsync(TcpClient client,
    CancellationToken token)
{
    using (client)
    {
        NetworkStream stream = client.GetStream();
        try
        {
            while (await ReadFrameAsync<Request>(stream, token)
                is Request request)
            {
                Console.WriteLine(
                    $"сервер: запит {request.Id} ({request.Op})");
                await WriteFrameAsync(stream, Calculate(request));
            }
        }
        catch (Exception ex) when (ex is IOException
            or InvalidDataException or JsonException
            or OperationCanceledException)
        {
            Console.WriteLine($"сервер: {ex.GetType().Name}");
        }
    }
}

static Response Calculate(Request r) => r.Op switch
{
    "add" => new(r.Id, r.A + r.B, null),
    "sub" => new(r.Id, r.A - r.B, null),
    "mul" => new(r.Id, r.A * r.B, null),
    "div" when r.B == 0 => new(r.Id, 0, "ділення на нуль"),
    "div" => new(r.Id, r.A / r.B, null),
    _ => new(r.Id, 0, $"невідома операція {r.Op}"),
};

static async Task RunClientAsync()
{
    using TcpClient client = new() { NoDelay = true };
    await client.ConnectAsync(IPAddress.Loopback, Port);
    NetworkStream stream = client.GetStream();

    // Звичайні запити: один кадр – один виклик WriteAsync.
    foreach (Request request in new Request[]
        { new(1, "add", 2, 3), new(2, "div", 1, 0) })
    {
        await WriteFrameAsync(stream, request);
        Response? reply = await ReadFrameAsync<Response>(stream);
        Console.WriteLine($"клієнт: {reply}");
    }

    // Кадр 3 іде трьома шматками з паузами.
    byte[] body = JsonSerializer.SerializeToUtf8Bytes(
        new Request(3, "mul", 1.5, 8), JsonSerializerOptions.Web);
    byte[] frame = new byte[4 + body.Length];
    BinaryPrimitives.WriteInt32BigEndian(frame, body.Length);
    body.CopyTo(frame, 4);
    Console.WriteLine($"кадр 3: {Convert.ToHexString(frame, 0, 4)}"
        + $" + {Encoding.UTF8.GetString(body)}");
    int offset = 0;
    foreach (int size in new[] { 3, 10, frame.Length - 13 })
    {
        await stream.WriteAsync(frame.AsMemory(offset, size));
        offset += size;
        Console.WriteLine($"клієнт: надіслано {size} байтів");
        await Task.Delay(300);
    }
    Response? last = await ReadFrameAsync<Response>(stream);
    Console.WriteLine($"клієнт: {last}");
}                                // Dispose закриває з’єднання

record Request(int Id, string Op, double A, double B);
record Response(int Id, double Result, string? Error);
