Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.Write("Number of rows (1–12): ");
if (!int.TryParse(Console.ReadLine(), out int rows)
    || rows is < 1 or > 12)
{
    Console.WriteLine("An integer from 1 to 12 is required.");
    return;
}

// Jagged array: row i has i + 1 elements.
long[][] triangle = new long[rows][];
for (int i = 0; i < rows; i++)
{
    triangle[i] = new long[i + 1];
    triangle[i][0] = 1;
    triangle[i][^1] = 1;
    for (int j = 1; j < i; j++)
    {
        triangle[i][j] = triangle[i - 1][j - 1] + triangle[i - 1][j];
    }
}

// The largest number is in the middle of the last row; the field width is even.
int digits = triangle[^1][(rows - 1) / 2].ToString().Length;
int width = 2 * (digits / 2 + 1);
for (int i = 0; i < rows; i++)
{
    Console.Write(new string(' ', (rows - 1 - i) * width / 2));
    foreach (long value in triangle[i])
    {
        Console.Write(value.ToString().PadLeft(width));
    }
    Console.WriteLine();
}

long rowSum = 0;
foreach (long value in triangle[^1])
{
    rowSum += value;
}
Console.WriteLine($"Last row sum: {rowSum} = 2^{rows - 1}");
