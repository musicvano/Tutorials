using Microsoft.EntityFrameworkCore;

namespace Shop.Orders;

// Продовжує саги, що «застрягли» (процес упав посеред саги або
// компенсацію не вдалося виконати через недоступний сервіс).
public class SagaRecovery(IServiceScopeFactory scopes,
    ILogger<SagaRecovery> log) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stop)
    {
        while (!stop.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(10), stop);
            try
            {
                await using var scope = scopes.CreateAsyncScope();
                var sp = scope.ServiceProvider;
                var db = sp.GetRequiredService<OrdersDb>();
                var saga = sp.GetRequiredService<OrderSaga>();
                DateTime old = DateTime.UtcNow.AddSeconds(-30);
                List<Order> stuck = await db.Orders
                    .Where(o => o.UpdatedAt < old &&
                        (o.Status == OrderStatus.Pending ||
                         o.Status == OrderStatus.Reserved ||
                         o.Status == OrderStatus.Compensating))
                    .ToListAsync(stop);
                foreach (Order o in stuck)
                {
                    log.LogInformation(
                        "Відновлення саги {Id} ({Status})", o.Id,
                        o.Status);
                    await saga.RunAsync(o);
                }
            }
            catch (Exception ex) when (!stop.IsCancellationRequested)
            {
                log.LogWarning("Відновлення: {Error}", ex.Message);
            }
        }
    }
}
