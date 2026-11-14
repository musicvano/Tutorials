using System.Drawing.Drawing2D;

namespace Graph;

public class MainForm : Form
{
    // Видима частина математичної площини.
    private const float XMin = -10, XMax = 10;
    private const float YMin = -10, YMax = 10;

    public MainForm()
    {
        Text = "Function Graph: y = sin(x) * x";
        ClientSize = new Size(600, 500);
        MinimumSize = new Size(300, 250);
        BackColor = Color.White;
        DoubleBuffered = true;
        ResizeRedraw = true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Rectangle area = ClientRectangle;
        area.Inflate(-30, -30);          // поля для підписів
        DrawGraph(e.Graphics, area);
    }

    private static Matrix CreateMatrix(Rectangle area)
    {
        float sx = area.Width / (XMax - XMin);
        float sy = area.Height / (YMax - YMin);
        var m = new Matrix();
        m.Translate(area.Left, area.Top);  // 3) до поля area
        m.Scale(sx, -sy);                  // 2) масштаб, Y угору
        m.Translate(-XMin, -YMax);         // 1) (XMin; YMax) у 0
        return m;
    }

    public static void DrawGraph(Graphics g, Rectangle area)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using Matrix m = CreateMatrix(area);
        DrawAxes(g, m);

        const int Segments = 400;
        var points = new PointF[Segments + 1];
        for (int i = 0; i <= Segments; i++)
        {
            float x = XMin + (XMax - XMin) * i / Segments;
            points[i] = new PointF(x, MathF.Sin(x) * x);
        }
        m.TransformPoints(points);         // у пікселі
        using var pen = new Pen(Color.Black, 2.5f);
        g.DrawLines(pen, points);
    }

    private static void DrawAxes(Graphics g, Matrix m)
    {
        using var font = new Font("Segoe UI", 8);
        using var axis = new Pen(Color.Black, 1.5f);
        axis.EndCap = LineCap.ArrowAnchor;

        for (int v = -10; v <= 10; v += 2)   // поділки й підписи
        {
            if (v == 0) continue;
            PointF[] p = [new(v, 0), new(0, v)];
            m.TransformPoints(p);
            g.DrawLine(Pens.Black, p[0].X, p[0].Y - 3,
                p[0].X, p[0].Y + 3);
            g.DrawLine(Pens.Black, p[1].X - 3, p[1].Y,
                p[1].X + 3, p[1].Y);
            DrawLabel(g, v.ToString(), font, p[0].X, p[0].Y + 12);
            DrawLabel(g, v.ToString(), font, p[1].X - 14, p[1].Y);
        }

        PointF[] ends = [new(XMin, 0), new(XMax, 0),
                         new(0, YMin), new(0, YMax)];
        m.TransformPoints(ends);
        g.DrawLine(axis, ends[0], ends[1]);
        g.DrawLine(axis, ends[2], ends[3]);
        DrawLabel(g, "x", font, ends[1].X + 10, ends[1].Y);
        DrawLabel(g, "y", font, ends[3].X, ends[3].Y - 10);
    }

    // Текст із центром у точці (x; y).
    private static void DrawLabel(Graphics g, string text,
        Font font, float x, float y)
    {
        SizeF size = g.MeasureString(text, font);
        g.DrawString(text, font, Brushes.Black,
            x - size.Width / 2, y - size.Height / 2);
    }
}
