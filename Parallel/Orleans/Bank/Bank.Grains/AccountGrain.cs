using Orleans.Runtime;

namespace Bank;

// Стан, який зберігає провайдер «bank» (пам’ять або Redis).
[GenerateSerializer]
public sealed class AccountState
{
    [Id(0)] public decimal Balance { get; set; }
    [Id(1)] public int Operations { get; set; }
}

public sealed class AccountGrain(
    [PersistentState("account", "bank")]
    IPersistentState<AccountState> account) : Grain, IAccountGrain
{
    string Number => this.GetPrimaryKeyString();

    public override Task OnActivateAsync(CancellationToken token)
    {
        Console.WriteLine($"  [силос] активація {Number}, " +
            $"баланс {account.State.Balance}");
        return Task.CompletedTask;
    }

    public Task<decimal> GetBalance() =>
        Task.FromResult(account.State.Balance);

    public async Task Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount));
        account.State.Balance += amount;
        account.State.Operations++;
        await account.WriteStateAsync();
    }

    public async Task Withdraw(decimal amount)
    {
        if (amount > account.State.Balance)
            throw new InvalidOperationException(
                $"{Number}: недостатньо коштів " +
                $"({account.State.Balance} < {amount})");
        account.State.Balance -= amount;
        account.State.Operations++;
        await account.WriteStateAsync();
    }

    public async Task Transfer(string toAccount, decimal amount)
    {
        await Withdraw(amount);
        IAccountGrain target =
            GrainFactory.GetGrain<IAccountGrain>(toAccount);
        await target.Deposit(amount);   // виклик іншого зерна
    }

    public Task<AccountInfo> GetInfo() => Task.FromResult(
        new AccountInfo(Number, account.State.Balance,
            account.State.Operations, RuntimeIdentity));
}
