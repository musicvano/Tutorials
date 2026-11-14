namespace Bank.Core;

public class Account
{
    private decimal balance;

    public Account(string owner)
    {
        Owner = Validator.RequireName(owner);
    }

    public string Owner { get; }
    public decimal Balance => balance;

    public void Deposit(decimal amount)
    {
        Validator.RequirePositive(amount);
        balance += amount;
    }

    internal void ApplyInterest(decimal rate) =>
        balance += Math.Round(balance * rate, 2);
}
