using System.Drawing.Drawing2D;

namespace Clock;

public class MainForm : Form
{
    private readonly System.Windows.Forms.Timer timer = new();

    public MainForm()
    {
        Text = "Analog Clock";
        ClientSize = new Size(360, 360);
        BackColor = Color.White;
        DoubleBuffered = true;       // без мерехтіння
        ResizeRedraw = true;
        timer.Interval = 200;        // мілісекунди
        timer.Tick += (sender, e) => Invalidate();
        timer.Start();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        DrawClock(e.Graphics, ClientSize, DateTime.Now);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) timer.Dispose();
        base.Dispose(disposing);
    }

    public static void DrawClock(Graphics g, Size client,
        DateTime time)
    {
        float r = Math.Min(client.Width, client.Height) / 2f - 10;
        if (r < 40) return;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        // Початок координат – у центрі циферблата.
        g.TranslateTransform(client.Width / 2f, client.Height / 2f);

        using var rim = new Pen(Color.Black, 4);
        g.DrawEllipse(rim, -r, -r, 2 * r, 2 * r);
        DrawMarks(g, r);

        float hours = time.Hour % 12 + time.Minute / 60f;
        float minutes = time.Minute + time.Second / 60f;
        DrawHand(g, Color.Black, hours * 30, r * 0.5f, 8);
        DrawHand(g, Color.Black, minutes * 6, r * 0.75f, 5);
        DrawHand(g, Color.Firebrick, time.Second * 6, r * 0.85f, 2);
        g.FillEllipse(Brushes.Black, -6, -6, 12, 12);
    }

    // 60 поділок: після кожної система повертається на 6°.
    private static void DrawMarks(Graphics g, float r)
    {
        using var thin = new Pen(Color.Black, 1);
        using var thick = new Pen(Color.Black, 4);
        for (int i = 0; i < 60; i++)
        {
            bool isHour = i % 5 == 0;
            float inner = isHour ? r - 18 : r - 9;
            g.DrawLine(isHour ? thick : thin, 0, -r + 3, 0, -inner);
            g.RotateTransform(6);
        }
    }

    private static void DrawHand(Graphics g, Color color,
        float angle, float length, float width)
    {
        GraphicsState state = g.Save();
        g.RotateTransform(angle);         // 0° – на цифру 12
        using var pen = new Pen(color, width);
        pen.StartCap = LineCap.Round;
        pen.EndCap = LineCap.Round;
        g.DrawLine(pen, 0, length * 0.15f, 0, -length);
        g.Restore(state);
    }
}
