Console.OutputEncoding = System.Text.Encoding.UTF8;

var timer = new LapTimer("Running track, 400 m", maxLaps: 10);

// Lap times are specified in seconds so the result does not depend on the clock.
double[] times = [92.4, 88.1, 90.7, 86.9, 89.3];
foreach (double t in times)
{
    timer.AddLap(t);
}

Console.WriteLine(timer.Report());

class LapTimer
{
    private readonly double[] laps;
    private int count;

    public LapTimer(string title, int maxLaps = 20)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentOutOfRangeException.ThrowIfLessThan(maxLaps, 1);
        Title = title;
        laps = new double[maxLaps];
    }

    public string Title { get; }
    public int LapCount => count;

    public double TotalSeconds
    {
        get
        {
            double sum = 0;
            for (int i = 0; i < count; i++)
            {
                sum += laps[i];
            }
            return sum;
        }
    }

    public void AddLap(double seconds)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(seconds);
        if (count == laps.Length)
        {
            throw new InvalidOperationException("Lap limit reached.");
        }
        laps[count++] = seconds;
    }

    public int BestLap()
    {
        if (count == 0)
        {
            throw new InvalidOperationException("No laps yet.");
        }
        int best = 0;
        for (int i = 1; i < count; i++)
        {
            if (laps[i] < laps[best])
            {
                best = i;
            }
        }
        return best;
    }

    public string Report()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine(Title);
        int best = BestLap();
        double elapsed = 0;
        for (int i = 0; i < count; i++)
        {
            elapsed += laps[i];
            string mark = i == best ? " ← best" : "";
            sb.AppendLine($"  Lap {i + 1}: {Format(laps[i])}"
                + $"  (total {Format(elapsed)}){mark}");
        }
        sb.Append($"Average lap: {Format(TotalSeconds / count)}");
        return sb.ToString();
    }

    private static string Format(double seconds) =>
        TimeSpan.FromSeconds(seconds).ToString(@"m\:ss\.f");
}
