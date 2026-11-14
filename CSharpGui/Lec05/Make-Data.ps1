# (Re)creates the test data of the Lec05 examples (git-ignored):
#   search-data  FileSearch: folder00…folder09 with notes0000.txt…, 400 lines of random words;
#                about 13 % of the files contain «CancellationToken» (as in the lecture: 64 of 500).
#                -Scale 1 gives the 500 files of the lecture; the default 25 (12 500 files) makes the
#                search take several seconds, so the «Searching...» state can be captured.
#   hash         Checksums: video.mp4 300 MB and backup.zip 120 MB of random bytes, notes.txt 1 MB of
#                text – 421 MB in total (status «Files: 3, 421 MB, … s»).
#   photos       Resize: IMG_0001.jpg…IMG_0040.jpg 4000×3000, IMG_0041.jpg (a text file) and
#                IMG_0042.jpg (0 bytes) – «Parameter is not valid.»; the app writes photos\small.
# Usage (in the Lec05 folder): pwsh -File .\Make-Data.ps1 [-Scale 25] [-Skip hash,photos]
param(
    [int] $Scale = 25,
    [string[]] $Skip = @()
)
$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot

Add-Type -TypeDefinition @'
using System;
using System.IO;
using System.Text;

public static class Lec05Data
{
    static readonly string[] Words = {
        "async", "await", "task", "thread", "form", "button", "progress", "result",
        "file", "folder", "search", "value", "timer", "event", "handler", "window",
        "control", "method", "class", "string", "list", "stream", "reader", "line",
        "delay", "run", "pool", "queue", "message", "loop", "context", "report",
        "status", "label", "text", "box", "grid", "page", "load", "save",
        "exception", "error", "source", "token", "cancel", "request", "response", "data" };

    static string Line(Random random)
    {
        var sb = new StringBuilder();
        int count = random.Next(5, 13);
        for (int i = 0; i < count; i++)
        {
            if (i > 0) sb.Append(' ');
            sb.Append(Words[random.Next(Words.Length)]);
        }
        return sb.ToString();
    }

    public static void SearchData(string folder, int folders, int filesPerFolder,
        int lines, int matches)
    {
        var random = new Random(5);
        int total = folders * filesPerFolder;
        var hit = new bool[total];
        for (int placed = 0; placed < matches; )
        {
            int k = random.Next(total);
            if (!hit[k]) { hit[k] = true; placed++; }
        }
        for (int f = 0; f < folders; f++)
        {
            string dir = Path.Combine(folder, "folder" + f.ToString("00"));
            Directory.CreateDirectory(dir);
            for (int n = 0; n < filesPerFolder; n++)
            {
                int index = f * filesPerFolder + n;
                int hitLine = hit[index] ? random.Next(lines) : -1;
                var sb = new StringBuilder();
                for (int l = 0; l < lines; l++)
                {
                    string line = Line(random);
                    if (l == hitLine)
                    {
                        line += " CancellationToken " + Line(random);
                    }
                    sb.Append(line).Append('\n');
                }
                File.WriteAllText(Path.Combine(dir,
                    "notes" + index.ToString("0000") + ".txt"), sb.ToString());
            }
        }
    }

    public static void RandomFile(string path, long bytes)
    {
        var random = new Random(path.Length);
        byte[] buffer = new byte[1024 * 1024];
        using var stream = File.Create(path);
        for (long left = bytes; left > 0; left -= buffer.Length)
        {
            random.NextBytes(buffer);
            stream.Write(buffer, 0, (int)Math.Min(left, buffer.Length));
        }
    }

    public static void TextFile(string path, int bytes)
    {
        var random = new Random(1);
        var sb = new StringBuilder();
        while (sb.Length < bytes)
        {
            sb.Append(Line(random)).Append('\n');
        }
        sb.Length = bytes - 1;
        sb.Append('\n');
        File.WriteAllText(path, sb.ToString());
    }
}
'@

function Reset-Folder([string] $name) {
    $path = Join-Path $root $name
    if (Test-Path $path) { Remove-Item $path -Recurse -Force }
    New-Item -ItemType Directory $path | Out-Null
    $path
}

$MB = 1024 * 1024

if ('search-data' -notin $Skip) {
    $dir = Reset-Folder 'search-data'
    [Lec05Data]::SearchData($dir, 10, 50 * $Scale, 400, 64 * $Scale)
    Write-Host "search-data: $(500 * $Scale) files, $(64 * $Scale) with CancellationToken"
}

if ('hash' -notin $Skip) {
    $dir = Reset-Folder 'hash'
    [Lec05Data]::RandomFile((Join-Path $dir 'video.mp4'), 300 * $MB)
    [Lec05Data]::RandomFile((Join-Path $dir 'backup.zip'), 120 * $MB)
    [Lec05Data]::TextFile((Join-Path $dir 'notes.txt'), $MB)
    Write-Host 'hash: video.mp4 300 MB, backup.zip 120 MB, notes.txt 1 MB'
}

if ('photos' -notin $Skip) {
    Add-Type -AssemblyName System.Drawing
    $dir = Reset-Folder 'photos'
    $width = 4000; $height = 3000
    $font = [System.Drawing.Font]::new('Segoe UI', 600, [System.Drawing.FontStyle]::Bold,
        [System.Drawing.GraphicsUnit]::Pixel)
    $format = [System.Drawing.StringFormat]::new()
    $format.Alignment = 'Center'; $format.LineAlignment = 'Center'
    $rect = [System.Drawing.Rectangle]::new(0, 0, $width, $height)
    for ($i = 1; $i -le 40; $i++) {
        $hue = $i * 9
        $from = [System.Drawing.Color]::FromArgb(255, (40 + $hue) % 256, 90, (560 - $hue) % 256)
        $to = [System.Drawing.Color]::FromArgb(255, 240, (120 + 3 * $hue) % 256, 60)
        $bitmap = [System.Drawing.Bitmap]::new($width, $height)
        $g = [System.Drawing.Graphics]::FromImage($bitmap)
        $brush = [System.Drawing.Drawing2D.LinearGradientBrush]::new($rect, $from, $to, [single]($i * 8))
        $g.FillRectangle($brush, $rect)
        $g.TextRenderingHint = 'AntiAlias'
        $g.DrawString(('{0:00}' -f $i), $font, [System.Drawing.Brushes]::White,
            [System.Drawing.RectangleF]$rect, $format)
        $bitmap.Save((Join-Path $dir ('IMG_{0:0000}.jpg' -f $i)),
            [System.Drawing.Imaging.ImageFormat]::Jpeg)
        $brush.Dispose(); $g.Dispose(); $bitmap.Dispose()
    }
    $font.Dispose(); $format.Dispose()
    Set-Content (Join-Path $dir 'IMG_0041.jpg') 'This is not a JPEG file.'
    New-Item -ItemType File (Join-Path $dir 'IMG_0042.jpg') | Out-Null
    Write-Host 'photos: 40 JPEGs 4000x3000, IMG_0041.jpg (text), IMG_0042.jpg (0 bytes)'
}
