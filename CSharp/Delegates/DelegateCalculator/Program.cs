Console.OutputEncoding = System.Text.Encoding.UTF8;

Dictionary<string, Func<double, double, double>> operations = new()
{
    ["+"] = (a, b) => a + b,
    ["-"] = (a, b) => a - b,
    ["*"] = (a, b) => a * b,
    ["/"] = Divide,
    ["^"] = Math.Pow,
};
// A new operation is added without changing the evaluation code.
operations["max"] = Math.Max;

Console.WriteLine($"Operations: {string.Join(" ", operations.Keys)}");
string[] inputs =
    ["12 + 30", "2 ^ 10", "7 / 0", "3 max 8", "5 % 2", "x * 2"];
foreach (string input in inputs)
{
    Console.WriteLine($"{input,-8} → {Evaluate(input)}");
}

string Evaluate(string expression)
{
    string[] parts = expression.Split(' ');
    if (parts.Length != 3
        || !double.TryParse(parts[0], out double left)
        || !double.TryParse(parts[2], out double right))
    {
        return "format: number operation number";
    }

    if (!operations.TryGetValue(parts[1], out var operation))
    {
        return $"unknown operation “{parts[1]}”";
    }
    try
    {
        return operation(left, right).ToString();
    }
    catch (DivideByZeroException)
    {
        return "division by zero";
    }
}

static double Divide(double a, double b) =>
    b == 0 ? throw new DivideByZeroException() : a / b;
