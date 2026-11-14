using System.Diagnostics;

namespace Pages;

public record PageResult(string Url, int Kilobytes, long Ms,
    string Status);

public static class PageLoader
{
    private static readonly HttpClient http = new();

    public static async Task<PageResult[]> LoadAllAsync(
        IEnumerable<string> urls, int maxParallel,
        CancellationToken token)
    {
        using var gate = new SemaphoreSlim(maxParallel);
        Task<PageResult>[] tasks = urls
            .Select(url => LoadAsync(url, gate, token))
            .ToArray();                 // усі задачі запущено
        return await Task.WhenAll(tasks);
    }

    private static async Task<PageResult> LoadAsync(string url,
        SemaphoreSlim gate, CancellationToken token)
    {
        await gate.WaitAsync(token);    // не більше maxParallel
        var watch = Stopwatch.StartNew();
        try
        {
            byte[] data = await http.GetByteArrayAsync(url, token);
            return new PageResult(url, data.Length / 1024,
                watch.ElapsedMilliseconds, "OK");
        }
        catch (HttpRequestException ex)
        {
            string status = ex.StatusCode?.ToString() ?? ex.Message;
            return new PageResult(url, 0,
                watch.ElapsedMilliseconds, status);
        }
        finally
        {
            gate.Release();
        }
    }
}
