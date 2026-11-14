namespace Bank;

// Контракт зерна: ключ – номер рахунку (рядок).
public interface IAccountGrain : IGrainWithStringKey
{
    Task<decimal> GetBalance();
    Task Deposit(decimal amount);
    Task Withdraw(decimal amount);
    Task Transfer(string toAccount, decimal amount);
    Task<AccountInfo> GetInfo();
}

// Дані, що передаються між клієнтом і силосом.
[GenerateSerializer, Immutable]
public sealed record AccountInfo(
    [property: Id(0)] string Number,
    [property: Id(1)] decimal Balance,
    [property: Id(2)] int Operations,
    [property: Id(3)] string Silo);
