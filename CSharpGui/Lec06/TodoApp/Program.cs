using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TodoApp;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        HostApplicationBuilder builder =
            Host.CreateApplicationBuilder(args);
        builder.Services.Configure<TodoOptions>(
            builder.Configuration.GetSection("Todo"));
        builder.Services.AddSingleton<ITaskStore, MemoryTaskStore>();
        builder.Services.AddTransient<MainForm>();

        using IHost host = builder.Build();
        host.Start();
        Application.Run(host.Services.GetRequiredService<MainForm>());
        host.StopAsync().GetAwaiter().GetResult();
    }
}
