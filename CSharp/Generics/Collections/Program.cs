Console.OutputEncoding = System.Text.Encoding.UTF8;

int[] scores = [72, 95, 88, 61];
string[] cities = ["Мадрид", "Брюссель", "Париж", "Відень"];
DateOnly[] deadlines =
[
    new(2026, 10, 1), new(2026, 12, 20), new(2026, 11, 5),
];

// Аргумент типу компілятор виводить з аргументів методу.
Console.WriteLine($"Найбільший бал: {FindMax(scores)}");
Console.WriteLine($"Останнє місто за абеткою: {FindMax(cities)}");
Console.WriteLine($"Найпізніший дедлайн: {FindMax(deadlines)}");
Console.WriteLine($"Тип указано явно: {FindMax<double>([2.5, 7])}");

string first = "ліво", second = "право";
Swap(ref first, ref second);
Console.WriteLine($"Після обміну: {first}, {second}");

static T FindMax<T>(T[] items) where T : IComparable<T>
{
    if (items.Length == 0)
    {
        throw new ArgumentException("масив порожній", nameof(items));
    }

    T max = items[0];
    foreach (T item in items)
    {
        if (item.CompareTo(max) > 0)
        {
            max = item;
        }
    }
    return max;
}

static void Swap<T>(ref T a, ref T b) => (a, b) = (b, a);
