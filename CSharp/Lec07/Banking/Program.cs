Console.OutputEncoding = System.Text.Encoding.UTF8;

var account = new BankAccount("Олена Коваль", 500m);
account.Deposit(1200m);
Console.WriteLine($"{account.Owner}: {account.Balance:N2} грн");

try
{
    account.Withdraw(2000m);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Відмова: {ex.Message}");
}

account.Withdraw(700m);
Console.WriteLine($"Після зняття: {account.Balance:N2} грн");
Console.WriteLine($"Операцій: {account.OperationCount}");

