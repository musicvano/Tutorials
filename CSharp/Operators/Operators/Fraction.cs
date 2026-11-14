readonly struct Fraction : IEquatable<Fraction>
{
    public Fraction(long numerator, long denominator)
    {
        if (denominator == 0)
        {
            throw new DivideByZeroException("знаменник дорівнює 0");
        }
        long gcd = Gcd(Math.Abs(numerator), Math.Abs(denominator));
        int sign = denominator < 0 ? -1 : 1;
        Numerator = sign * numerator / gcd;
        Denominator = sign * denominator / gcd;
    }

    public long Numerator { get; }
    public long Denominator { get; }

    public static Fraction operator +(Fraction x, Fraction y) =>
        new(x.Numerator * y.Denominator + y.Numerator * x.Denominator,
            x.Denominator * y.Denominator);

    public static Fraction operator -(Fraction x) =>
        new(-x.Numerator, x.Denominator);

    public static Fraction operator -(Fraction x, Fraction y) =>
        x + -y;

    public static Fraction operator *(Fraction x, Fraction y) =>
        new(x.Numerator * y.Numerator, x.Denominator * y.Denominator);

    public static Fraction operator /(Fraction x, Fraction y) =>
        new(x.Numerator * y.Denominator, x.Denominator * y.Numerator);

    // Порівняння оголошують парами.
    public static bool operator ==(Fraction x, Fraction y) =>
        x.Equals(y);
    public static bool operator !=(Fraction x, Fraction y) =>
        !x.Equals(y);
    public static bool operator <(Fraction x, Fraction y) =>
        x.Numerator * y.Denominator < y.Numerator * x.Denominator;
    public static bool operator >(Fraction x, Fraction y) => y < x;

    // Перетворення: з цілого без втрат, у double – явно.
    public static implicit operator Fraction(long n) => new(n, 1);
    public static explicit operator double(Fraction f) =>
        (double)f.Numerator / f.Denominator;

    public bool Equals(Fraction other) =>
        Numerator == other.Numerator
        && Denominator == other.Denominator;
    public override bool Equals(object? obj) =>
        obj is Fraction other && Equals(other);
    public override int GetHashCode() =>
        HashCode.Combine(Numerator, Denominator);
    public override string ToString() => $"{Numerator}/{Denominator}";

    private static long Gcd(long x, long y) =>
        y == 0 ? Math.Max(x, 1) : Gcd(y, x % y);
}
