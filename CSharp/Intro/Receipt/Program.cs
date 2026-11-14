Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

Console.Write("Назва товару: ");
string name = Console.ReadLine() ?? "";
Console.Write("Ціна за одиницю, грн: ");
decimal price = decimal.Parse(Console.ReadLine() ?? "0");
Console.Write("Кількість: ");
int count = int.Parse(Console.ReadLine() ?? "0");

decimal total = price * count;
decimal vat = total * 0.2m / 1.2m; // ПДВ 20 % у складі ціни
string line = new string('-', 38);

Console.WriteLine();
Console.WriteLine($"{"Товар",-20}{"К-сть",6}{"Сума",12}");
Console.WriteLine(line);
Console.WriteLine($"{name,-20}{count,6}{total,12:N2}");
Console.WriteLine(line);
Console.WriteLine($"{"Разом до сплати:",-26}{total,12:N2}");
Console.WriteLine($"{"у т. ч. ПДВ:",-26}{vat,12:N2}");
