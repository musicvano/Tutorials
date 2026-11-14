using System.Drawing;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddSingleton<Board>();

var app = builder.Build();
app.MapHub<BoardHub>("/hubs/board");
app.Run("http://localhost:5113");

public record Stroke(int Width, Point[] Points);

// Усі штрихи дошки: новий учасник отримує їх під час підключення.
public class Board
{
    private readonly List<Stroke> strokes = [];
    private readonly Lock sync = new();

    public void Add(Stroke stroke)
    {
        lock (sync) { strokes.Add(stroke); }
    }

    public void Clear()
    {
        lock (sync) { strokes.Clear(); }
    }

    public Stroke[] Snapshot()
    {
        lock (sync) { return [.. strokes]; }
    }
}

public interface IBoardClient
{
    Task Load(Stroke[] strokes);
    Task StrokeDrawn(Stroke stroke);
    Task Cleared();
}

public class BoardHub(Board board) : Hub<IBoardClient>
{
    public override Task OnConnectedAsync() =>
        Clients.Caller.Load(board.Snapshot());

    public async Task Draw(Stroke stroke)
    {
        if (stroke.Points.Length < 2 || stroke.Width is < 1 or > 20)
        {
            throw new HubException("Invalid stroke");
        }
        board.Add(stroke);
        await Clients.Others.StrokeDrawn(stroke);  // крім автора
    }

    public async Task Clear()
    {
        board.Clear();
        await Clients.All.Cleared();
    }
}
