Console.OutputEncoding = System.Text.Encoding.UTF8;

const decimal BaseFare = 60m;        // base fare, UAH
const decimal PricePerKm = 18.50m;   // UAH per kilometer
const decimal PricePerMinute = 3m;   // UAH per minute of travel

Console.Write("Distance, km: ");
decimal distance = decimal.Parse(Console.ReadLine() ?? "0");
Console.Write("Trip duration, s: ");
int durationSeconds = int.Parse(Console.ReadLine() ?? "0");
Console.Write("Promo code, % discount (Enter – no discount): ");
string? promo = Console.ReadLine();

// A partial minute is charged as a full minute.
int minutes = (int)Math.Ceiling(durationSeconds / 60.0);

// int? – an integer or null if no discount was entered.
int? discount = int.TryParse(promo, out int percent)
    ? percent
    : null;
int discountPercent = Math.Clamp(discount ?? 0, 0, 50);

decimal cost = BaseFare + distance * PricePerKm
    + minutes * PricePerMinute;
decimal discountSum = Math.Round(cost * discountPercent / 100, 2);
decimal toPay = cost - discountSum;

Console.WriteLine($"Billable minutes: {minutes}");
Console.WriteLine($"Fare:  {cost,10:N2} UAH");
Console.WriteLine($"Discount {discountPercent,2} %: {discountSum,7:N2} UAH");
Console.WriteLine($"Total due: {toPay,10:N2} UAH");
