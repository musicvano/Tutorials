Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.Write("Number a (a ≥ 0): ");
if (!double.TryParse(Console.ReadLine(), out double a) || a < 0)
{
    Console.WriteLine("Error: a nonnegative number is required.");
    return;
}
Console.Write("Tolerance (for example, 1e-10): ");
if (!double.TryParse(Console.ReadLine(), out double eps)
    || eps <= 0)
{
    Console.WriteLine("Error: the tolerance must be positive.");
    return;
}

if (a == 0)
{
    Console.WriteLine("Root: 0");  // convergence is slow for zero
    return;
}

const int MaxIterations = 100;
double x = a > 1 ? a : 1;       // initial approximation
int iterations = 0;
double delta;

do
{
    double next = (x + a / x) / 2;  // Newton’s formula
    delta = Math.Abs(next - x);
    x = next;
    iterations++;
    Console.WriteLine($"{iterations,3}: x = {x:F12}");
} while (delta >= eps && iterations < MaxIterations);

Console.WriteLine($"Root: {x}");
Console.WriteLine($"Math.Sqrt: {Math.Sqrt(a)}");
Console.WriteLine($"Iterations: {iterations}");
