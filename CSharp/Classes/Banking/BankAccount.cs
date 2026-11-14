class BankAccount
{
    private decimal balance;      // прихований стан об’єкта
    private int operationCount;

    public BankAccount(string owner, decimal initialBalance)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(owner);
        ArgumentOutOfRangeException.ThrowIfNegative(initialBalance);
        Owner = owner;
        balance = initialBalance;
    }

    public string Owner { get; }
    public decimal Balance => balance;
    public int OperationCount => operationCount;

    public void Deposit(decimal amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        balance += amount;
        operationCount++;
    }

    public void Withdraw(decimal amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        if (amount > balance)
        {
            throw new InvalidOperationException(
                $"недостатньо коштів (доступно {balance:N2} грн)");
        }
        balance -= amount;
        operationCount++;
    }
}
