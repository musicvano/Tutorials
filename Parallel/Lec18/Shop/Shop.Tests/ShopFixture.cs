using Aspire.Hosting;

namespace Shop.Tests;

// Один запуск застосунку (контейнери й сервіси) на клас тестів.
public class ShopFixture : IAsyncLifetime
{
    public DistributedApplication App { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        var host = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.Shop_AppHost>();
        App = await host.BuildAsync();
        await App.StartAsync();
        using CancellationTokenSource cts =
            new(TimeSpan.FromMinutes(3));
        string[] resources = ["gateway", "orders", "shipping"];
        foreach (string r in resources)
            await App.ResourceNotifications
                .WaitForResourceHealthyAsync(r, cts.Token);
    }

    public async Task DisposeAsync() => await App.DisposeAsync();
}
