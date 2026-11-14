Console.OutputEncoding = System.Text.Encoding.UTF8;

var rect = new Rectangle(4, 2.5);
Print("Initial", rect);

rect.Scale(1.5);
Print("After Scale(1.5)", rect);

rect.Width = 3;
Print("Width = 3", rect);

try
{
    rect.Height = -1;
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine($"Error: {ex.ParamName} = {ex.ActualValue}");
    Print("Unchanged", rect);
}

static void Print(string title, Rectangle r) =>
    Console.WriteLine($"{title,-17} {r.Width} × {r.Height}: "
        + $"S = {r.Area:F2}, P = {r.Perimeter:F2}");

class Rectangle
{
    private double width;
    private double height;

    public Rectangle(double width, double height)
    {
        Width = width;      // validation in the properties
        Height = height;
    }

    public double Width
    {
        get => width;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
            width = value;
        }
    }

    public double Height
    {
        get => height;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
            height = value;
        }
    }

    public double Area => width * height;
    public double Perimeter => 2 * (width + height);
    public bool IsSquare => Math.Abs(width - height) < 1e-9;

    public void Scale(double factor)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(factor);
        width *= factor;
        height *= factor;
    }
}
