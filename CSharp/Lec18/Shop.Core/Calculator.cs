namespace Shop.Core;

public class Calculator
{
    public int Add(int a, int b) => checked(a + b);

    public double Divide(double a, double b)
    {
        if (b == 0)
        {
            throw new DivideByZeroException("дільник дорівнює 0");
        }
        return a / b;
    }
}
