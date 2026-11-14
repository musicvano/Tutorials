namespace Bank.Core;

// Допоміжний клас доступний лише всередині збірки Bank.Core.
internal static class Validator
{
    public static string RequireName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return name.Trim();
    }

    public static void RequirePositive(decimal amount) =>
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
}
