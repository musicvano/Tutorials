namespace Shop.Core;

// Наданий код: усе в одному методі.
public static class SalesReportOld
{
    public static string Make(string[] lines)
    {
        string r = "";
        decimal t = 0;
        int n = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string[] p = lines[i].Split(';');
            if (p.Length == 3)
            {
                decimal a = decimal.Parse(p[1]) * int.Parse(p[2]);
                if (a > 5000) a = a - a * 0.03m;
                r += p[0].PadRight(12) + a.ToString("F2").PadLeft(10)
                    + "\n";
                t = t + a;
                n++;
            }
        }
        r += "Разом: " + t.ToString("F2") + " (" + n + " поз.)";
        return r;
    }
}
