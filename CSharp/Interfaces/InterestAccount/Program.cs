Console.OutputEncoding = System.Text.Encoding.UTF8;

Account[] accounts =
[
    new SavingsAccount("UA-S1", 50_000m, annualRate: 0.12m),
    new CreditCardAccount("UA-C7", -8_000m, annualRate: 0.36m),
    new CreditCardAccount("UA-C8", 2_500m, annualRate: 0.36m),
];

for (int month = 1; month <= 3; month++)
{
    foreach (Account a in accounts)
    {
        a.CloseMonth();
    }
}

foreach (Account a in accounts)
{
    Console.WriteLine($"{a.Number}: {a.Balance,12:N2} UAH "
        + $"({a.GetType().Name})");
}

abstract class Account(string number, decimal balance)
{
    public string Number { get; } = number;
    public decimal Balance { get; protected set; } = balance;

    // Each account type calculates monthly interest in its own way.
    protected abstract decimal MonthlyInterest();

    public void CloseMonth() =>
        Balance += Math.Round(MonthlyInterest(), 2);
}

class SavingsAccount(string number, decimal balance,
    decimal annualRate) : Account(number, balance)
{
    // Interest accrues on a positive balance.
    protected override decimal MonthlyInterest() =>
        Balance > 0 ? Balance * annualRate / 12 : 0;
}

class CreditCardAccount(string number, decimal balance,
    decimal annualRate) : Account(number, balance)
{
    // Interest is charged only on debt.
    protected override decimal MonthlyInterest() =>
        Balance < 0 ? Balance * annualRate / 12 : 0;
}
