using Bank.Core;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var account = new Account("  Олена Коваль ");
account.Deposit(12_000m);
InterestService.AddMonthlyInterest(account, 0.12m);

Console.WriteLine($"{account.Owner}: {account.Balance:N2} грн");

// account.ApplyInterest(0.5m);         // CS1061: метод internal
// Validator.RequirePositive(10m);      // CS0122: клас internal
