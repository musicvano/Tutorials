using System.Diagnostics;

namespace Mandelbrot;

// Незмінна область площини: центр і розмір пікселя.
public record View(double X, double Y, double Scale);

public partial class MainForm : Form
{
    private const int MaxIterations = 1000;
    private View view = new(-0.5, 0, 0.004);
    private CancellationTokenSource? cts;

    public MainForm()
    {
        InitializeComponent();
        Shown += async (s, e) => await RenderAsync();
    }

    private async void pictureBox_MouseClick(object sender,
        MouseEventArgs e)
    {
        // Точка під курсором стає центром; ліва кнопка – наблизити.
        double x = view.X
            + (e.X - pictureBox.Width / 2.0) * view.Scale;
        double y = view.Y
            + (e.Y - pictureBox.Height / 2.0) * view.Scale;
        double zoom = e.Button == MouseButtons.Left ? 0.5 : 2;
        view = new View(x, y, view.Scale * zoom);
        await RenderAsync();
    }

    private async Task RenderAsync()
    {
        cts?.Cancel();                   // зупинити попередній кадр
        using var current = new CancellationTokenSource();
        cts = current;
        View frame = view;               // копія для фонового потоку
        Size size = pictureBox.ClientSize;
        var progress = new Progress<int>(p => progressBar.Value = p);
        var watch = Stopwatch.StartNew();
        try
        {
            Bitmap bitmap = await Task.Run(
                () => Draw(frame, size, progress, current.Token),
                current.Token);
            pictureBox.Image?.Dispose();
            pictureBox.Image = bitmap;
            statusLabel.Text = $"Scale {frame.Scale:E1}, "
                + $"{watch.ElapsedMilliseconds} ms";
        }
        catch (OperationCanceledException)
        {
            // Кадр застарів: уже малюється новий.
        }
        finally
        {
            if (cts == current)
            {
                cts = null;
            }
        }
    }

    private static Bitmap Draw(View v, Size size,
        IProgress<int> progress, CancellationToken token)
    {
        var bitmap = new Bitmap(size.Width, size.Height);
        try
        {
            for (int py = 0; py < size.Height; py++)
            {
                token.ThrowIfCancellationRequested();
                double y = v.Y + (py - size.Height / 2.0) * v.Scale;
                for (int px = 0; px < size.Width; px++)
                {
                    double x =
                        v.X + (px - size.Width / 2.0) * v.Scale;
                    int n = Iterations(x, y);
                    int c = n == MaxIterations ? 0 : 255 - n % 64 * 4;
                    bitmap.SetPixel(px, py, Color.FromArgb(c, c, c));
                }
                progress.Report((py + 1) * 100 / size.Height);
            }
            return bitmap;
        }
        catch
        {
            bitmap.Dispose();            // кадр не потрібен
            throw;
        }
    }

    private static int Iterations(double x, double y)
    {
        double zx = 0, zy = 0;
        int n = 0;
        while (n < MaxIterations && zx * zx + zy * zy <= 4)
        {
            (zx, zy) = (zx * zx - zy * zy + x, 2 * zx * zy + y);
            n++;
        }
        return n;
    }
}
