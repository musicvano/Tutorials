Console.OutputEncoding = System.Text.Encoding.UTF8;

BankAccount account = new("UA-001", creditLimit: 1_000m);
Statement statement = new();
SmsNotifier sms = new("+380 67 000 00 00");

account.BalanceChanged += statement.OnBalanceChanged;
account.BalanceChanged += sms.OnBalanceChanged;
account.Overdrawn += sms.OnOverdrawn;

account.Deposit(2_500m);
account.Withdraw(3_000m);
account.Withdraw(900m);           // exceeds the credit limit

account.BalanceChanged -= sms.OnBalanceChanged;
account.Deposit(1_000m);          // SMS is no longer sent

statement.Print();

class BalanceChangedEventArgs(decimal amount, decimal balance)
    : EventArgs
{
    public decimal Amount { get; } = amount;
    public decimal Balance { get; } = balance;
}

class BankAccount(string number, decimal creditLimit)
{
    public string Number { get; } = number;
    public decimal Balance { get; private set; }

    public event EventHandler<BalanceChangedEventArgs>?
        BalanceChanged;
    public event EventHandler? Overdrawn;

    public void Deposit(decimal amount) => Change(amount);

    public void Withdraw(decimal amount)
    {
        if (Balance - amount < -creditLimit)
        {
            Overdrawn?.Invoke(this, EventArgs.Empty);
            return;
        }
        Change(-amount);
    }

    private void Change(decimal amount)
    {
        Balance += amount;
        BalanceChanged?.Invoke(this, new(amount, Balance));
    }
}

class Statement
{
    private readonly List<string> lines = [];

    public void OnBalanceChanged(
        object? sender, BalanceChangedEventArgs e) =>
        lines.Add($"{e.Amount,10:+0.00;-0.00} {e.Balance,10:N2}");

    public void Print()
    {
        Console.WriteLine("Statement:");
        foreach (string line in lines) Console.WriteLine($"  {line}");
    }
}

class SmsNotifier(string phone)
{
    public void OnBalanceChanged(
        object? sender, BalanceChangedEventArgs e) =>
        Console.WriteLine($"SMS {phone}: balance {e.Balance:N2} UAH");

    public void OnOverdrawn(object? sender, EventArgs e)
    {
        string number = (sender as BankAccount)?.Number ?? "?";
        Console.WriteLine($"SMS {phone}: {number} – declined, limit");
    }
}
