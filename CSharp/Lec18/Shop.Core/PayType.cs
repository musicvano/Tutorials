namespace Shop.Core;

public enum PayType { Hourly, Salary }

// Після рефакторингу: константи, окремі методи, одна формула.
public static class Payroll
{
    private const int NormHours = 160;
    private const decimal OvertimeFactor = 1.5m;
    private const int LoyaltyYears = 5;
    private const decimal LoyaltyBonus = 0.10m;
    private const decimal TaxRate = 0.195m;   // ПДФО 18 % + ВЗ 1,5 %

    public static decimal NetPay(
        PayType type, decimal rate, int hours, int years)
    {
        decimal gross = type switch
        {
            PayType.Hourly => HourlyPay(rate, hours),
            PayType.Salary => rate,
            _ => throw new ArgumentOutOfRangeException(nameof(type)),
        };
        return Math.Round(AfterTax(WithLoyalty(gross, years)), 2);
    }

    private static decimal HourlyPay(decimal rate, int hours)
    {
        int overtime = Math.Max(0, hours - NormHours);
        return (hours - overtime) * rate
            + overtime * rate * OvertimeFactor;
    }

    private static decimal WithLoyalty(decimal pay, int years) =>
        years >= LoyaltyYears ? pay * (1 + LoyaltyBonus) : pay;

    private static decimal AfterTax(decimal pay) =>
        pay * (1 - TaxRate);
}
