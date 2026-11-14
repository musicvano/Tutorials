Console.OutputEncoding = System.Text.Encoding.UTF8;

string[] expressions =
[
    "(a + b) * [c - {d / e}]",
    "arr[i] = (x + y]",
    "f(g(x)",
    "{ return a); }",
    "no brackets",
];

foreach (string expression in expressions)
{
    string? error = CheckBrackets(expression);
    Console.WriteLine($"{expression,-24} {error ?? "OK"}");
}

// Returns a description of the first error, or null.
static string? CheckBrackets(string text)
{
    Stack<(char Bracket, int Position)> open = new();
    for (int i = 0; i < text.Length; i++)
    {
        char c = text[i];
        if (c is '(' or '[' or '{')
        {
            open.Push((c, i));
        }
        else if (c is ')' or ']' or '}')
        {
            if (open.Count == 0)
            {
                return $"extra “{c}” at position {i}";
            }
            var (bracket, position) = open.Pop();
            if (Pair(bracket) != c)
            {
                return $"“{bracket}”({position}) closed by “{c}”({i})";
            }
        }
    }
    return open.TryPeek(out var last)
        ? $"“{last.Bracket}” not closed ({last.Position})"
        : null;
}

static char Pair(char bracket) => bracket switch
{
    '(' => ')',
    '[' => ']',
    _ => '}',
};
