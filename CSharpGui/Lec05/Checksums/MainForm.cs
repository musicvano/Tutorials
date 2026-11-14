using System.Diagnostics;
using System.Security.Cryptography;

namespace Checksums;

public partial class MainForm : Form
{
    private const int BufferSize = 1024 * 1024;       // 1 МБ
    private CancellationTokenSource? cts;

    public MainForm()
    {
        InitializeComponent();
        cancelButton.Enabled = false;
    }

    private async void computeButton_Click(object sender, EventArgs e)
    {
        if (openFileDialog.ShowDialog(this) == DialogResult.OK)
        {
            await ComputeAsync(openFileDialog.FileNames);
        }
    }

    private void cancelButton_Click(object sender, EventArgs e) =>
        cts?.Cancel();

    private async Task ComputeAsync(string[] files)
    {
        long total = files.Sum(f => new FileInfo(f).Length);
        long done = 0;
        var progress = new Progress<int>(bytes =>
        {
            done += bytes;               // виконується в потоці UI
            progressBar.Value =
                (int)(done * 100 / Math.Max(total, 1));
        });
        hashListBox.Items.Clear();
        cts = new CancellationTokenSource();
        SetBusy(true);
        var watch = Stopwatch.StartNew();
        try
        {
            foreach (string file in files)
            {
                string hash = await Sha256Async(file, progress,
                    cts.Token);
                string name = Path.GetFileName(file);
                hashListBox.Items.Add($"{hash} *{name}");
            }
            double seconds = watch.Elapsed.TotalSeconds;
            statusLabel.Text = $"Files: {files.Length}, "
                + $"{total / BufferSize:N0} MB, {seconds:F1} s";
        }
        catch (OperationCanceledException)
        {
            statusLabel.Text = $"Canceled at {progressBar.Value} %";
        }
        catch (IOException ex)
        {
            statusLabel.Text = $"Error: {ex.Message}";
        }
        finally
        {
            cts.Dispose();
            cts = null;
            SetBusy(false);
        }
    }

    private static async Task<string> Sha256Async(string path,
        IProgress<int> progress, CancellationToken token)
    {
        await using var stream = new FileStream(path, FileMode.Open,
            FileAccess.Read, FileShare.Read, BufferSize,
            useAsync: true);
        using var sha = IncrementalHash.CreateHash(
            HashAlgorithmName.SHA256);
        byte[] buffer = new byte[BufferSize];
        int read;
        while ((read = await stream.ReadAsync(buffer, token)) > 0)
        {
            sha.AppendData(buffer, 0, read);
            progress.Report(read);
        }
        return Convert.ToHexStringLower(sha.GetHashAndReset());
    }

    private void SetBusy(bool busy)
    {
        computeButton.Enabled = !busy;
        cancelButton.Enabled = busy;
        if (busy)
        {
            progressBar.Value = 0;
            statusLabel.Text = "Computing...";
        }
    }
}
