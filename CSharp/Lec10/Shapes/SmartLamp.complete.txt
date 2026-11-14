class SmartLamp(string name) : ISwitchable, IDimmable
{
    private int brightness = 100;

    public bool IsOn { get; private set; }
    public void TurnOn() => IsOn = true;
    public void TurnOff() => IsOn = false;

    public int Brightness
    {
        get => brightness;
        set => brightness = Math.Clamp(value, 0, 100);
    }

    // Власний Status замінює реалізацію за замовчуванням.
    public string Status => IsOn
        ? $"{name}: увімк., яскравість {Brightness} %"
        : $"{name}: вимк.";
}

