using Common.Input;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

int age = ConsoleInput.ReadInt("Age: ", 10, 100);
int course = ConsoleInput.ReadChoice("Year of study: ",
    "first", "second", "third", "fourth");
double hours =
    ConsoleInput.ReadDouble("Hours of programming per week: ");

Console.WriteLine(
    $"Age {age}, year {course + 1}, {hours} h/week");
