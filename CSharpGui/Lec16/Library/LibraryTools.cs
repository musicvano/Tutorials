using System.ComponentModel;

public record Book(int Id, string Author, string Title, int Copies);

public class LibraryTools
{
    private readonly List<Book> books =
    [
        new(1, "Іван Франко", "Захар Беркут", 2),
        new(2, "Іван Франко", "Перехресні стежки", 0),
        new(3, "Леся Українка", "Лісова пісня", 3),
        new(4, "Ліна Костенко", "Маруся Чурай", 1)
    ];

    [Description("Шукає книги за частиною прізвища автора або назви")]
    public List<Book> SearchBooks(
        [Description("Прізвище автора або слово з назви")]
        string query)
    {
        return books
            .Where(b => b.Author.Contains(query,
                            StringComparison.OrdinalIgnoreCase)
                     || b.Title.Contains(query,
                            StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    [Description("Бронює примірник книги за її Id")]
    public string ReserveBook(
        [Description("Id книги з результатів пошуку")] int bookId)
    {
        int index = books.FindIndex(b => b.Id == bookId);
        if (index < 0) return "Книгу не знайдено.";
        Book book = books[index];
        if (book.Copies == 0) return "Вільних примірників немає.";

        // Дія змінює дані, тому її підтверджує людина, а не модель.
        Console.Write($"Забронювати «{book.Title}»? (y/n): ");
        if (Console.ReadLine()?.Trim().ToLower() != "y")
            return "Користувач скасував бронювання.";

        books[index] = book with { Copies = book.Copies - 1 };
        return $"Заброньовано «{book.Title}», залишилось " +
               $"{book.Copies - 1}.";
    }
}
