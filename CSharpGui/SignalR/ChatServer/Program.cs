using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddSingleton<UserRegistry>();

var app = builder.Build();
app.MapHub<ChatHub>("/hubs/chat");
app.MapHub<RoomHub>("/hubs/rooms");
app.Run("http://localhost:5110");

public class ChatHub : Hub
{
    // Метод хабу, який викликають клієнти.
    public async Task SendMessage(string user, string text)
    {
        // Виклик методу ReceiveMessage на всіх клієнтах.
        await Clients.All.SendAsync("ReceiveMessage", user, text);
    }
}
