using System.Diagnostics;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

string[] items = ["Клавіатура", "Миша", "Монітор 27\"", "Навушники"];
int[] counts = [3, 5, 2, 4];
decimal[] prices = [1250m, 480m, 9999m, 1299.5m];

var report = new StringBuilder();
report.AppendLine("┌────────────────────┬──────┬───────────┐");
report.AppendLine($"│{"Товар",-20}│{"К-сть",6}│{"Сума",11}│");
report.AppendLine("├────────────────────┼──────┼───────────┤");

decimal total = 0;
for (int i = 0; i < items.Length; i++)
{
    decimal sum = counts[i] * prices[i];
    total += sum;
    report.AppendLine($"│{items[i],-20}│{counts[i],6}│{sum,11:N2}│");
}

report.AppendLine("├────────────────────┴──────┼───────────┤");
report.AppendLine($"│{"Разом",-27}│{total,11:N2}│");
report.Append("└───────────────────────────┴───────────┘");
string text2 = report.ToString();
Console.WriteLine(text2);
