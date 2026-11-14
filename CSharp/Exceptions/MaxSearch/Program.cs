Console.OutputEncoding = System.Text.Encoding.UTF8;

int[] temps = [12, 15, 9, 21, 18, 14, 25];

int max = FindMax(temps, out int day);
Console.WriteLine($"Maximum {max} °C on day {day + 1}");

// BUG: the loop does not reach the last element.
static int FindMax(int[] values, out int index)
{
    index = 0;
    for (int i = 1; i < values.Length - 1; i++)
    {
        if (values[i] > values[index])
        {
            index = i;
        }
    }
    return values[index];
}
