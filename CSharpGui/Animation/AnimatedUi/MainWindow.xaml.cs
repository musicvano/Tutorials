using System.Windows;
using System.Windows.Media.Animation;

namespace AnimatedUi;

public partial class MainWindow : Window
{
    private readonly Storyboard loading;

    public MainWindow()
    {
        InitializeComponent();
        loading = (Storyboard)FindResource("LoadingStoryboard");
        loading.Completed += (s, e) => status.Text = "Done";
    }

    private void Start_Click(object sender, RoutedEventArgs e)
    {
        status.Text = "Loading...";
        loading.Begin(this, isControllable: true);
    }

    private void Pause_Click(object sender, RoutedEventArgs e)
    {
        loading.Pause(this);
        TimeSpan? time = loading.GetCurrentTime(this);
        status.Text = $"Paused at {time?.TotalSeconds:F1} s";
    }

    private void Resume_Click(object sender, RoutedEventArgs e)
    {
        loading.Resume(this);
        status.Text = "Loading...";
    }

    private void Stop_Click(object sender, RoutedEventArgs e)
    {
        loading.Stop(this);           // значення повертаються
        status.Text = "Stopped";
    }
}
