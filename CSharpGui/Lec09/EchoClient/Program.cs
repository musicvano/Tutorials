using System.Net.Sockets;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

using var client = new TcpClient();
await client.ConnectAsync("127.0.0.1", 5050);
Console.WriteLine($"Підключено до {client.Client.RemoteEndPoint}");

NetworkStream stream = client.GetStream();
var reader = new StreamReader(stream, Encoding.UTF8);
var writer = new StreamWriter(stream, new UTF8Encoding(false))
{
    AutoFlush = true,
    NewLine = "\n"
};

while (true)
{
    Console.Write("> ");
    string? text = Console.ReadLine();
    if (string.IsNullOrEmpty(text))
    {
        break;                      // порожній рядок – вихід
    }
    await writer.WriteLineAsync(text);
    string? answer = await reader.ReadLineAsync();
    if (answer is null)
    {
        Console.WriteLine("Сервер закрив з’єднання");
        break;
    }
    Console.WriteLine(answer);
}
