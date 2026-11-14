namespace WaterTracker;

public partial class MainPage : ContentPage
{
    private const int Goal = 2000;              // мл на день
    private int total;

    public MainPage()
    {
        InitializeComponent();
        string today = DateTime.Today.ToString("yyyy-MM-dd");
        // Збережена сума діє лише в день, коли її записано.
        total = Preferences.Default.Get("date", "") == today
            ? Preferences.Default.Get("total", 0)
            : 0;
        Preferences.Default.Set("date", today);
        ShowTotal();
    }

    private void OnAdd250Clicked(object? sender, EventArgs e) =>
        Add(250);

    private void OnAdd500Clicked(object? sender, EventArgs e) =>
        Add(500);

    private void OnResetClicked(object? sender, EventArgs e) =>
        Add(-total);

    private void Add(int ml)
    {
        total += ml;
        Preferences.Default.Set("total", total);
        ShowTotal();
    }

    private void ShowTotal()
    {
        totalLabel.Text = $"{total} / {Goal} ml";
        progressBar.Progress = Math.Min(1.0, (double)total / Goal);
    }
}
