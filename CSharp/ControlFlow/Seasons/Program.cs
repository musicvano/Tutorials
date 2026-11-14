Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.Write("Month number (1–12): ");
if (!int.TryParse(Console.ReadLine(), out int month)
    || month is < 1 or > 12)
{
    Console.WriteLine("Error: the month must be from 1 to 12.");
    return;
}
Console.Write("Year: ");
if (!int.TryParse(Console.ReadLine(), out int year) || year < 1)
{
    Console.WriteLine("Error: the year must be positive.");
    return;
}

bool isLeap = (year % 4 == 0 && year % 100 != 0) || year % 400 == 0;

string season = month switch
{
    12 or 1 or 2 => "winter",
    >= 3 and <= 5 => "spring",
    >= 6 and <= 8 => "summer",
    _ => "fall",
};

int days = month switch
{
    2 => isLeap ? 29 : 28,
    4 or 6 or 9 or 11 => 30,
    _ => 31,
};

Console.WriteLine($"Season: {season}");
Console.WriteLine($"The year is {(isLeap ? "a leap year" : "not a leap year")}");
Console.WriteLine($"Days in the month: {days}");
