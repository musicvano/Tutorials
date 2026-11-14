class HourlyEmployee : Employee
{
    private const int NormHours = 168;

    public HourlyEmployee(string name, decimal rate, int hours)
        : base(name)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(rate);
        ArgumentOutOfRangeException.ThrowIfNegative(hours);
        Rate = rate;
        Hours = hours;
    }

    public decimal Rate { get; }
    public int Hours { get; }

    // Понаднормові години оплачуються в 1,5 раза.
    public override decimal CalculatePay()
    {
        int overtime = Math.Max(0, Hours - NormHours);
        return (Hours - overtime) * Rate + overtime * Rate * 1.5m;
    }

    public override string Describe() =>
        $"{base.Describe()} ({Hours} год)";
}
