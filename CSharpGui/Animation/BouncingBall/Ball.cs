namespace BouncingBall;

// Модель м’ячика: центр і швидкість у пікселях (DIP).
public class Ball
{
    public double X { get; set; } = 40;
    public double Y { get; set; } = 40;
    public double VX { get; set; } = 180;    // пікселів за секунду
    public double VY { get; set; }
    public double Radius { get; } = 20;
    public double Gravity { get; set; } = 900;
    public double Elasticity { get; set; } = 0.8;

    // Рух за dt секунд у полі width × height.
    // Повертає true, якщо м’ячик ударився об підлогу.
    public bool Step(double dt, double width, double height)
    {
        VY += Gravity * dt;
        X += VX * dt;
        Y += VY * dt;
        if (X < Radius || X > width - Radius)
        {
            X = Math.Clamp(X, Radius, width - Radius);
            VX = -VX;                        // відбиття від стіни
        }
        if (Y > height - Radius && VY > 0)
        {
            Y = height - Radius;
            VY = -VY * Elasticity;           // втрата енергії
            return true;
        }
        return false;
    }
}
