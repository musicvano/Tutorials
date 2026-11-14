Console.WriteLine(Health.Bmi(70, 1.75).ToString("F1"));

static class Health
{
    /// <summary>
    /// Обчислює індекс маси тіла.
    /// </summary>
    /// <param name="mass">Маса, кг.</param>
    /// <param name="height">Зріст, м.</param>
    /// <returns>Індекс маси тіла, кг/м².</returns>
    public static double Bmi(double mass, double height) =>
        mass / (height * height);
}
