using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

// Адреса з аргументу; без аргументу – усі мережні інтерфейси.
IPAddress address = args.Length > 0
    ? IPAddress.Parse(args[0]) : IPAddress.Any;
var clients = new ConcurrentDictionary<int, StreamWriter>();
var sendLock = new SemaphoreSlim(1, 1);
var tasks = new List<Task>();
int lastId = 0;

using var stop = new CancellationTokenSource();
_ = Task.Run(() => { Console.ReadLine(); stop.Cancel(); });

var listener = new TcpListener(address, 5055);
listener.Start();
Console.WriteLine($"Чат-сервер слухає {listener.LocalEndpoint}");
Console.WriteLine("Enter – зупинити сервер");
try
{
    while (true)
    {
        TcpClient client =
            await listener.AcceptTcpClientAsync(stop.Token);
        int id = Interlocked.Increment(ref lastId);
        tasks.Add(ServeAsync(id, client, stop.Token));
    }
}
catch (OperationCanceledException)
{
    listener.Stop();        // нові з’єднання більше не приймаються
}
await Task.WhenAll(tasks);  // дочекатися завершення всіх клієнтів
Console.WriteLine("Сервер зупинено");

async Task ServeAsync(int id, TcpClient client,
    CancellationToken token)
{
    using (client)
    {
        NetworkStream stream = client.GetStream();
        var reader = new StreamReader(stream, Encoding.UTF8);
        var writer = new StreamWriter(stream, new UTF8Encoding(false))
        {
            AutoFlush = true,
            NewLine = "\n"
        };
        string? name = null;
        try
        {
            // Перший рядок від клієнта – ім’я користувача.
            name = (await reader.ReadLineAsync(token))?.Trim();
            if (name is null || name.Length is 0 or > 20) return;
            clients[id] = writer;
            await BroadcastAsync($"* {name} приєднується до чату");

            string? line;
            while ((line = await reader.ReadLineAsync(token)) != null)
            {
                if (line.Length > 500) line = line[..500];
                await BroadcastAsync($"{name}: {line}");
            }
        }
        catch (OperationCanceledException)
        {
            await writer.WriteLineAsync("* Сервер зупиняється");
        }
        catch (IOException)
        {
            // З’єднання розірвано без коректного закриття.
        }
        finally
        {
            if (clients.TryRemove(id, out _))
            {
                await BroadcastAsync($"* {name} виходить із чату");
            }
        }
    }
}

async Task BroadcastAsync(string message)
{
    Console.WriteLine($"{DateTime.Now:HH:mm:ss} {message}");
    await sendLock.WaitAsync();     // один запис у мережу за раз
    try
    {
        foreach ((int id, StreamWriter writer) in clients)
        {
            try
            {
                await writer.WriteLineAsync(message);
            }
            catch (Exception ex)
                when (ex is IOException or ObjectDisposedException)
            {
                clients.TryRemove(id, out _);
            }
        }
    }
    finally
    {
        sendLock.Release();
    }
}
