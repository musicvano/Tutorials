// Generates the sample media files of the «Media Player» example (screenshots 14/03 and 14/04):
//
//   Player\Media\intro.mp4       04:10, 640×360 H.264 + AAC: title card with a large running time
//   Player\Media\lecture-01.mp4  00:30, the same kind of video
//   Player\Media\chime.wav       1,5 s, three-note chime (PCM 16 bit)
//
// Every frame and sample is produced here (text, shapes, sine tones), so the files are free of
// third-party rights. They are ignored by git (see ..\.gitignore); run again to recreate them:
//
//   dotnet run Make-Media.cs          (in the Animation folder)
//
// Frames are drawn with WPF and joined into MP4 by Windows.Media.Editing.MediaComposition
// (Media Foundation H.264 encoder of Windows), so no ffmpeg is needed.
#:property TargetFramework=net10.0-windows10.0.19041.0
#:property UseWPF=true
#:property Nullable=enable
#:property PublishAot=false

using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Windows.Media.Editing;
using Windows.Media.MediaProperties;
using Windows.Storage;

const int Width = 640, Height = 360;

string outDir = Path.Combine(ScriptDir(), "Player", "Media");
string tempDir = Path.Combine(Path.GetTempPath(), "Animation-media");
Directory.CreateDirectory(outDir);
if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
Directory.CreateDirectory(tempDir);

string chime = Path.Combine(outDir, "chime.wav");
WriteWav(chime, Chime());
Console.WriteLine($"{chime}: {new FileInfo(chime).Length:N0} bytes");

await MakeVideo("intro.mp4", "Introduction", 250);
await MakeVideo("lecture-01.mp4", "Lecture 1", 30);
Directory.Delete(tempDir, true);

async Task MakeVideo(string name, string title, int seconds)
{
    // 1. One PNG per second of video (WPF needs an STA thread).
    string frameDir = Path.Combine(tempDir, Path.GetFileNameWithoutExtension(name));
    Directory.CreateDirectory(frameDir);
    var thread = new Thread(() =>
    {
        for (int s = 0; s < seconds; s++)
        {
            SavePng(DrawFrame(title, s, seconds), Path.Combine(frameDir, $"{s:D4}.png"));
        }
    });
    thread.SetApartmentState(ApartmentState.STA);
    thread.Start();
    thread.Join();

    // 2. Background sound: a quiet note every second.
    string wav = Path.Combine(tempDir, Path.GetFileNameWithoutExtension(name) + ".wav");
    WriteWav(wav, Melody(seconds));

    // 3. Image clips of 1 s + audio track → MP4.
    var composition = new MediaComposition();
    for (int s = 0; s < seconds; s++)
    {
        StorageFile png = await StorageFile.GetFileFromPathAsync(Path.Combine(frameDir, $"{s:D4}.png"));
        composition.Clips.Add(await MediaClip.CreateFromImageFileAsync(png, TimeSpan.FromSeconds(1)));
    }
    StorageFile audio = await StorageFile.GetFileFromPathAsync(wav);
    composition.BackgroundAudioTracks.Add(await BackgroundAudioTrack.CreateFromFileAsync(audio));

    MediaEncodingProfile profile = MediaEncodingProfile.CreateMp4(VideoEncodingQuality.Vga);
    profile.Video.Width = Width;
    profile.Video.Height = Height;
    profile.Video.Bitrate = 400_000;
    profile.Video.FrameRate.Numerator = 25;
    profile.Video.FrameRate.Denominator = 1;

    string path = Path.Combine(outDir, name);
    File.Delete(path);
    StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(outDir);
    StorageFile target = await folder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
    var result = await composition.RenderToFileAsync(target, MediaTrimmingPreference.Precise, profile);
    Console.WriteLine($"{path}: {result}, {new FileInfo(path).Length:N0} bytes, {seconds} s");
}

// Title card: grey background, title, large running time «mm:ss», a progress line and a dot.
static DrawingVisual DrawFrame(string title, int second, int total)
{
    var visual = new DrawingVisual();
    using DrawingContext dc = visual.RenderOpen();
    dc.DrawRectangle(new LinearGradientBrush(Color.FromRgb(70, 70, 70), Color.FromRgb(20, 20, 20), 90),
        null, new Rect(0, 0, Width, Height));
    var face = new Typeface(new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.SemiBold, FontStretches.Normal);
    void Text(string text, double size, double y, Brush brush)
    {
        var ft = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
            face, size, brush, 1.0);
        dc.DrawText(ft, new Point((Width - ft.Width) / 2, y));
    }
    Text("OOP C# · Sample video", 22, 40, Brushes.Silver);
    Text(title, 40, 80, Brushes.White);
    Text($"{second / 60:D2}:{second % 60:D2}", 96, 140, Brushes.White);

    double x0 = 60, x1 = Width - 60, y = 300;
    dc.DrawLine(new Pen(Brushes.Gray, 4), new Point(x0, y), new Point(x1, y));
    double x = x0 + (x1 - x0) * second / Math.Max(1, total - 1);
    dc.DrawLine(new Pen(Brushes.White, 4), new Point(x0, y), new Point(x, y));
    dc.DrawEllipse(Brushes.White, null, new Point(x, y), 10, 10);
    return visual;
}

static void SavePng(Visual visual, string path)
{
    var bitmap = new RenderTargetBitmap(Width, Height, 96, 96, PixelFormats.Pbgra32);
    bitmap.Render(visual);
    var encoder = new PngBitmapEncoder();
    encoder.Frames.Add(BitmapFrame.Create(bitmap));
    using FileStream stream = File.Create(path);
    encoder.Save(stream);
}

// Three notes C6–E6–G6 with a decaying envelope.
static short[] Chime()
{
    const int rate = 44100;
    double[] notes = [1046.5, 1318.5, 1568.0];
    var samples = new short[(int)(rate * 1.5)];
    for (int n = 0; n < notes.Length; n++)
    {
        int start = n * rate / 5;
        for (int i = start; i < samples.Length; i++)
        {
            double t = (i - start) / (double)rate;
            double v = Math.Sin(2 * Math.PI * notes[n] * t) * Math.Exp(-3 * t) * 0.25;
            samples[i] = (short)Math.Clamp(samples[i] + v * short.MaxValue, short.MinValue, short.MaxValue);
        }
    }
    return samples;
}

// A quiet pentatonic note at the start of every second.
static short[] Melody(int seconds)
{
    const int rate = 44100;
    double[] scale = [261.6, 293.7, 329.6, 392.0, 440.0, 392.0, 329.6, 293.7];
    var samples = new short[rate * seconds];
    for (int s = 0; s < seconds; s++)
    {
        double f = scale[s % scale.Length];
        for (int i = 0; i < rate; i++)
        {
            double t = i / (double)rate;
            samples[s * rate + i] = (short)(Math.Sin(2 * Math.PI * f * t) * Math.Exp(-4 * t) * 0.12 * short.MaxValue);
        }
    }
    return samples;
}

// Mono PCM 16 bit, 44,1 kHz.
static void WriteWav(string path, short[] samples)
{
    const int rate = 44100;
    using var w = new BinaryWriter(File.Create(path));
    w.Write("RIFF"u8); w.Write(36 + samples.Length * 2); w.Write("WAVE"u8);
    w.Write("fmt "u8); w.Write(16); w.Write((short)1); w.Write((short)1);
    w.Write(rate); w.Write(rate * 2); w.Write((short)2); w.Write((short)16);
    w.Write("data"u8); w.Write(samples.Length * 2);
    foreach (short s in samples) w.Write(s);
}

static string ScriptDir([System.Runtime.CompilerServices.CallerFilePath] string path = "") =>
    Path.GetDirectoryName(path)!;
