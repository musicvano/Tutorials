using System.Diagnostics;

namespace Resize;

public partial class MainForm : Form
{
    private CancellationTokenSource? cts;

    public MainForm()
    {
        InitializeComponent();
    }

    private async void startButton_Click(object sender, EventArgs e)
    {
        if (cts is not null)            // друге натискання – Cancel
        {
            cts.Cancel();
            return;
        }
        string folder = folderTextBox.Text;
        if (!Directory.Exists(folder))
        {
            MessageBox.Show("Folder not found.", Text);
            return;
        }
        string[] files = Directory.GetFiles(folder, "*.jpg");
        progressBar.Maximum = Math.Max(files.Length, 1);
        progressBar.Value = 0;
        var progress = new Progress<int>(n => progressBar.Value = n);
        errorsListBox.Items.Clear();
        cts = new CancellationTokenSource();
        startButton.Text = "&Cancel";
        var watch = Stopwatch.StartNew();
        try
        {
            ResizeReport report = await PhotoResizer.ResizeAllAsync(
                files, Path.Combine(folder, "small"),
                (int)widthNumeric.Value, (int)parallelNumeric.Value,
                progress, cts.Token);
            errorsListBox.Items.AddRange(report.Errors);
            statusLabel.Text = $"Resized: {report.Done}, errors: "
                + $"{report.Errors.Length}, "
                + $"{watch.ElapsedMilliseconds} ms";
        }
        catch (OperationCanceledException)
        {
            statusLabel.Text = "Canceled";
        }
        finally
        {
            cts.Dispose();
            cts = null;
            startButton.Text = "&Start";
        }
    }
}
