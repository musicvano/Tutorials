namespace Bank.Core;

public static class InterestService
{
    // Єдиний публічний спосіб нарахувати відсотки.
    public static void AddMonthlyInterest(Account account,
        decimal annualRate)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(annualRate);
        account.ApplyInterest(annualRate / 12);
    }
}
