using System.Diagnostics;

namespace Pages;

// The lecture describes this handler in words only (no listing).
public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
    }

    private async void loadButton_Click(object sender, EventArgs e)
    {
        string[] urls = urlsTextBox.Lines
            .Select(line => line.Trim())
            .Where(line => line.Length > 0)
            .ToArray();
        int parallel = (int)parallelNumeric.Value;
        int timeout = (int)timeoutNumeric.Value;
        loadButton.Enabled = false;
        statusLabel.Text = "Loading...";
        var watch = Stopwatch.StartNew();
        try
        {
            using var cts = new CancellationTokenSource(
                TimeSpan.FromSeconds(timeout));
            PageResult[] results = await PageLoader.LoadAllAsync(
                urls, parallel, cts.Token);
            resultsGrid.DataSource = results;
            int loaded = results.Count(r => r.Status == "OK");
            int kilobytes = results.Sum(r => r.Kilobytes);
            statusLabel.Text = $"Loaded {loaded} of {results.Length}, "
                + $"{kilobytes:N0} KB, {watch.ElapsedMilliseconds:N0} ms";
        }
        catch (OperationCanceledException)
        {
            statusLabel.Text = $"Timeout: {timeout} s";
        }
        finally
        {
            loadButton.Enabled = true;
        }
    }
}
