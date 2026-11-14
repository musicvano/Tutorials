using Microsoft.Extensions.Http.Resilience;

namespace Shop.Orders;

// Стандартний конвеєр: обмеження частоти → тайм-аут усього виклику
// → повтори → запобіжник → тайм-аут однієї спроби.
public static class PaymentsPolicy
{
    public static void Configure(HttpStandardResilienceOptions o)
    {
        o.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(5);
        o.Retry.MaxRetryAttempts = 2;              // 3 спроби разом
        o.Retry.Delay = TimeSpan.FromMilliseconds(200);
        o.CircuitBreaker.FailureRatio = 0.5;     // половина невдалих
        o.CircuitBreaker.MinimumThroughput = 5;  // серед ≥ 5 викликів
        o.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(10);
        o.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(15);
        o.AttemptTimeout.Timeout = TimeSpan.FromSeconds(1);
    }
}
