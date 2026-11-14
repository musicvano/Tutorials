using System.Text;

namespace Shop.Core;

public record SaleLine(string Product, decimal Amount);

// Після рефакторингу: розбір, розрахунок і форматування окремо.
public static class SalesReport
{
    private const decimal BulkThreshold = 5000m;
    private const decimal BulkDiscount = 0.03m;

    public static string Make(string[] lines)
    {
        List<SaleLine> sales = Parse(lines);
        StringBuilder report = new();
        foreach (SaleLine sale in sales)
        {
            report.Append(FormatLine(sale)).Append('\n');
        }
        decimal total = sales.Sum(s => s.Amount);
        report.Append($"Разом: {total:F2} ({sales.Count} поз.)");
        return report.ToString();
    }

    private static List<SaleLine> Parse(string[] lines)
    {
        List<SaleLine> sales = [];
        foreach (string line in lines)
        {
            string[] fields = line.Split(';');
            if (fields.Length != 3)
            {
                continue;                     // як і раніше: пропуск
            }
            decimal price = decimal.Parse(fields[1]);
            int quantity = int.Parse(fields[2]);
            decimal amount = ApplyDiscount(price * quantity);
            sales.Add(new SaleLine(fields[0], amount));
        }
        return sales;
    }

    private static decimal ApplyDiscount(decimal amount) =>
        amount > BulkThreshold ? amount * (1 - BulkDiscount) : amount;

    private static string FormatLine(SaleLine sale) =>
        $"{sale.Product,-12}{sale.Amount,10:F2}";
}
