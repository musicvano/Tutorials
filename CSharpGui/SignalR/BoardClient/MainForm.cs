using System.Drawing.Drawing2D;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace BoardClient;

public record Stroke(int Width, Point[] Points);

public partial class MainForm : Form
{
    private readonly List<Stroke> strokes = [];
    private readonly HubConnection connection;
    private List<Point>? current;     // штрих, який малюють зараз

    public MainForm()
    {
        InitializeComponent();
        widthComboBox.Items.AddRange("1", "3", "6");
        widthComboBox.SelectedIndex = 1;
        connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5113/hubs/board")
            .WithAutomaticReconnect()
            .Build();
        connection.On<Stroke[]>("Load", all => InvokeAsync(() =>
        {
            strokes.Clear();
            strokes.AddRange(all);
            canvas.Invalidate();
        }));
        connection.On<Stroke>("StrokeDrawn", s => InvokeAsync(() =>
        {
            strokes.Add(s);
            canvas.Invalidate();
        }));
        connection.On("Cleared", () => InvokeAsync(() =>
        {
            strokes.Clear();
            canvas.Invalidate();
        }));
    }

    private async void MainForm_Load(object sender, EventArgs e)
    {
        try
        {
            await connection.StartAsync();
            statusLabel.Text = "Online";
        }
        catch (HttpRequestException ex)
        {
            statusLabel.Text = $"Offline: {ex.Message}";
        }
    }

    private void canvas_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            current = [e.Location];
        }
    }

    private void canvas_MouseMove(object sender, MouseEventArgs e)
    {
        if (current is not null)
        {
            current.Add(e.Location);
            canvas.Invalidate();
        }
    }

    private async void canvas_MouseUp(object sender, MouseEventArgs e)
    {
        if (current is not { Count: > 1 })
        {
            current = null;
            return;
        }
        int width = int.Parse((string)widthComboBox.SelectedItem!);
        var stroke = new Stroke(width, [.. current]);
        current = null;
        strokes.Add(stroke);            // свій штрих – одразу
        canvas.Invalidate();
        try
        {
            await connection.InvokeAsync("Draw", stroke);
        }
        catch (Exception ex) when (ex is HubException
            or InvalidOperationException)   // хаб відхилив або офлайн
        {
            statusLabel.Text = ex.Message;
        }
    }

    private async void clearButton_Click(object sender, EventArgs e)
    {
        if (connection.State == HubConnectionState.Connected)
        {
            await connection.InvokeAsync("Clear");
        }
    }

    private void canvas_Paint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        foreach (Stroke stroke in strokes)
        {
            DrawStroke(e.Graphics, stroke.Width, stroke.Points);
        }
        if (current is { Count: > 1 })
        {
            int width =
                int.Parse((string)widthComboBox.SelectedItem!);
            DrawStroke(e.Graphics, width, [.. current]);
        }
    }

    private static void DrawStroke(Graphics g, int width,
        Point[] points)
    {
        using var pen = new Pen(Color.Black, width)
        {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round,
            LineJoin = LineJoin.Round
        };
        g.DrawLines(pen, points);
    }
}
