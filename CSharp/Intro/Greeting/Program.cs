Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

Console.Write("Enter your name: ");
string? name = Console.ReadLine();

if (string.IsNullOrWhiteSpace(name))
{
    name = "student";
}

DateTime now = DateTime.Now;
Console.WriteLine($"Hello, {name}!");
Console.WriteLine($"Today is {now:dd.MM.yyyy}.");
Console.WriteLine($"Current time: {now:HH:mm}.");
