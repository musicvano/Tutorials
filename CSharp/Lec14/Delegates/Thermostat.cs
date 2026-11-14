class TemperatureChangedEventArgs(double oldValue, double newValue)
    : EventArgs
{
    public double OldValue { get; } = oldValue;
    public double NewValue { get; } = newValue;
}

// Видавець.
class Thermostat(string room, double maxAllowed)
{
    private double current = double.NaN;

    public string Room { get; } = room;
    public double MaxAllowed { get; } = maxAllowed;

    public event EventHandler<TemperatureChangedEventArgs>?
        TemperatureChanged;

    public void Measure(double value)
    {
        Console.WriteLine($"Вимір: {value:F1} °C");
        if (value == current)
        {
            return;
        }
        TemperatureChangedEventArgs e = new(current, value);
        current = value;
        OnTemperatureChanged(e);
    }

    protected virtual void OnTemperatureChanged(
        TemperatureChangedEventArgs e) =>
        TemperatureChanged?.Invoke(this, e);
}

// Підписник.
class AlarmSystem
{
    public void OnTemperatureChanged(
        object? sender, TemperatureChangedEventArgs e)
    {
        if (sender is Thermostat t && e.NewValue > t.MaxAllowed)
        {
            Console.WriteLine(
                $"  [Тривога] {t.Room}: понад {t.MaxAllowed} °C!");
        }
    }
}
