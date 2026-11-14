using System.Diagnostics;

namespace Primes;

public partial class MainForm : Form
{
    private int ticks;       // скільки разів спрацював таймер

    public MainForm()
    {
        InitializeComponent();
        clockTimer.Start();  // Interval = 100 мс
    }

    private void clockTimer_Tick(object sender, EventArgs e)
    {
        ticks++;
        clockLabel.Text = DateTime.Now.ToString("HH:mm:ss.f");
    }

    private void syncButton_Click(object sender, EventArgs e)
    {
        int limit = (int)limitNumeric.Value;
        ticks = 0;
        var watch = Stopwatch.StartNew();
        int count = PrimeMath.CountPrimes(limit);  // UI заблоковано
        ShowResult(limit, count, watch.ElapsedMilliseconds);
    }

    private async void asyncButton_Click(object sender, EventArgs e)
    {
        int limit = (int)limitNumeric.Value;
        ticks = 0;
        SetBusy(true);
        try
        {
            var watch = Stopwatch.StartNew();
            int count = await Task.Run(
                () => PrimeMath.CountPrimes(limit));
            ShowResult(limit, count, watch.ElapsedMilliseconds);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void ShowResult(int limit, int count, long ms) =>
        resultLabel.Text = $"Primes up to {limit:N0}: {count:N0}"
            + $"\n{ms:N0} ms, timer ticks: {ticks}";

    private void SetBusy(bool busy)
    {
        syncButton.Enabled = !busy;
        asyncButton.Enabled = !busy;
        UseWaitCursor = busy;
    }
}

public static class PrimeMath
{
    public static int CountPrimes(int limit)
    {
        int count = 0;
        for (int n = 2; n <= limit; n++)
        {
            if (IsPrime(n))
            {
                count++;
            }
        }
        return count;
    }

    private static bool IsPrime(int n)
    {
        if (n % 2 == 0)
        {
            return n == 2;
        }
        for (int d = 3; (long)d * d <= n; d += 2)
        {
            if (n % d == 0)
            {
                return false;
            }
        }
        return true;
    }
}
