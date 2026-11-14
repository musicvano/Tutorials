namespace UnitConverter;

// Одиниця вимірювання: перетворення в базову одиницю та назад.
public record Unit(string Symbol, Func<double, double> ToBase,
    Func<double, double> FromBase)
{
    public static Unit Linear(string symbol, double factor) =>
        new(symbol, v => v * factor, v => v / factor);

    public override string ToString() => Symbol;   // для ComboBox
}

public record Category(string Name, double Min, double Max,
    Unit[] Units)
{
    public static readonly Category[] All =
    [
        new("Length", 0, 1000,
        [
            Unit.Linear("m", 1), Unit.Linear("km", 1000),
            Unit.Linear("cm", 0.01), Unit.Linear("mi", 1609.344),
            Unit.Linear("ft", 0.3048), Unit.Linear("in", 0.0254)
        ]),
        new("Mass", 0, 1000,
        [
            Unit.Linear("kg", 1), Unit.Linear("g", 0.001),
            Unit.Linear("t", 1000), Unit.Linear("lb", 0.45359237),
            Unit.Linear("oz", 0.028349523125)
        ]),
        new("Temperature", -100, 200,
        [
            Unit.Linear("°C", 1),
            new("°F", f => (f - 32) * 5 / 9, c => c * 9 / 5 + 32),
            new("K", k => k - 273.15, c => c + 273.15)
        ])
    ];
}
