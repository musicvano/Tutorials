Console.OutputEncoding = System.Text.Encoding.UTF8;

List<Student> students =
[
    new("Коваль Олена", "ПІ-21", 4.7),
    new("Бондар Петро", "ПІ-22", 3.6),
    new("Мельник Ірина", "ПІ-21", 4.2),
    new("Ткач Андрій", "ПІ-22", 4.9),
    new("Шевчук Марія", "ПІ-21", 3.9),
];

// Синтаксис методів.
var byMethods = students
    .Where(s => s.Average >= 4.0)
    .OrderByDescending(s => s.Average)
    .Select(s => new { s.Name, Grade = Math.Round(s.Average) });

Console.WriteLine("Відмінники й хорошисти:");
Console.OutputEncoding = System.Text.Encoding.UTF8;

Sale[] sales =
[
    new("Кобзар", "Книги", 3, 350m),
    new("Шахи", "Ігри", 2, 890m),
    new("Навушники", "Техніка", 1, 2_499m),
    new("Тигролови", "Книги", 5, 280m),
    new("Монополія", "Ігри", 1, 1_150m),
    new("Миша", "Техніка", 4, 449m),
    new("Маруся", "Книги", 2, 310m),
    new("Павербанк", "Техніка", 2, 1_199m),
];

// Класичний спосіб: GroupBy і агрегування в кожній групі.
var report = sales
    .GroupBy(s => s.Category)
    .Select(g => new Summary(
        g.Key, g.Count(),
        g.Sum(s => s.Quantity * s.Price),
        g.MaxBy(s => s.Quantity * s.Price)!.Product))
    .OrderByDescending(r => r.Revenue).ToList();

Console.WriteLine(report.Count);

record Student(string Name, string Group, double Average);
record Sale(string Product, string Category, int Quantity, decimal Price);

record Summary(string Category, int Count, decimal Revenue, string Best);
