using System.Net;
using System.Net.Sockets;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
const int Port = 5000;

if (args is ["server"])          // лише сервер, до закриття вікна
{
    await RunServerAsync(CancellationToken.None);
    return;
}

// Демонстрація: сервер і два клієнти в одному процесі.
using CancellationTokenSource stop = new();
Task server = RunServerAsync(stop.Token);
await Task.WhenAll(
    RunClientAsync("Олена", 0, ["привіт", "як справи?"]),
    RunClientAsync("Богдан", 30, ["сокети"]));
await stop.CancelAsync();
await server;

static async Task RunServerAsync(CancellationToken token)
{
    TcpListener listener = new(IPAddress.Loopback, Port);
    listener.Start();                        // Bind + Listen
    Log($"сервер слухає {listener.LocalEndpoint}");
    List<Task> sessions = [];
    try
    {
        while (true)
        {
            TcpClient client =
                await listener.AcceptTcpClientAsync(token);
            sessions.Add(ServeAsync(client, token));   // не чекаємо
        }
    }
    catch (OperationCanceledException) { }
    finally
    {
        listener.Stop();
        await Task.WhenAll(sessions);
        Log($"сервер зупинено, клієнтів: {sessions.Count}");
    }
}

static async Task ServeAsync(TcpClient client,
    CancellationToken token)
{
    using (client)
    {
        EndPoint who = client.Client.RemoteEndPoint!;
        Log($"  підключено {who}");
        NetworkStream stream = client.GetStream();
        using StreamReader reader = new(stream, Encoding.UTF8);
        StreamWriter writer = new(stream, new UTF8Encoding(false));
        writer.AutoFlush = true;
        try
        {
            string? line;
            while ((line = await reader.ReadLineAsync(token)) != null)
            {
                Log($"  {who} → {line}");
                await writer.WriteLineAsync($"ECHO {line.ToUpper()}");
            }
            Log($"  {who} закрив з’єднання");
        }
        catch (Exception ex) when (ex is IOException
            or OperationCanceledException)
        {
            Log($"  {who} обірвано: {ex.GetType().Name}");
        }
    }
}

static async Task RunClientAsync(string name, int delay,
    string[] lines)
{
    await Task.Delay(delay);
    using TcpClient client = new();
    await client.ConnectAsync(IPAddress.Loopback, Port);
    NetworkStream stream = client.GetStream();
    using StreamReader reader = new(stream, Encoding.UTF8);
    StreamWriter writer = new(stream, new UTF8Encoding(false));
    writer.AutoFlush = true;
    foreach (string line in lines)
    {
        await writer.WriteLineAsync($"{name}: {line}");
        string? reply = await reader.ReadLineAsync();
        Log($"{name} отримує «{reply}»");
        await Task.Delay(100);
    }
}                                // Dispose закриває з’єднання

static void Log(string text) => Console.WriteLine(text);
