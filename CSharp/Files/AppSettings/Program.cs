using System.Text.Json;
using System.Text.Json.Serialization;

Console.OutputEncoding = System.Text.Encoding.UTF8;

string path = Path.Combine("data", "settings.json");
Directory.CreateDirectory("data");
JsonSerializerOptions options = new() { WriteIndented = true };

// 1. There is no file – default settings are created.
File.Delete(path);
AppSettings settings = LoadOrCreate(path, options);
Console.WriteLine($"Defaults: {settings}");

// 2. The user changed some of the values manually.
File.WriteAllText(path, """
    {
      "theme": "dark",
      "font_size": 18,
      "unknown_option": true
    }
    """);
settings = LoadOrCreate(path, options);
Console.WriteLine($"From the file: {settings}");

// 3. Invalid JSON – a warning and default values.
File.WriteAllText(path, """{ "font_size": "large" }""");
settings = LoadOrCreate(path, options);
Console.WriteLine($"After the error: {settings}");
Console.WriteLine(File.ReadAllText(path));

static AppSettings LoadOrCreate(
    string path, JsonSerializerOptions options)
{
    AppSettings? settings = null;
    if (File.Exists(path))
    {
        try
        {
            settings = JsonSerializer.Deserialize<AppSettings>(
                File.ReadAllText(path), options);
        }
        catch (JsonException e)
        {
            Console.WriteLine($"  JSON error at {e.Path}");
        }
    }
    settings ??= new AppSettings();
    settings.FontSize = Math.Clamp(settings.FontSize, 8, 32);
    string json = JsonSerializer.Serialize(settings, options);
    File.WriteAllText(path, json);
    return settings;
}

class AppSettings
{
    [JsonPropertyName("theme")]
    public string Theme { get; set; } = "light";

    [JsonPropertyName("font_size")]
    public int FontSize { get; set; } = 14;

    [JsonPropertyName("recent_files")]
    public List<string> RecentFiles { get; set; } = [];

    [JsonIgnore]                         // not saved to the file
    public bool IsDark => Theme == "dark";

    public override string ToString() =>
        $"{Theme}, {FontSize} pt, dark: {IsDark}";
}
