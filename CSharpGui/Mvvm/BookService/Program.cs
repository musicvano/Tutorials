var app = WebApplication.Create(args);

Book[] books =
[
    new(1, "Kobzar", "Taras Shevchenko", 1840),
    new(2, "Clean Code", "Robert C. Martin", 2008),
    new(3, "The Pragmatic Programmer", "D. Thomas, A. Hunt", 2019),
    new(4, "C# 12 in a Nutshell", "Joseph Albahari", 2023),
    new(5, "Code Complete", "Steve McConnell", 2004)
];

app.MapGet("/api/books", async (string? search,
    CancellationToken token) =>
{
    await Task.Delay(2000, token);   // імітація повільного сервісу
    return books.Where(b => string.IsNullOrEmpty(search)
        || b.Title.Contains(search,
            StringComparison.OrdinalIgnoreCase));
});

app.Run("http://localhost:5080");

record Book(int Id, string Title, string Author, int Year);
