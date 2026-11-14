Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.Write("Відстань, км: ");
int distance = int.Parse(Console.ReadLine() ?? "0");
Console.Write("Середня швидкість, км/год: ");
int speed = int.Parse(Console.ReadLine() ?? "0");

int totalMinutes = distance * 60 / speed; // ціле ділення
int hours = totalMinutes / 60;
int minutes = totalMinutes % 60;          // остача від ділення

DateTime arrival = DateTime.Now.AddMinutes(totalMinutes);
Console.WriteLine($"Час у дорозі: {hours} год {minutes} хв");
Console.WriteLine($"Прибуття приблизно о {arrival:HH:mm}");
