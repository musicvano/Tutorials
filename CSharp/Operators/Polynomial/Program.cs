Console.OutputEncoding = System.Text.Encoding.UTF8;

Polynomial p = new();
p[2] = 3;           // 3x^2
p[0] = 1;           // + 1
p[1] = -2;          // − 2x

Polynomial q = new();
q[3] = 1;
q[2] = -3;
q[0] = 4;

Console.WriteLine($"p(x) = {p}");
Console.WriteLine($"q(x) = {q}");
Console.WriteLine($"p + q = {p + q}");
Console.WriteLine($"Coefficient of x^5 in p: {p[5]}");
Console.WriteLine($"p(2) = {p.Evaluate(2)}, q(2) = {q.Evaluate(2)}");
Console.WriteLine($"(p + q)(3) = {(p + q).Evaluate(3)}");

class Polynomial
{
    private double[] coefficients = [];

    public int Degree
    {
        get
        {
            for (int i = coefficients.Length - 1; i >= 0; i--)
                if (coefficients[i] != 0)
                    return i;
            return 0;
        }
    }

    // Indexer: power → coefficient.
    public double this[int power]
    {
        get
        {
            ArgumentOutOfRangeException.ThrowIfNegative(power);
            return power < coefficients.Length
                ? coefficients[power]
                : 0;
        }
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(power);
            if (power >= coefficients.Length)
            {
                Array.Resize(ref coefficients, power + 1);
            }
            coefficients[power] = value;
        }
    }

    public static Polynomial operator +(Polynomial a, Polynomial b)
    {
        Polynomial sum = new();
        for (int i = Math.Max(a.Degree, b.Degree); i >= 0; i--)
        {
            sum[i] = a[i] + b[i];
        }
        return sum;
    }

    // Horner's method.
    public double Evaluate(double x)
    {
        double result = 0;
        for (int i = Degree; i >= 0; i--)
        {
            result = result * x + this[i];
        }
        return result;
    }

    public override string ToString()
    {
        var text = new System.Text.StringBuilder();
        for (int i = Degree; i >= 0; i--)
        {
            double c = this[i];
            if (c == 0 && !(i == 0 && text.Length == 0)) continue;
            text.Append(text.Length == 0
                ? (c < 0 ? "-" : "")
                : (c < 0 ? " - " : " + "));
            double abs = Math.Abs(c);
            if (abs != 1 || i == 0) text.Append(abs);
            if (i > 0) text.Append(i == 1 ? "x" : $"x^{i}");
        }
        return text.ToString();
    }
}
