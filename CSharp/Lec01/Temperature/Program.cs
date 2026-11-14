Console.OutputEncoding = System.Text.Encoding.UTF8;

double celsius = ReadNumber("Температура за Цельсієм: ");
double fahrenheit = celsius * 9 / 5 + 32;
double kelvin = celsius + 273.15;

Console.WriteLine($"За Фаренгейтом: {fahrenheit:F1} °F");
Console.WriteLine($"За Кельвіном: {kelvin:F2} K");

// Повторює запит, доки користувач не введе число.
static double ReadNumber(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();
        if (double.TryParse(input, out double value))
        {
            return value;
        }
        Console.WriteLine("Це не число, спробуйте ще раз.");
    }
}
