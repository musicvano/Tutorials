using System.Threading.Channels;

Console.OutputEncoding = System.Text.Encoding.UTF8;

const int LineCount = 100_000;
BoundedChannelOptions options = new(50)
{
    FullMode = BoundedChannelFullMode.Wait,   // зворотний тиск
};
Channel<string> lines = Channel.CreateBounded<string>(options);
Channel<LogRecord> records =
    Channel.CreateBounded<LogRecord>(options);

// Етап 1: читання журналу (рядки генеруються детерміновано).
Task reader = Task.Run(async () =>
{
    string[] paths = ["/", "/api/orders", "/api/users", "/login"];
    int[] codes = [200, 200, 200, 201, 404, 500];
    for (int i = 0; i < LineCount; i++)
    {
        string line = $"{i} GET {paths[i % paths.Length]} " +
                      $"{codes[i * 7 % codes.Length]} {i % 900 + 5}";
        if (i % 1000 == 999) line = "пошкоджений рядок";
        await lines.Writer.WriteAsync(line);   // чекає, якщо повно
    }
    lines.Writer.Complete();          // більше рядків не буде
});

// Етап 2: два розбирачі читають з одного каналу.
int badLines = 0;
Task[] parsers = new Task[2];
for (int p = 0; p < parsers.Length; p++)
{
    parsers[p] = Task.Run(async () =>
    {
        await foreach (string line in lines.Reader.ReadAllAsync())
        {
            string[] parts = line.Split(' ');
            if (parts.Length == 5
                && int.TryParse(parts[3], out int status)
                && int.TryParse(parts[4], out int ms))
            {
                await records.Writer.WriteAsync(
                    new LogRecord(parts[2], status, ms));
            }
            else
            {
                Interlocked.Increment(ref badLines);
            }
        }
    });
}
// Другий канал закривається, коли завершилися обидва розбирачі.
Task closeRecords = Task.Run(async () =>
{
    await Task.WhenAll(parsers);
    records.Writer.Complete();
});

// Етап 3: агрегація в одному потоці – блокування не потрібні.
SortedDictionary<int, int> byStatus = [];
long totalMs = 0;
int count = 0;
await foreach (LogRecord r in records.Reader.ReadAllAsync())
{
    byStatus[r.Status] = byStatus.GetValueOrDefault(r.Status) + 1;
    totalMs += r.Milliseconds;
    count++;
}
await Task.WhenAll(reader, closeRecords);

Console.WriteLine(
    $"Записів: {count:N0}, пошкоджених рядків: {badLines}");
foreach (var (status, n) in byStatus)
{
    double share = (double)n / count;
    Console.WriteLine($"  {status}: {n,7:N0} ({share,6:P1})");
}
double average = (double)totalMs / count;
Console.WriteLine($"Середній час відповіді: {average:F1} мс");

record LogRecord(string Path, int Status, int Milliseconds);
