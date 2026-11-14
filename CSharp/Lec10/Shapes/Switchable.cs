interface ISwitchable
{
    bool IsOn { get; }
    void TurnOn();
    void TurnOff();

    // Член за замовчуванням: реалізація в інтерфейсі.
    string Status =>
        $"{GetType().Name}: {(IsOn ? "увімк." : "вимк.")}";
}

interface IDimmable
{
    int Brightness { get; set; }
}

