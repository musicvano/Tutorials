using System.Diagnostics;

namespace StopwatchApp;

public partial class MainForm : Form
{
    private const string TimeFormat = @"mm\:ss\.ff";
    private readonly Stopwatch stopwatch = new();
    private TimeSpan lastLap = TimeSpan.Zero;

    public MainForm()
    {
        InitializeComponent();
        UpdateButtons();
    }

    private void startStopButton_Click(object sender, EventArgs e)
    {
        if (stopwatch.IsRunning)
        {
            stopwatch.Stop();
            uiTimer.Stop();
        }
        else
        {
            stopwatch.Start();
            uiTimer.Start();
        }
        ShowTime();
        UpdateButtons();
    }

    private void lapButton_Click(object sender, EventArgs e)
    {
        TimeSpan total = stopwatch.Elapsed;
        TimeSpan lap = total - lastLap;
        lastLap = total;
        int number = lapsListBox.Items.Count + 1;
        string lapText = lap.ToString(TimeFormat);
        string totalText = total.ToString(TimeFormat);
        lapsListBox.Items.Add(
            $"Lap {number,2}  {lapText}  {totalText}");
        UpdateButtons();
    }

    private void resetButton_Click(object sender, EventArgs e)
    {
        stopwatch.Reset();
        uiTimer.Stop();
        lastLap = TimeSpan.Zero;
        lapsListBox.Items.Clear();
        ShowTime();
        UpdateButtons();
    }

    private void saveButton_Click(object sender, EventArgs e)
    {
        if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }
        File.WriteAllLines(saveFileDialog.FileName,
            lapsListBox.Items.Cast<string>());
        MessageBox.Show($"Saved {lapsListBox.Items.Count} laps.",
            Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void uiTimer_Tick(object sender, EventArgs e) =>
        ShowTime();

    private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
        // KeyPreview = true: форма отримує клавіші раніше за кнопки.
        if (e.KeyCode == Keys.Space)
        {
            startStopButton.PerformClick();
            e.SuppressKeyPress = true;   // не передавати кнопці
        }
    }

    private void ShowTime() =>
        timeLabel.Text = stopwatch.Elapsed.ToString(TimeFormat);

    private void UpdateButtons()
    {
        startStopButton.Text =
            stopwatch.IsRunning ? "&Stop" : "&Start";
        lapButton.Enabled = stopwatch.IsRunning;
        saveButton.Enabled = lapsListBox.Items.Count > 0;
    }
}
