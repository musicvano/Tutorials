Console.OutputEncoding = System.Text.Encoding.UTF8;

Point[] stops =
[
    new(0, 0), new(4.5, 1), new(2, 6),
    new(5, 1.5), new(-3, 2), new(2.4, 5.2),
];

(int First, int Second, double Distance) best =
    (-1, -1, double.MaxValue);
for (int i = 0; i < stops.Length; i++)
{
    for (int j = i + 1; j < stops.Length; j++)
    {
        double d = stops[i].DistanceTo(stops[j]);
        if (d < best.Distance)
        {
            best = (i, j, d);
        }
    }
}

Console.WriteLine(
    $"Closest: {stops[best.First]} and {stops[best.Second]}");
Console.WriteLine($"Distance: {best.Distance:F3}");

// Value equality and a copy with a different coordinate.
Point a = new(2, 6);
Point b = a with { Y = 7 };
Console.WriteLine($"{a} == {stops[2]}: {a == stops[2]}");
Console.WriteLine($"{a} == {b}: {a == b}");

readonly record struct Point(double X, double Y)
{
    public double DistanceTo(Point other) =>
        Math.Sqrt((X - other.X) * (X - other.X)
            + (Y - other.Y) * (Y - other.Y));

    public override string ToString() => $"({X}; {Y})";
}
