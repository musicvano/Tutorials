Console.OutputEncoding = System.Text.Encoding.UTF8;

int from = ReadInt("Start of range: ");
int to = ReadInt("End of range: ");
if (from > to)
{
    (from, to) = (to, from);          // swap the endpoints
}

int count = 0;
for (int n = from; n <= to; n++)
{
    if (n < 2)
    {
        continue;                     // 0 and 1 are not prime
    }

    bool isPrime = true;
    for (int d = 2; d * d <= n; d++)
    {
        if (n % d == 0)
        {
            isPrime = false;
            break;                    // a divisor was found
        }
    }
    if (!isPrime)
    {
        continue;
    }

    count++;
    Console.Write($"{n,6}");
    if (count % 10 == 0)
    {
        Console.WriteLine();          // 10 numbers per line
    }
}

if (count % 10 != 0)
{
    Console.WriteLine();
}
Console.WriteLine($"Prime numbers: {count}");

static int ReadInt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out int value))
        {
            return value;
        }
        Console.WriteLine("An integer is required.");
    }
}
