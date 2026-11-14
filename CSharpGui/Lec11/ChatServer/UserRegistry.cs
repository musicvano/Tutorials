using System.Collections.Concurrent;

public class ChatUser(string name)
{
    public string Name { get; } = name;
    public string? Room { get; set; }
}

// Singleton: живе весь час роботи сервера, на відміну від хабу.
public class UserRegistry
{
    private readonly ConcurrentDictionary<string, ChatUser> users =
        new();

    public int Count => users.Count;

    public void Add(string connectionId, string name) =>
        users[connectionId] = new ChatUser(name);

    public ChatUser Get(string connectionId) => users[connectionId];

    public ChatUser? Remove(string connectionId) =>
        users.TryRemove(connectionId, out ChatUser? user)
            ? user : null;
}
