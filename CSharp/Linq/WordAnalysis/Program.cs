Console.OutputEncoding = System.Text.Encoding.UTF8;

string text = """
    The C# programming language lets you write programs for different
    platforms. Programs in C# are compiled into an intermediate language, and
    the .NET runtime executes programs on different platforms. The language
    supports object-oriented and functional programming.
    """;

string[] words = text
    .Split([' ', '.', ',', '\r', '\n'],
        StringSplitOptions.RemoveEmptyEntries)
    .Where(w => w.Length > 3)
    .Select(w => w.ToLower())
    .ToArray();

// The classic approach: grouping by word.
var top = words
    .GroupBy(w => w)
    .Select(g => new { Word = g.Key, Count = g.Count() })
    .OrderByDescending(x => x.Count)
    .ThenBy(x => x.Word)
    .Take(5);
Console.WriteLine("Top 5 (GroupBy):");
foreach (var x in top)
{
    Console.WriteLine($"  {x.Word,-16} {x.Count}");
}

// .NET 9+: CountBy – the same, but shorter.
var top3 = words.CountBy(w => w)
    .OrderByDescending(p => p.Value)
    .ThenBy(p => p.Key)
    .Take(3)
    .Select(p => $"{p.Key}×{p.Value}");
Console.WriteLine($"Top 3 (CountBy): {string.Join(", ", top3)}");

// Distribution of word lengths.
var lengths = words
    .GroupBy(w => w.Length)
    .OrderBy(g => g.Key)
    .Select(g => $"{g.Key}:{g.Count()}");
Console.WriteLine($"Length:count  {string.Join(" ", lengths)}");
Console.WriteLine($"Words > 3 letters: {words.Length}, " +
    $"distinct: {words.Distinct().Count()}, " +
    $"longest: {words.MaxBy(w => w.Length)}");
