using System.Net.Http;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace BookClient;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var services = new ServiceCollection();
        services.AddSingleton(new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5080/"),
            Timeout = TimeSpan.FromSeconds(10)
        });
        services.AddTransient<BookSearchViewModel>();
        services.AddTransient<MainWindow>();
        services.BuildServiceProvider()
            .GetRequiredService<MainWindow>().Show();
    }
}
