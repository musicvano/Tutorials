Console.OutputEncoding = System.Text.Encoding.UTF8;

var report = new SalesSummary(104_391m);

report.Print();                     // a regular class method
((IPrintable)report).Print();       // for printing on a printer
((ILoggable)report).Print();        // for the log

ILoggable log = report;
log.Print();

interface IPrintable
{
    void Print();
}

interface ILoggable
{
    void Print();
}

class SalesSummary(decimal total) : IPrintable, ILoggable
{
    public void Print() =>
        Console.WriteLine($"Sales: {total:N2} UAH");

    // Explicit implementation: accessible only through an interface variable.
    void IPrintable.Print()
    {
        Console.WriteLine("┌──────────────────────────┐");
        Console.WriteLine($"│ SALES   {total,14:N2} ₴ │");
        Console.WriteLine("└──────────────────────────┘");
    }

    void ILoggable.Print() =>
        Console.WriteLine($"2026-09-16 12:00:00 INFO sales={total}");
}
