using Microsoft.AspNetCore.SignalR.Client;

Console.Write("Name: ");
string name = Console.ReadLine() ?? "Guest";

await using HubConnection connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5110/hubs/chat")
    .Build();

// Метод клієнта ReceiveMessage, який викликає сервер.
connection.On<string, string>("ReceiveMessage", (user, text) =>
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {user}: {text}"));

await connection.StartAsync();
Console.WriteLine($"Connected: {connection.ConnectionId}");
Console.WriteLine("Type messages, an empty line to exit.");

while (Console.ReadLine() is { Length: > 0 } text)
{
    // Виклик методу хабу SendMessage на сервері.
    await connection.InvokeAsync("SendMessage", name, text);
}
