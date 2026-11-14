Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("Enter space-separated integers:");
string[] parts = (Console.ReadLine() ?? "").Split(' ',
    StringSplitOptions.RemoveEmptyEntries);

int[] numbers = new int[parts.Length];
for (int i = 0; i < parts.Length; i++)
{
    if (!int.TryParse(parts[i], out numbers[i]))
    {
        Console.WriteLine($"“{parts[i]}” — not an integer.");
        return;
    }
}
if (numbers.Length < 2)
{
    Console.WriteLine("At least two numbers are required.");
    return;
}

Console.Write("Rotate right by k positions: ");
if (!int.TryParse(Console.ReadLine(), out int k))
{
    Console.WriteLine("An integer is required.");
    return;
}

int n = numbers.Length;
int shift = ((k % n) + n) % n;        // negative k rotates left
int[] shifted = new int[n];
for (int i = 0; i < n; i++)
{
    shifted[(i + shift) % n] = numbers[i];
}

int first = int.MinValue;
int second = int.MinValue;
foreach (int x in numbers)
{
    if (x > first)
    {
        second = first;
        first = x;
    }
    else if (x > second && x < first)
    {
        second = x;
    }
}

Console.WriteLine($"Original: [{string.Join(", ", numbers)}]");
Console.WriteLine($"After rotation: [{string.Join(", ", shifted)}]");
Console.WriteLine($"Maximum: {first}");
Console.WriteLine(second == int.MinValue
    ? "No second maximum: all numbers are equal."
    : $"Second maximum: {second}");
