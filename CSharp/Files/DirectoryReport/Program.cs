Console.OutputEncoding = System.Text.Encoding.UTF8;

// Preparing a test directory with files of known size.
string root = Path.Combine("data", "project");
if (Directory.Exists(root))
{
    Directory.Delete(root, recursive: true);
}
CreateFile("README.md", 1_200);
CreateFile(Path.Combine("src", "Program.cs"), 3_400);
CreateFile(Path.Combine("src", "Models", "Book.cs"), 900);
CreateFile(Path.Combine("docs", "guide.md"), 5_600);
CreateFile(Path.Combine("docs", "images", "logo.png"), 48_000);
CreateFile(Path.Combine("docs", "images", "scheme.png"), 131_000);

DirectoryInfo directory = new(root);
FileInfo[] files =
    directory.GetFiles("*", SearchOption.AllDirectories);

Console.WriteLine($"Directory {directory.Name}: {files.Length} files");
var byExtension = files
    .GroupBy(f => f.Extension.ToLowerInvariant())
    .Select(g =>
        (Ext: g.Key, Count: g.Count(), Size: g.Sum(f => f.Length)))
    .OrderByDescending(x => x.Size);
foreach (var (ext, count, total) in byExtension)
{
    Console.WriteLine($"  {ext,-5}{count} pcs {FormatSize(total),9}");
}

FileInfo largest = files.MaxBy(f => f.Length)!;
string relative = Path.GetRelativePath(root, largest.FullName);
string size = FormatSize(largest.Length);
Console.WriteLine($"Largest: {relative} ({size})");
Console.WriteLine($"Subdirectories: {directory.GetDirectories("*",
    SearchOption.AllDirectories).Length}");

void CreateFile(string relativePath, int size)
{
    string full = Path.Combine(root, relativePath);
    Directory.CreateDirectory(Path.GetDirectoryName(full)!);
    File.WriteAllBytes(full, new byte[size]);
}

static string FormatSize(long bytes) => bytes switch
{
    < 1024 => $"{bytes} B",
    < 1024 * 1024 => $"{bytes / 1024.0:F1} KB",
    _ => $"{bytes / (1024.0 * 1024):F1} MB",
};
