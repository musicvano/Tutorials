Console.OutputEncoding = System.Text.Encoding.UTF8;

// An unstable “connection”: the first two attempts fail.
int calls = 0;

bool Connect()
{
    calls++;
    return calls >= 3;
}

bool ok = Retry(Connect, attempts: 5, log: Console.WriteLine);
Console.WriteLine($"Connected: {ok}, calls: {calls}\n");

// An operation that always fails; the log is collected in a list.
List<string> journal = [];
ok = Retry(() => false, attempts: 3, log: journal.Add);
Console.WriteLine($"Saved: {ok}");
Console.WriteLine($"Log: {string.Join("; ", journal)}");

// A higher-order function: the action and the log are passed as delegates.
static bool Retry(Func<bool> action, int attempts, Action<string> log)
{
    ArgumentOutOfRangeException.ThrowIfLessThan(attempts, 1);
    for (int attempt = 1; attempt <= attempts; attempt++)
    {
        if (action())
        {
            log($"attempt {attempt}: success");
            return true;
        }
        log($"attempt {attempt}: failure");
    }
    return false;
}
