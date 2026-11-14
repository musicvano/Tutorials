Console.OutputEncoding = System.Text.Encoding.UTF8;

Thread main = Thread.CurrentThread;
main.Name = "Main";
Console.WriteLine(
    $"{main.Name}: id={main.ManagedThreadId}, " +
    $"фоновий={main.IsBackground}");

int[] delays = [400, 100, 300, 200];      // «тривалість роботи», мс
string[] reports = new string[delays.Length];
Thread[] workers = new Thread[delays.Length];

for (int i = 0; i < workers.Length; i++)
{
    int index = i;                        // копія для лямбди
    workers[i] = new Thread(
        () => reports[index] = Work(delays[index]))
    {
        Name = $"Worker {i + 1}",
    };
}
Console.WriteLine($"До Start: {workers[0].ThreadState}");

foreach (Thread worker in workers) worker.Start();
Console.WriteLine($"Після Start: {workers[0].ThreadState}");

foreach (Thread worker in workers) worker.Join();  // очікування
Console.WriteLine($"Після Join: {workers[0].ThreadState}");
foreach (string report in reports) Console.WriteLine(report);

// Фоновий потік не утримує процес: нескінченний цикл
// завершиться разом із процесом.
Thread heartbeat = new(() =>
{
    while (true) Thread.Sleep(50);
})
{ Name = "Heartbeat", IsBackground = true };
heartbeat.Start();
Console.WriteLine(
    $"{heartbeat.Name}: фоновий={heartbeat.IsBackground}");
Console.WriteLine("Main завершується");

static string Work(int delayMs)
{
    Thread me = Thread.CurrentThread;
    Console.WriteLine($"  {me.Name} почав (id={me.ManagedThreadId})");
    Thread.Sleep(delayMs);                // імітація роботи
    Console.WriteLine($"  {me.Name} завершив");
    return $"{me.Name}: {delayMs} мс, пул={me.IsThreadPoolThread}";
}
