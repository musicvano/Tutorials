using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;

if (args is ["--min", var text])            // напр. --min 48
{
    ThreadPool.GetMinThreads(out _, out int io);
    ThreadPool.SetMinThreads(int.Parse(text), io);
}

// 40 робіт блокують потоки пулу на 3 с кожна.
for (int i = 0; i < 40; i++)
    ThreadPool.QueueUserWorkItem(_ => Thread.Sleep(3_000));

Stopwatch clock = Stopwatch.StartNew();
while (clock.ElapsedMilliseconds < 3_000)
{
    Console.WriteLine(
        $"{clock.ElapsedMilliseconds,5} мс: потоків " +
        $"{ThreadPool.ThreadCount,2}, у черзі " +
        $"{ThreadPool.PendingWorkItemCount,2}");
    Thread.Sleep(500);
}
