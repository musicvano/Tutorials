using System.Globalization;

Console.OutputEncoding = System.Text.Encoding.UTF8;

const double KmPerMile = 1.609344;         // international mile
const double KmPerNauticalMile = 1.852;    // nautical mile

Console.Write("Speed, km/h: ");
string input = (Console.ReadLine() ?? "").Replace(',', '.');

if (!double.TryParse(input, NumberStyles.Float,
        CultureInfo.InvariantCulture, out double kmh)
    || kmh < 0 || double.IsInfinity(kmh))
{
    Console.WriteLine("Error: enter a nonnegative number.");
    return;
}

double mph = kmh / KmPerMile;
double knots = kmh / KmPerNauticalMile;
double ms = kmh * 1000 / 3600;

Console.WriteLine($"{"Unit",-12}{"Value",12}");
Console.WriteLine(new string('-', 24));
Console.WriteLine($"{"km/h",-12}{kmh,12:F2}");
Console.WriteLine($"{"mph",-12}{mph,12:F2}");
Console.WriteLine($"{"knots",-12}{knots,12:F2}");
Console.WriteLine($"{"m/s",-12}{ms,12:F2}");
