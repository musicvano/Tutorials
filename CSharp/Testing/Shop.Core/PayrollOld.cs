namespace Shop.Core;

// До рефакторингу: довгий метод, магічні числа, дублювання.
public static class PayrollOld
{
    public static decimal Calc(
        string type, decimal rate, int hours, int years)
    {
        decimal pay = 0;
        if (type == "hourly")
        {
            if (hours > 160)
            {
                pay = 160 * rate + (hours - 160) * rate * 1.5m;
            }
            else
            {
                pay = hours * rate;
            }
            if (years >= 5) pay = pay + pay * 0.1m;
        }
        else if (type == "salary")
        {
            pay = rate;
            if (years >= 5) pay = pay + pay * 0.1m;
        }
        pay = pay - pay * 0.195m;
        return Math.Round(pay, 2);
    }
}
