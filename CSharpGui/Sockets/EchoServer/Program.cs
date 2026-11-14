using System.Net;
using System.Net.Sockets;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

var listener = new TcpListener(IPAddress.Loopback, 5050);
listener.Start();
Console.WriteLine($"Ехо-сервер слухає {listener.LocalEndpoint}");

while (true)
{
    // Очікування наступного клієнта (по одному за раз).
    using TcpClient client = await listener.AcceptTcpClientAsync();
    EndPoint? remote = client.Client.RemoteEndPoint;
    Console.WriteLine($"Клієнт {remote} підключився");

    NetworkStream stream = client.GetStream();
    var reader = new StreamReader(stream, Encoding.UTF8);
    var writer = new StreamWriter(stream, new UTF8Encoding(false))
    {
        AutoFlush = true,   // надсилати кожен рядок одразу
        NewLine = "\n"
    };

    string? line;
    while ((line = await reader.ReadLineAsync()) != null)
    {
        Console.WriteLine($"{remote} → {line}");
        await writer.WriteLineAsync($"Ехо: {line}");
    }
    Console.WriteLine($"Клієнт {remote} відключився");
}
