using System.Diagnostics;
using System.Net;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

const string Prefix = "http://localhost:5055/";
using CancellationTokenSource stop = new();
Task server = RunServerAsync(Prefix, stop.Token);

string[] pages = ["news", "weather", "slow", "missing", "rates"];
TimeSpan timeout = TimeSpan.FromSeconds(1);
using HttpClient client = new() { BaseAddress = new Uri(Prefix) };

Stopwatch total = Stopwatch.StartNew();
PageResult[] results = await Task.WhenAll(
    pages.Select(p => DownloadAsync(client, p, timeout)));
total.Stop();

Console.WriteLine($"{"Сторінка",-9} {"Результат",-22} {"Байт",6} " +
    $"{"мс",6}");
foreach (PageResult r in results)
{
    Console.WriteLine($"{r.Page,-9} {r.Status,-22} {r.Bytes,6} " +
        $"{r.Milliseconds,6:F0}");
}
int ok = results.Count(r => r.Status == "OK");
Console.WriteLine($"Успішно {ok} з {results.Length}, " +
    $"загальний час {total.ElapsedMilliseconds} мс, " +
    $"сума часів {results.Sum(r => r.Milliseconds):F0} мс");

await stop.CancelAsync();
await server;

static async Task<PageResult> DownloadAsync(
    HttpClient client, string page, TimeSpan timeout)
{
    using CancellationTokenSource cts = new(timeout);
    long start = Stopwatch.GetTimestamp();
    string status;
    int bytes = 0;
    try
    {
        using HttpResponseMessage response =
            await client.GetAsync(page, cts.Token);
        response.EnsureSuccessStatusCode();
        byte[] body =
            await response.Content.ReadAsByteArrayAsync(cts.Token);
        (status, bytes) = ("OK", body.Length);
    }
    catch (OperationCanceledException)
        when (cts.IsCancellationRequested)
    {
        status = $"таймаут {timeout.TotalSeconds} с";
    }
    catch (HttpRequestException ex)
    {
        status = $"HTTP {(int?)ex.StatusCode}";
    }
    double ms = Stopwatch.GetElapsedTime(start).TotalMilliseconds;
    return new PageResult(page, status, bytes, ms);
}

// Локальний тестовий сервер: різні затримки та відповідь 404.
static async Task RunServerAsync(
    string prefix, CancellationToken token)
{
    using HttpListener listener = new();
    listener.Prefixes.Add(prefix);
    listener.Start();
    using CancellationTokenRegistration reg =
        token.Register(listener.Stop);
    List<Task> handlers = [];
    try
    {
        while (!token.IsCancellationRequested)
        {
            HttpListenerContext context =
                await listener.GetContextAsync();
            handlers.Add(HandleAsync(context, token));
        }
    }
    catch (HttpListenerException) when (token.IsCancellationRequested)
    {
        // listener.Stop() перериває очікування запиту
    }
    await Task.WhenAll(handlers);
}

static async Task HandleAsync(
    HttpListenerContext context, CancellationToken token)
{
    string page = context.Request.Url!.AbsolutePath.Trim('/');
    (int delay, int code) = page switch
    {
        "news" => (250, 200),
        "weather" => (400, 200),
        "slow" => (3000, 200),
        "rates" => (150, 200),
        _ => (50, 404),
    };
    try
    {
        await Task.Delay(delay, token);
        string html = $"<p>{page}: {new string('x', delay)}</p>";
        byte[] body = Encoding.UTF8.GetBytes(html);
        context.Response.StatusCode = code;
        await context.Response.OutputStream.WriteAsync(body, token);
        context.Response.Close();
    }
    catch (Exception ex) when (ex is OperationCanceledException
        or HttpListenerException)
    {
        context.Response.Abort();    // сервер зупиняється
    }
}

record PageResult(string Page, string Status, int Bytes,
    double Milliseconds);
