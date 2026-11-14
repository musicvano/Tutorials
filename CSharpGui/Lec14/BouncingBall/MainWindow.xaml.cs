using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BouncingBall;

public partial class MainWindow : Window
{
    private readonly Ball ball = new();
    private TimeSpan lastTime = TimeSpan.Zero;

    public MainWindow()
    {
        InitializeComponent();
        Loaded += (s, e) =>
            CompositionTarget.Rendering += OnRendering;
        Closed += (s, e) =>
            CompositionTarget.Rendering -= OnRendering;
    }

    private void OnRendering(object? sender, EventArgs e)
    {
        TimeSpan now = ((RenderingEventArgs)e).RenderingTime;
        if (now == lastTime) return;         // цей кадр уже оброблено
        double dt = lastTime == TimeSpan.Zero
            ? 0 : (now - lastTime).TotalSeconds;
        lastTime = now;
        if (field.ActualWidth < 2 * ball.Radius) return;

        ball.Gravity = gravitySlider.Value;
        ball.Elasticity = elasticitySlider.Value;
        dt = Math.Min(dt, 0.05);             // після затримки вікна
        ball.Step(dt, field.ActualWidth, field.ActualHeight);
        Draw();
    }

    private void Draw()
    {
        Canvas.SetLeft(ballShape, ball.X - ball.Radius);
        Canvas.SetTop(ballShape, ball.Y - ball.Radius);
        trail.Points.Add(new Point(ball.X, ball.Y));
        if (trail.Points.Count > 150) trail.Points.RemoveAt(0);
    }

    private void Throw_Click(object sender, RoutedEventArgs e)
    {
        (ball.X, ball.Y, ball.VX, ball.VY) = (40, 40, 180, 0);
        trail.Points.Clear();
    }
}
