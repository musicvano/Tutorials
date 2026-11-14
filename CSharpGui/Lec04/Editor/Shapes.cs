using System.Drawing.Drawing2D;

namespace Editor;

// Фігура задається двома точками: початком і кінцем руху миші.
public abstract class Shape
{
    public Point Start { get; set; }
    public Point End { get; set; }

    public Rectangle Bounds => Rectangle.FromLTRB(
        Math.Min(Start.X, End.X), Math.Min(Start.Y, End.Y),
        Math.Max(Start.X, End.X), Math.Max(Start.Y, End.Y));

    // Кожен нащадок описує свій контур.
    protected abstract GraphicsPath CreatePath();

    public void Draw(Graphics g, Pen pen)
    {
        using GraphicsPath path = CreatePath();
        g.DrawPath(pen, path);
    }

    // Влучання всередину фігури або поруч із її контуром.
    public bool HitTest(Point point)
    {
        using GraphicsPath path = CreatePath();
        using var zone = new Pen(Color.Black, 10);
        return path.IsVisible(point)
            || path.IsOutlineVisible(point, zone);
    }

    public void Offset(int dx, int dy)
    {
        Start = new Point(Start.X + dx, Start.Y + dy);
        End = new Point(End.X + dx, End.Y + dy);
    }
}

public class LineShape : Shape
{
    protected override GraphicsPath CreatePath()
    {
        var path = new GraphicsPath();
        path.AddLine(Start, End);
        return path;
    }
}

public class RectangleShape : Shape
{
    protected override GraphicsPath CreatePath()
    {
        var path = new GraphicsPath();
        path.AddRectangle(Bounds);
        return path;
    }
}

public class EllipseShape : Shape
{
    protected override GraphicsPath CreatePath()
    {
        var path = new GraphicsPath();
        path.AddEllipse(Bounds);
        return path;
    }
}
