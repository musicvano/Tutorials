Console.OutputEncoding = System.Text.Encoding.UTF8;

// Аргумент naive – лише наївний варіант, процес «зависає»
// для діагностики; без аргументу – обидва варіанти.
bool hang = args is ["naive"];
Simulate(ordered: false, hang);
if (!hang) Simulate(ordered: true, hang);

static void Simulate(bool ordered, bool hang)
{
    Account a = new(1, 10_000m), b = new(2, 10_000m);
    string title = ordered ? "Упорядковані блокування"
                           : "Наївні блокування";
    Console.WriteLine($"{title}:");

    // Перший клієнт переказує з A на B, другий – з B на A.
    Thread t1 = Start("A→B", () => Repeat(a, b, 100_000, ordered));
    Thread t2 = Start("B→A", () => Repeat(b, a, 60_000, ordered));

    bool done = t1.Join(TimeSpan.FromSeconds(3))
             && t2.Join(TimeSpan.FromSeconds(3));
    if (!done)
    {
        Console.WriteLine("  потоки не завершилися за 3 с: " +
            "взаємоблокування");
        if (hang) Thread.Sleep(Timeout.Infinite);
        return;
    }
    Console.WriteLine($"  A = {a.Balance:N2}, B = {b.Balance:N2}, " +
        $"разом {a.Balance + b.Balance:N2}");
}

static Thread Start(string name, ThreadStart body)
{
    // Фоновий потік не заважає процесу завершитися.
    Thread t = new(body) { Name = name, IsBackground = true };
    t.Start();
    return t;
}

static void Repeat(Account from, Account to, int count, bool ordered)
{
    for (int i = 0; i < count; i++)
    {
        if (ordered) TransferOrdered(from, to, 0.10m);
        else TransferNaive(from, to, 0.10m);
    }
}

// Небезпечно: порядок захоплення залежить від напрямку переказу.
static void TransferNaive(Account from, Account to, decimal sum)
{
    lock (from.Sync)
    {
        Thread.SpinWait(100);        // інший потік встигає почати
        lock (to.Sync)
        {
            from.Balance -= sum;
            to.Balance += sum;
        }
    }
}

// Безпечно: спочатку завжди блокується рахунок з меншим номером.
static void TransferOrdered(Account from, Account to, decimal sum)
{
    Account first = from.Id < to.Id ? from : to;
    Account second = from.Id < to.Id ? to : from;
    lock (first.Sync)
    {
        Thread.SpinWait(100);
        lock (second.Sync)
        {
            from.Balance -= sum;
            to.Balance += sum;
        }
    }
}

class Account(int id, decimal balance)
{
    public int Id { get; } = id;
    public decimal Balance { get; set; } = balance;
    public object Sync { get; } = new();   // об’єкт блокування
}
