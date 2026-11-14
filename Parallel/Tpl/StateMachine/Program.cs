Console.OutputEncoding = System.Text.Encoding.UTF8;

// Два тестові файли для методу ReadAsync.
string p1 = Path.Combine(Path.GetTempPath(), "state-machine-1.txt");
string p2 = Path.Combine(Path.GetTempPath(), "state-machine-2.txt");
await File.WriteAllTextAsync(p1, "перший файл");
await File.WriteAllTextAsync(p2, "другий файл");

int length = await Demo.ReadAsync(p1, p2);
Console.WriteLine($"Символів: {length}");

static class Demo
{
    // Компілятор створює вкладену машину станів <ReadAsync>d__0.
    public static async Task<int> ReadAsync(string p1, string p2)
    {
        Console.WriteLine("старт");
        var a = await File.ReadAllTextAsync(p1);
        var b = await File.ReadAllTextAsync(p2);
        return a.Length + b.Length;
    }
}
