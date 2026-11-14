using System.Text.RegularExpressions;

Console.OutputEncoding = System.Text.Encoding.UTF8;

string[] messages =
{
    "Ти ДУРЕНЬ чи що? Пиши на ivan.petrenko@example.com",
    "Ну й бовдури... Дзвоніть: +380 67 123 45 67",
    "Дурниці! Звичайне повідомлення без порушень.",
};

foreach (string message in messages)
{
    Console.Write("Позиції слів:");
    var matches = Censor.BadWord().EnumerateMatches(message);
    foreach (ValueMatch m in matches)
        Console.Write($" {m.Index}+{m.Length}");
    Console.WriteLine();

    string clean = Censor.Clean(message, out int count);
    Console.WriteLine($"{clean} (замін: {count})");
}

static partial class Censor
{
    // Корені слів і будь-які закінчення; регістр не важливий.
    [GeneratedRegex(@"\b(дур(ень|н[іяю])|бовдур|йолоп)\p{L}*",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    public static partial Regex BadWord();

    // E-mail або номер телефону з пробілами чи дефісами.
    [GeneratedRegex(@"[\w.+-]+@[\w-]+(\.[\w-]+)+|\+?\d[\d -]{8,}\d")]
    private static partial Regex Contact { get; }

    public static string Clean(string text, out int count)
    {
        int replaced = 0;
        string result = BadWord().Replace(text, m =>
        {
            replaced++;
            return m.Value[0] + new string('*', m.Length - 1);
        });
        result = Contact.Replace(result, m =>
        {
            replaced++;
            return "[контакт приховано]";
        });
        count = replaced;
        return result;
    }
}
