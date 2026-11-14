namespace CompassLevel;

public static class LevelMath
{
    private static readonly string[] Directions =
        ["N", "NE", "E", "SE", "S", "SW", "W", "NW"];

    // 0° – північ, 90° – схід; сектор кожного напрямку – 45°.
    public static string ToDirection(double heading)
    {
        double normalized = (heading % 360 + 360) % 360;
        int index = (int)Math.Round(normalized / 45) % 8;
        return Directions[index];
    }

    // Нахил уздовж осей Y (pitch) і X (roll) у градусах.
    public static (double Pitch, double Roll) Tilt(
        double x, double y, double z)
    {
        double pitch = Math.Atan2(y, Math.Sqrt(x * x + z * z));
        double roll = Math.Atan2(x, Math.Sqrt(y * y + z * z));
        return (pitch * 180 / Math.PI, roll * 180 / Math.PI);
    }
}

public partial class MainPage : ContentPage
{
    private const double MaxShift = 90;   // половина ширини рамки

    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (Compass.Default.IsSupported)
        {
            Compass.Default.ReadingChanged += OnCompassChanged;
            Compass.Default.Start(SensorSpeed.UI,
                applyLowPassFilter: true);
        }
        else
        {
            headingLabel.Text = "Compass is not supported";
        }
        if (Accelerometer.Default.IsSupported)
        {
            Accelerometer.Default.ReadingChanged += OnAccelerometer;
            Accelerometer.Default.Start(SensorSpeed.UI);
        }
        else
        {
            tiltLabel.Text = "Accelerometer is not supported";
        }
    }

    // The book shows OnDisappearing only as a comment.
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (Compass.Default.IsMonitoring)
        {
            Compass.Default.Stop();
        }
        Compass.Default.ReadingChanged -= OnCompassChanged;
        if (Accelerometer.Default.IsMonitoring)
        {
            Accelerometer.Default.Stop();
        }
        Accelerometer.Default.ReadingChanged -= OnAccelerometer;
    }

    private void OnCompassChanged(object? sender,
        CompassChangedEventArgs e)
    {
        double heading = e.Reading.HeadingMagneticNorth;
        headingLabel.Text =
            $"{heading:F0}° {LevelMath.ToDirection(heading)}";
        needle.Rotation = -heading;       // стрілка на північ
    }

    private void OnAccelerometer(object? sender,
        AccelerometerChangedEventArgs e)
    {
        var a = e.Reading.Acceleration;
        var (pitch, roll) = LevelMath.Tilt(a.X, a.Y, a.Z);
        bool level = Math.Abs(pitch) < 1 && Math.Abs(roll) < 1;
        tiltLabel.Text = level
            ? "Level"
            : $"Pitch {pitch:F1}°, roll {roll:F1}°";
        // Бульбашка рухається протилежно до нахилу.
        bubble.TranslationX = Math.Clamp(-roll * 3, -MaxShift,
            MaxShift);
        bubble.TranslationY = Math.Clamp(pitch * 3, -MaxShift,
            MaxShift);
    }
}
