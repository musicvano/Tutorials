using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

Console.OutputEncoding = System.Text.Encoding.UTF8;

JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
{
    WriteIndented = true,
    Converters = { new JsonStringEnumConverter() },
    // Кирилиця без екранування \uXXXX.
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
};
string path = Path.Combine("data", "books.json");
Directory.CreateDirectory("data");
File.Delete(path);

List<Book> books = Load(path, options);
Console.WriteLine($"Завантажено книг: {books.Count}");

books.Add(new("Тигролови", 1944, ["Іван Багряний"], Genre.Novel));
books.Add(new("Лісова пісня", 1911, ["Леся Українка"], Genre.Drama));
Save(path, books, options);
string json = JsonSerializer.Serialize(books, options);

Console.WriteLine(File.ReadAllText(path));
List<Book> loaded = Load(path, options);
Console.WriteLine($"Завантажено книг: {loaded.Count}, " +
    $"однакові: {loaded.SequenceEqual(books, new BookComparer())}");

static List<Book> Load(string path, JsonSerializerOptions options)
{
    if (!File.Exists(path))
    {
        return [];            // перший запуск
    }
    string json = File.ReadAllText(path);
    return JsonSerializer.Deserialize<List<Book>>(json, options)
        ?? [];
}

static void Save(
    string path, List<Book> books, JsonSerializerOptions options)
{
    // Запис у тимчасовий файл і заміна: старі дані не зіпсуються.
    string temp = path + ".tmp";
    File.WriteAllText(temp, JsonSerializer.Serialize(books, options));
    File.Move(temp, path, overwrite: true);
}

enum Genre { Novel, Poetry, Drama }

record Book(string Title, int Year, string[] Authors, Genre Genre);

// Масиви в записах порівнюються за посиланням – власний порівнювач.
class BookComparer : IEqualityComparer<Book>
{
    public bool Equals(Book? x, Book? y) =>
        x is not null && y is not null && x.Title == y.Title
        && x.Year == y.Year && x.Genre == y.Genre
        && x.Authors.SequenceEqual(y.Authors);

    public int GetHashCode(Book b) =>
        HashCode.Combine(b.Title, b.Year);
}
