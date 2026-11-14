using Microsoft.AspNetCore.SignalR;

// Методи, які сервер викликає на клієнтах.
public interface IRoomClient
{
    Task ReceiveMessage(string user, string text);
    Task Notify(string text);
}

public class RoomHub(UserRegistry users, ILogger<RoomHub> logger)
    : Hub<IRoomClient>
{
    public override async Task OnConnectedAsync()
    {
        // Ім’я передано в рядку запиту: /hubs/rooms?user=Olena
        string? name =
            Context.GetHttpContext()?.Request.Query["user"];
        name = string.IsNullOrWhiteSpace(name)
            ? "Guest" : name.Trim();
        users.Add(Context.ConnectionId, name);
        logger.LogInformation("Connected: {User}", name);
        await Clients.Caller.Notify(
            $"Welcome, {name}! Users online: {users.Count}");
    }

    public override async Task OnDisconnectedAsync(
        Exception? exception)
    {
        ChatUser? user = users.Remove(Context.ConnectionId);
        if (user?.Room is not null)
        {
            await Clients.Group(user.Room)
                .Notify($"{user.Name} left {user.Room}");
        }
        logger.LogInformation("Disconnected: {User}", user?.Name);
    }

    public async Task JoinRoom(string room)
    {
        ChatUser user = users.Get(Context.ConnectionId);
        if (user.Room == room)
        {
            return;
        }
        if (user.Room is not null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId,
                user.Room);
            await Clients.Group(user.Room)
                .Notify($"{user.Name} left {user.Room}");
        }
        await Groups.AddToGroupAsync(Context.ConnectionId, room);
        user.Room = room;
        logger.LogInformation("{User} joined group {Room}",
            user.Name, room);
        await Clients.OthersInGroup(room)
            .Notify($"{user.Name} joined {room}");
    }

    public async Task SendToRoom(string text)
    {
        ChatUser user = users.Get(Context.ConnectionId);
        if (user.Room is null)
        {
            // Текст HubException передається клієнту.
            throw new HubException("Join a room first");
        }
        await Clients.Group(user.Room)
            .ReceiveMessage(user.Name, text);
    }
}
