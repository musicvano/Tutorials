Console.Write("Секунд: ");
int seconds = int.Parse(Console.ReadLine() ?? "0");
int millis = checked(seconds * 1000);
