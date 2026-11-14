using System.Security.Cryptography;

Console.OutputEncoding = System.Text.Encoding.UTF8;

int seconds = args.Length > 0 ? int.Parse(args[0]) : 10;
string folder = Path.Combine(Path.GetTempPath(), "hash-demo");
string[] files = await CreateFilesAsync(folder, count: 100);

using CancellationTokenSource esc = new();
using CancellationTokenSource timeout = new();
timeout.CancelAfter(TimeSpan.FromSeconds(seconds));
using CancellationTokenSource linked = CancellationTokenSource
    .CreateLinkedTokenSource(esc.Token, timeout.Token);
using CancellationTokenRegistration reg = linked.Token.Register(
    () => Console.WriteLine("  [Register] запит скасування"));

_ = WatchEscapeAsync(esc);    // окрема задача стежить за клавіатурою
Console.WriteLine($"Хешування {files.Length} файлів, " +
    $"Esc – скасувати, таймаут {seconds} с");

ConsoleProgress progress = new(files.Length);
int done = 0;
try
{
    done = await HashAllAsync(files, progress, linked.Token);
    Console.WriteLine($"Готово: {done} файлів");
}
catch (OperationCanceledException)
{
    string reason = timeout.IsCancellationRequested
        ? "таймаут" : "клавіша Esc";
    Console.WriteLine($"Операцію скасовано ({reason}), " +
        $"оброблено {progress.Done} з {files.Length} файлів");
}

static async Task<int> HashAllAsync(string[] files,
    IProgress<string> progress, CancellationToken token)
{
    foreach (string file in files)
    {
        token.ThrowIfCancellationRequested();
        await using FileStream stream = File.OpenRead(file);
        byte[] hash = await SHA256.HashDataAsync(stream, token);
        await Task.Delay(40, token);   // імітація повільного диска
        string hex = Convert.ToHexString(hash)[..16];
        progress.Report($"{Path.GetFileName(file)} {hex}");
    }
    return files.Length;
}

static async Task WatchEscapeAsync(CancellationTokenSource cts)
{
    if (Console.IsInputRedirected) return;   // немає клавіатури
    while (!cts.IsCancellationRequested)
    {
        if (Console.KeyAvailable
            && Console.ReadKey(true).Key == ConsoleKey.Escape)
        {
            await cts.CancelAsync();
            return;
        }
        await Task.Delay(50);
    }
}

static async Task<string[]> CreateFilesAsync(string folder, int count)
{
    Directory.CreateDirectory(folder);
    string[] names = new string[count];
    byte[] buffer = new byte[1 << 20];               // 1 МБ
    for (int i = 0; i < count; i++)
    {
        names[i] = Path.Combine(folder, $"file{i:D3}.bin");
        new Random(i).NextBytes(buffer);             // сталий вміст
        await File.WriteAllBytesAsync(names[i], buffer);
    }
    return names;
}

// Власна реалізація IProgress<T>: виводить одразу й по порядку.
class ConsoleProgress(int total) : IProgress<string>
{
    public int Done { get; private set; }

    public void Report(string value)
    {
        Done++;
        if (Done % 20 == 0 || Done == total)
        {
            Console.WriteLine($"  {Done,3}/{total}: {value}");
        }
    }
}
