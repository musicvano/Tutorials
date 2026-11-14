using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;

const int Places = 3;
using SemaphoreSlim parking = new(Places, Places);
Stopwatch clock = Stopwatch.StartNew();
Lock consoleGate = new();
int parked = 0, maxParked = 0, refused = 0;

// Авто: номер, затримка приїзду, час стоянки, терпіння (мс).
(string Plate, int Arrive, int Stay, int Patience)[] cars =
[
    ("AA1001", 0, 400, 1000), ("AE2002", 20, 250, 1000),
    ("BC3003", 40, 600, 1000), ("KA4004", 60, 300, 1000),
    ("AX5005", 80, 200, 100), ("BI6006", 100, 300, 1000),
    ("CE7007", 120, 150, 50),
];

var threads = cars.Select(car => new Thread(() =>
{
    Thread.Sleep(car.Arrive);
    Log($"{car.Plate} під’їхав, вільно {parking.CurrentCount}");
    if (!parking.Wait(car.Patience))  // очікування з таймаутом
    {
        Interlocked.Increment(ref refused);
        Log($"{car.Plate} не дочекався місця й поїхав");
        return;
    }
    try
    {
        int now = Interlocked.Increment(ref parked);
        UpdateMax(now);
        Log($"{car.Plate} заїхав, зайнято {now}");
        Thread.Sleep(car.Stay);       // стоїть на парковці
    }
    finally
    {
        Interlocked.Decrement(ref parked);
        Log($"{car.Plate} виїхав");
        parking.Release();            // звільнити місце
    }
})).ToList();

threads.ForEach(t => t.Start());
threads.ForEach(t => t.Join());
Console.WriteLine($"Максимум одночасно: {maxParked} з {Places}, " +
    $"відмов: {refused}");

void UpdateMax(int value)
{
    int seen;
    do
    {
        seen = maxParked;               // CAS-цикл для максимуму
        if (value <= seen) return;
    } while (Interlocked.CompareExchange(
        ref maxParked, value, seen) != seen);
}

void Log(string message)
{
    lock (consoleGate)
    {
        Console.WriteLine(
            $"{clock.ElapsedMilliseconds / 10 * 10,5} мс  {message}");
    }
}
