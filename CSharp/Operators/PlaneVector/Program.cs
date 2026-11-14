Console.OutputEncoding = System.Text.Encoding.UTF8;

Vector2 wind = new(3, -1);
Vector2 engine = new(10, 4);
Vector2 drag = -0.5 * engine;

Vector2 total = wind + engine + drag;
Console.WriteLine($"Resultant: {total}, magnitude {total.Length:F2}");
Console.WriteLine($"Engine − wind: {engine - wind}");

Vector2 east = new(1, 0);
Vector2 north = new(0, 1);
Console.WriteLine($"east · north = {Vector2.Dot(east, north)}");
double angle = Vector2.AngleDegrees(east, total);
Console.WriteLine($"Angle to east: {angle:F1}°");
bool same = 2 * east == east + east;
Console.WriteLine($"2 * east == east + east: {same}");
Console.WriteLine($"total != engine: {total != engine}");

readonly struct Vector2(double x, double y) : IEquatable<Vector2>
{
    public double X { get; } = x;
    public double Y { get; } = y;
    public double Length => Math.Sqrt(Dot(this, this));

    public static Vector2 operator +(Vector2 a, Vector2 b) =>
        new(a.X + b.X, a.Y + b.Y);
    public static Vector2 operator -(Vector2 a, Vector2 b) =>
        new(a.X - b.X, a.Y - b.Y);
    public static Vector2 operator -(Vector2 v) => new(-v.X, -v.Y);

    // Multiplication by a number in both operand orders.
    public static Vector2 operator *(double k, Vector2 v) =>
        new(k * v.X, k * v.Y);
    public static Vector2 operator *(Vector2 v, double k) => k * v;

    public static double Dot(Vector2 a, Vector2 b) =>
        a.X * b.X + a.Y * b.Y;

    public static double AngleDegrees(Vector2 a, Vector2 b) =>
        Math.Acos(Dot(a, b) / (a.Length * b.Length)) * 180 / Math.PI;

    public static bool operator ==(Vector2 a, Vector2 b) =>
        a.Equals(b);
    public static bool operator !=(Vector2 a, Vector2 b) =>
        !a.Equals(b);

    public bool Equals(Vector2 other) => X == other.X && Y == other.Y;
    public override bool Equals(object? obj) =>
        obj is Vector2 other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(X, Y);
    public override string ToString() => $"({X}; {Y})";
}
