using System.Globalization;

Console.OutputEncoding = System.Text.Encoding.UTF8;

double width = ReadPositive("Rectangle width, cm: ");
double height = ReadPositive("Rectangle height, cm: ");

double area = width * height;
double perimeter = 2 * (width + height);

Console.WriteLine($"Area: {area:F2} cm²");
Console.WriteLine($"Perimeter: {perimeter:F2} cm");

// Reads a positive number; the separator can be a comma or a period.
static double ReadPositive(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string input = Console.ReadLine() ?? "";
        input = input.Replace(',', '.');
        if (double.TryParse(input, NumberStyles.Float,
            CultureInfo.InvariantCulture, out double value)
            && value > 0)
        {
            return value;
        }
        Console.WriteLine("Error: enter a positive number.");
    }
}
