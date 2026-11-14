using System.Collections.Concurrent;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Resize;

public record ResizeReport(int Done, string[] Errors);

public static class PhotoResizer
{
    public static async Task<ResizeReport> ResizeAllAsync(
        string[] files, string outputFolder, int maxWidth,
        int maxParallel, IProgress<int> progress,
        CancellationToken token)
    {
        Directory.CreateDirectory(outputFolder);
        var errors = new ConcurrentBag<string>();
        int done = 0;
        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = maxParallel,
            CancellationToken = token
        };
        await Parallel.ForEachAsync(files, options,
            async (file, ct) =>
            {
                string name = Path.GetFileName(file);
                try
                {
                    string target = Path.Combine(outputFolder, name);
                    await ResizeAsync(file, target, maxWidth, ct);
                }
                catch (Exception ex) when (ex is IOException
                    or ArgumentException)
                {
                    errors.Add($"{name}: {ex.Message}");
                }
                progress.Report(Interlocked.Increment(ref done));
            });
        return new ResizeReport(done - errors.Count,
            [.. errors.Order()]);
    }

    private static async Task ResizeAsync(string source,
        string target, int maxWidth, CancellationToken token)
    {
        byte[] data = await File.ReadAllBytesAsync(source, token);
        using var input = new MemoryStream(data);
        using var original = new Bitmap(input);   // CPU-робота
        double k = Math.Min(1.0, (double)maxWidth / original.Width);
        int width = (int)(original.Width * k);
        int height = (int)(original.Height * k);
        using var result = new Bitmap(width, height);
        using (Graphics g = Graphics.FromImage(result))
        {
            g.InterpolationMode =
                InterpolationMode.HighQualityBicubic;
            g.DrawImage(original, 0, 0, width, height);
        }
        using var output = new MemoryStream();
        result.Save(output, ImageFormat.Jpeg);
        await File.WriteAllBytesAsync(target, output.ToArray(),
            token);
    }
}
