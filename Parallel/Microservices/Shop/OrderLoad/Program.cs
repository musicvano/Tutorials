using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

// Навантажувальний клієнт Shop: надсилає POST-запити замовлень і
// групує відповіді за результатом (статус і причина або код HTTP).
// dotnet run -c Release -- <адреса> <кількість> <задач> <пауза, мс>
string url = args.Length > 0 ? args[0]
    : "http://localhost:5100/api/orders";
int count = args.Length > 1 ? int.Parse(args[1]) : 20;
int tasks = args.Length > 2 ? int.Parse(args[2]) : 1;
int pause = args.Length > 3 ? int.Parse(args[3]) : 0;

using HttpClient http = new();
ConcurrentBag<(string Result, double Ms)> results = [];
int next = 0;
long start = Stopwatch.GetTimestamp();

await Parallel.ForAsync(0, tasks, async (_, ct) =>
{
    while (Interlocked.Increment(ref next) <= count)
    {
        long t0 = Stopwatch.GetTimestamp();
        string result;
        try
        {
            HttpResponseMessage resp = await http.PostAsJsonAsync(url,
                new
                {
                    customer = "load",
                    lines = new[] { new { productId = 3, quantity = 1 } },
                }, ct);
            result = resp.IsSuccessStatusCode
                ? Describe(await resp.Content
                    .ReadFromJsonAsync<JsonElement>(ct))
                : $"HTTP {(int)resp.StatusCode}";
        }
        catch (Exception ex)
        {
            result = ex.GetType().Name;
        }
        results.Add((result,
            Stopwatch.GetElapsedTime(t0).TotalMilliseconds));
        if (pause > 0)
            await Task.Delay(pause, ct);
    }
});
double seconds = Stopwatch.GetElapsedTime(start).TotalSeconds;

var groups = results.GroupBy(r => r.Result)
    .OrderBy(g => g.Key, StringComparer.Ordinal).ToList();
int width = Math.Max(21, groups.Max(g => g.Key.Length) + 2);
Console.WriteLine(
    $"{"Результат".PadRight(width)}{"к-сть",5}{"сер., мс",10}{"макс., мс",10}");
foreach (var g in groups)
    Console.WriteLine($"{g.Key.PadRight(width)}{g.Count(),5}" +
        $"{g.Average(r => r.Ms),10:F0}{g.Max(r => r.Ms),10:F0}");
Console.WriteLine($"Усього {results.Count} за {seconds:F1} с");

// "Confirmed" або "Cancelled оплату відхилено".
static string Describe(JsonElement order)
{
    string status = order.GetProperty("status").GetString()!;
    return order.TryGetProperty("reason", out JsonElement reason)
        && reason.ValueKind == JsonValueKind.String
        ? $"{status} {reason.GetString()}" : status;
}
