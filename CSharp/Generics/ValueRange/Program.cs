Console.OutputEncoding = System.Text.Encoding.UTF8;

Range<int> working = new(9, 18);
Range<int> meeting = new(16, 20);
Console.WriteLine($"{working} contains 12: {working.Contains(12)}");
Console.WriteLine($"Intersection with {meeting}: {working.Intersect(meeting)}");
Range<int>? none = working.Intersect(new(19, 22));
Console.WriteLine($"Intersection with [19; 22]: {none?.ToString() ?? "none"}");

Range<DateOnly> trip = new(new(2026, 7, 1), new(2026, 7, 21));
DateOnly check = new(2026, 7, 15);
Console.WriteLine($"{trip} contains {check}: {trip.Contains(check)}");

Range<decimal> budget = new(500m, 1500m);
foreach (decimal p in new[] { 499.99m, 1200m, 1500m })
{
    Console.WriteLine($"{budget} contains {p}: {budget.Contains(p)}");
}

try
{
    Range<double> wrong = new(5.5, 1.0);
}
catch (ArgumentException e)
{
    Console.WriteLine($"Error: {e.Message}");
}

class Range<T> where T : IComparable<T>
{
    public Range(T start, T end)
    {
        if (start.CompareTo(end) > 0)
        {
            throw new ArgumentException(
                $"start {start} is after end {end}");
        }
        Start = start;
        End = end;
    }

    public T Start { get; }
    public T End { get; }

    public bool Contains(T value) =>
        value.CompareTo(Start) >= 0 && value.CompareTo(End) <= 0;

    // The intersection, or null if the ranges do not overlap.
    public Range<T>? Intersect(Range<T> other)
    {
        T start = Max(Start, other.Start);
        T end = Min(End, other.End);
        return start.CompareTo(end) <= 0
            ? new Range<T>(start, end)
            : null;
    }

    public override string ToString() => $"[{Start}; {End}]";

    private static T Max(T a, T b) => a.CompareTo(b) >= 0 ? a : b;
    private static T Min(T a, T b) => a.CompareTo(b) <= 0 ? a : b;
}
