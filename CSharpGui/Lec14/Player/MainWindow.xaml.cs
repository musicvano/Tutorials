using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using Microsoft.Win32;

namespace Player;

public partial class MainWindow : Window
{
    private readonly DispatcherTimer timer =
        new() { Interval = TimeSpan.FromMilliseconds(250) };
    private bool isPlaying;
    private bool isDragging;       // користувач тягне повзунок

    public MainWindow()
    {
        InitializeComponent();
        timer.Tick += (s, e) => UpdatePosition();
    }

    private void Open_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Filter = "Media files|*.mp4;*.wmv;*.mp3;*.m4a;*.wav"
                + "|All files|*.*",
            Multiselect = true
        };
        if (dialog.ShowDialog(this) != true) return;
        foreach (string file in dialog.FileNames)
            playlist.Items.Add(new FileInfo(file));
        if (playlist.SelectedIndex < 0) playlist.SelectedIndex = 0;
    }

    private void Playlist_SelectionChanged(object sender,
        SelectionChangedEventArgs e)
    {
        if (playlist.SelectedItem is not FileInfo file) return;
        media.Source = new Uri(file.FullName);
        SetPlaying(true);
    }

    private void SetPlaying(bool play)
    {
        if (play) media.Play();
        else media.Pause();
        isPlaying = play;
        playButton.Content = play ? "Pause" : "Play";
        timer.IsEnabled = play;
    }

    private void Play_Click(object sender, RoutedEventArgs e)
    {
        if (media.Source != null) SetPlaying(!isPlaying);
    }

    private void Stop_Click(object sender, RoutedEventArgs e)
    {
        SetPlaying(false);
        media.Stop();                // позиція повертається в 0
        UpdatePosition();
    }

    private void Media_MediaOpened(object sender, RoutedEventArgs e)
    {
        positionSlider.Maximum = media.NaturalDuration.HasTimeSpan
            ? media.NaturalDuration.TimeSpan.TotalSeconds : 0;
        UpdatePosition();
    }

    private void Media_MediaEnded(object sender, RoutedEventArgs e)
    {
        if (playlist.SelectedIndex < playlist.Items.Count - 1)
            playlist.SelectedIndex++;          // наступний файл
        else
            Stop_Click(sender, e);
    }

    private void Media_MediaFailed(object? sender,
        ExceptionRoutedEventArgs e)
    {
        Stop_Click(this, e);
        MessageBox.Show(this, e.ErrorException.Message,
            "Cannot play the file", MessageBoxButton.OK,
            MessageBoxImage.Error);
    }

    private void Position_DragStarted(object sender,
        DragStartedEventArgs e) => isDragging = true;

    private void Position_DragCompleted(object sender,
        DragCompletedEventArgs e)
    {
        isDragging = false;
        media.Position = TimeSpan.FromSeconds(positionSlider.Value);
    }

    private void UpdatePosition()
    {
        if (!isDragging)
            positionSlider.Value = media.Position.TotalSeconds;
        TimeSpan total = media.NaturalDuration.HasTimeSpan
            ? media.NaturalDuration.TimeSpan : TimeSpan.Zero;
        timeText.Text = $@"{media.Position:mm\:ss} / {total:mm\:ss}";
    }
}
