using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// ∫ 2x·cos(x²) dx на [0; L] = sin(L²): коливання частішають
// праворуч, тому права частина відрізка потребує більше роботи.
double length = args.Length > 0 ? double.Parse(args[0]) : 200;
const double Eps = 1e-8;
double exact = Math.Sin(length * length);
Console.WriteLine($"L = {length}, ε = {Eps:E0}, " +
    $"точне значення {exact:F12}");
Console.WriteLine($"{"Спосіб",-25}{"Час, мс",9}{"S",6}" +
    $"{"Похибка",10}{"Задач",8}");

double value = 0;
double t0 = Median(() => value = Simpson.Whole(0, length, Eps));
Print("послідовно", t0, value, 0);

foreach (int parts in new[] { 16, 256 })
{
    double[] partTimes = new double[parts];
    double t = Median(() => value =
        Simpson.Static(0, length, Eps, parts, partTimes));
    Print($"статично, {parts} частин", t, value, parts);
    if (parts == 16)
        Console.WriteLine($"  час частин, мс: {partTimes.Min():F1}" +
            $" … {partTimes.Max():F1}");
}

foreach (int levels in new[] { 4, 8, 12, 16 })
{
    double t = Median(() => value =
        Simpson.Tasks(0, length, Eps, levels));
    Print($"задачі, поріг 2^{levels}", t, value, Simpson.TaskCount);
}

void Print(string name, double t, double v, long tasks) =>
    Console.WriteLine($"{name,-25}{t,9:F1}{t0 / t,6:F1}" +
        $"{Math.Abs(v - exact),10:E1}{tasks,8}");

static double Median(Action action, int runs = 7)
{
    action();
    Thread.Sleep(200);
    action();                            // прогрівання
    double[] times = new double[runs];
    for (int i = 0; i < runs; i++)
    {
        long start = Stopwatch.GetTimestamp();
        action();
        times[i] = Stopwatch.GetElapsedTime(start).TotalMilliseconds;
    }
    Array.Sort(times);
    return times[runs / 2];
}

static class Simpson
{
    const int MinDepth = 14;             // не менше 16 384 відрізків
    public static long TaskCount;

    static double F(double x) => 2 * x * Math.Cos(x * x);

    public static double Whole(double a, double b, double eps)
    {
        var (fa, fm, fb) = (F(a), F((a + b) / 2), F(b));
        double whole = Rule(a, b, fa, fm, fb);
        return Adapt(a, b, fa, fm, fb, whole, eps, 0);
    }

    static double Rule(double a, double b, double fa, double fm,
        double fb) => (b - a) / 6 * (fa + 4 * fm + fb);

    // Адаптивний Сімпсон: ділимо відрізок, доки оцінка похибки
    // (за правилом Рунге) не стане меншою за eps.
    static double Adapt(double a, double b, double fa, double fm,
        double fb, double whole, double eps, int depth)
    {
        double m = (a + b) / 2;
        double flm = F((a + m) / 2), frm = F((m + b) / 2);
        double left = Rule(a, m, fa, flm, fm);
        double right = Rule(m, b, fm, frm, fb);
        double delta = left + right - whole;
        if (depth >= MinDepth && Math.Abs(delta) <= 15 * eps
            || depth > 50)
            return left + right + delta / 15;
        return Adapt(a, m, fa, flm, fm, left, eps / 2, depth + 1)
             + Adapt(m, b, fm, frm, fb, right, eps / 2, depth + 1);
    }

    // Статичне розбиття на parts рівних частин (parts = 2^k).
    public static double Static(double a, double b, double eps,
        int parts, double[] partTimes)
    {
        double[] sums = new double[parts];
        double h = (b - a) / parts;
        Parallel.For(0, parts, k =>
        {
            long start = Stopwatch.GetTimestamp();
            double x0 = a + k * h, x1 = x0 + h;
            var (f0, fm, f1) = (F(x0), F(x0 + h / 2), F(x1));
            sums[k] = Adapt(x0, x1, f0, fm, f1,
                Rule(x0, x1, f0, fm, f1), eps / parts,
                (int)Math.Log2(parts));
            partTimes[k] = Stopwatch.GetElapsedTime(start)
                .TotalMilliseconds;
        });
        double total = 0;
        foreach (double s in sums) total += s;
        return total;
    }

    // Рекурсивні задачі: до глибини levels кожна половина стає
    // окремою задачею, глибше – звичайна рекурсія.
    public static double Tasks(double a, double b, double eps,
        int levels)
    {
        TaskCount = 0;
        var (fa, fm, fb) = (F(a), F((a + b) / 2), F(b));
        return AdaptTasks(a, b, fa, fm, fb, Rule(a, b, fa, fm, fb),
            eps, 0, levels);
    }

    static double AdaptTasks(double a, double b, double fa,
        double fm, double fb, double whole, double eps, int depth,
        int levels)
    {
        if (depth >= levels)
            return Adapt(a, b, fa, fm, fb, whole, eps, depth);
        double m = (a + b) / 2;
        double flm = F((a + m) / 2), frm = F((m + b) / 2);
        double left = Rule(a, m, fa, flm, fm);
        double right = Rule(m, b, fm, frm, fb);
        double delta = left + right - whole;
        if (depth >= MinDepth && Math.Abs(delta) <= 15 * eps)
            return left + right + delta / 15;
        double l = 0, r = 0;
        Interlocked.Add(ref TaskCount, 2);
        Parallel.Invoke(
            () => l = AdaptTasks(a, m, fa, flm, fm, left, eps / 2,
                depth + 1, levels),
            () => r = AdaptTasks(m, b, fm, frm, fb, right, eps / 2,
                depth + 1, levels));
        return l + r;
    }
}
