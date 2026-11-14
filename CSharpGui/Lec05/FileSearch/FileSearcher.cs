using System.Runtime.CompilerServices;

namespace FileSearch;

public static class FileSearcher
{
    // Повертає файли *.txt, що містять text, по одному,
    // щойно файл знайдено; percent – перебіг у відсотках.
    public static async IAsyncEnumerable<string> FindAsync(
        string folder, string text, IProgress<int>? percent,
        [EnumeratorCancellation] CancellationToken token = default)
    {
        string[] files = await Task.Run(() => Directory.GetFiles(
            folder, "*.txt", SearchOption.AllDirectories), token);
        for (int i = 0; i < files.Length; i++)
        {
            token.ThrowIfCancellationRequested();
            if (await ContainsAsync(files[i], text, token))
            {
                yield return files[i];
            }
            percent?.Report((i + 1) * 100 / files.Length);
        }
    }

    private static async Task<bool> ContainsAsync(
        string path, string text, CancellationToken token)
    {
        using var reader = new StreamReader(path);
        string? line;
        while ((line = await reader.ReadLineAsync(token)) is not null)
        {
            if (line.Contains(text,
                StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }
}
