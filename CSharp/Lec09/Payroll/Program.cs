Console.OutputEncoding = System.Text.Encoding.UTF8;

Employee[] staff =
[
    new SalariedEmployee("Коваль О.", 32_000m),
    new HourlyEmployee("Бондар П.", 250m, 172),
    new HourlyEmployee("Мельник І.", 250m, 190),
    new SalariedEmployee("Ткаченко А.", 41_500m),
];

decimal total = 0;
foreach (Employee e in staff)
{
    decimal pay = e.CalculatePay();       // пізнє зв’язування
    total += pay;
    Console.WriteLine($"{e.Describe(),-40}{pay,10:N2}");
}
Console.WriteLine($"{"Разом",-40}{total,10:N2}");

