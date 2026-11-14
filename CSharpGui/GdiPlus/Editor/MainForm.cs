using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;

namespace Editor;

public enum Tool { Select, Line, Rectangle, Ellipse }

// Panel з подвійною буферизацією: властивість захищена.
public class Canvas : Panel
{
    public Canvas()
    {
        DoubleBuffered = true;
        BackColor = Color.White;
    }
}

public class MainForm : Form
{
    private readonly List<Shape> shapes = [];
    private readonly Canvas canvas = new() { Dock = DockStyle.Fill };
    private readonly ToolStrip toolBar = new();
    private readonly ToolStripStatusLabel status = new();
    private Tool tool = Tool.Line;
    private Shape? drawing;       // фігура, яку зараз малюють
    private Shape? selected;      // вибрана фігура
    private Point lastPoint;      // попереднє положення миші

    public MainForm()
    {
        Text = "Paint Editor";
        ClientSize = new Size(700, 480);
        foreach (Tool t in Enum.GetValues<Tool>())
            toolBar.Items.Add(t.ToString(), null, (s, e) => tool = t);
        toolBar.Items.Add(new ToolStripSeparator());
        toolBar.Items.Add("Save PNG...", null, (s, e) => SavePng());
        toolBar.Items.Add("Print Preview...", null,
            (s, e) => ShowPrintPreview());
        var statusBar = new StatusStrip();
        statusBar.Items.Add(status);

        Controls.Add(canvas);     // Fill додається першим
        Controls.Add(toolBar);
        Controls.Add(statusBar);
        canvas.Paint += (s, e) => DrawShapes(e.Graphics, true);
        canvas.MouseDown += Canvas_MouseDown;
        canvas.MouseMove += Canvas_MouseMove;
        canvas.MouseUp += (s, e) => drawing = null;
    }

    private void Canvas_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;
        lastPoint = e.Location;
        if (tool == Tool.Select)
        {
            // Верхня фігура – остання в списку, тому пошук з кінця.
            selected = shapes.LastOrDefault(
                s => s.HitTest(e.Location));
        }
        else
        {
            drawing = tool switch
            {
                Tool.Line => new LineShape(),
                Tool.Rectangle => new RectangleShape(),
                _ => new EllipseShape()
            };
            drawing.Start = drawing.End = e.Location;
            shapes.Add(drawing);
            selected = null;
        }
        canvas.Invalidate();
    }

    private void Canvas_MouseMove(object? sender, MouseEventArgs e)
    {
        status.Text = $"{tool}   X: {e.X}, Y: {e.Y}";
        if (e.Button != MouseButtons.Left) return;
        if (drawing != null)
        {
            drawing.End = e.Location;          // розтягування
        }
        else if (selected != null)
        {
            selected.Offset(e.X - lastPoint.X, e.Y - lastPoint.Y);
            lastPoint = e.Location;            // перетягування
        }
        canvas.Invalidate();
    }

    private void DrawShapes(Graphics g, bool showSelection)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using var pen = new Pen(Color.Black, 3);
        foreach (Shape shape in shapes)
            shape.Draw(g, pen);                // поліморфний виклик

        if (!showSelection || selected == null) return;
        Rectangle r = selected.Bounds;
        using var frame = new Pen(Color.Gray);
        frame.DashStyle = DashStyle.Dash;
        g.DrawRectangle(frame, r);
        Point[] handles = [new(r.Left, r.Top), new(r.Right, r.Top),
            new(r.Left, r.Bottom), new(r.Right, r.Bottom)];
        foreach (Point p in handles)
            g.FillRectangle(Brushes.Black, p.X - 4, p.Y - 4, 8, 8);
    }

    private void SavePng()
    {
        using var dialog = new SaveFileDialog
        {
            Filter = "PNG image (*.png)|*.png",
            FileName = "drawing.png"
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        using var bitmap = new Bitmap(canvas.Width, canvas.Height);
        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.Clear(Color.White);
            DrawShapes(g, false);              // без маркерів вибору
        }
        bitmap.Save(dialog.FileName, ImageFormat.Png);
    }

    private PrintDocument CreateDocument()
    {
        var document = new PrintDocument { DocumentName = "Drawing" };
        document.PrintPage += (sender, e) =>
        {
            Graphics g = e.Graphics!;
            Rectangle m = e.MarginBounds;      // поля сторінки
            float scale = Math.Min(1f, Math.Min(
                (float)m.Width / canvas.Width,
                (float)m.Height / canvas.Height));
            g.TranslateTransform(m.Left, m.Top);
            g.ScaleTransform(scale, scale);
            g.DrawRectangle(Pens.Black, 0, 0,
                canvas.Width, canvas.Height);
            DrawShapes(g, false);
            e.HasMorePages = false;            // одна сторінка
        };
        return document;
    }

    private void ShowPrintPreview()
    {
        using PrintDocument document = CreateDocument();
        using var preview = new PrintPreviewDialog
        {
            Document = document,
            ClientSize = new Size(800, 600)
        };
        preview.ShowDialog(this);
    }
}
