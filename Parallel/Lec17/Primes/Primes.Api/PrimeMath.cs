namespace Primes;

// Повідомлення черги: частина [From, To) завдання JobId.
public record PrimeTask(string JobId, long From, long To);

public static class PrimeMath
{
    // Кількість простих чисел від from до to (не включно):
    // пробне ділення на непарні дільники до sqrt(n).
    public static long Count(long from, long to)
    {
        long count = 0;
        for (long n = Math.Max(from, 2); n < to; n++)
            if (IsPrime(n)) count++;
        return count;
    }

    static bool IsPrime(long n)
    {
        if (n < 4) return n >= 2;
        if (n % 2 == 0) return false;
        for (long d = 3; d * d <= n; d += 2)
            if (n % d == 0) return false;
        return true;
    }
}
