Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("Enter three space-separated integers:");
string[] parts = (Console.ReadLine() ?? "").Split(' ',
    StringSplitOptions.RemoveEmptyEntries);

if (parts.Length != 3
    || !int.TryParse(parts[0], out int a)
    || !int.TryParse(parts[1], out int b)
    || !int.TryParse(parts[2], out int c))
{
    Console.WriteLine("Exactly three integers are required.");
    return;
}

int swaps = SortThree(ref a, ref b, ref c);
Console.WriteLine($"Ascending: {a} {b} {c}");
Console.WriteLine($"Swaps: {swaps}");

// Sorts three caller variables; returns the swap count.
static int SortThree(ref int x, ref int y, ref int z)
{
    int count = 0;
    if (x > y) { Swap(ref x, ref y); count++; }
    if (y > z) { Swap(ref y, ref z); count++; }
    if (x > y) { Swap(ref x, ref y); count++; }
    return count;
}

// Swaps the values of two caller variables.
static void Swap(ref int first, ref int second)
{
    int temp = first;
    first = second;
    second = temp;
}
