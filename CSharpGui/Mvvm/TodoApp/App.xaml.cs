using System.IO;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Core;

namespace TodoApp;

public partial class App : Application
{
    private ServiceProvider? services;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        string file = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData),
            "TodoApp", "todos.json");

        var collection = new ServiceCollection();
        collection.AddSingleton<ITodoStore>(new JsonTodoStore(file));
        collection.AddTransient<TodoViewModel>();
        collection.AddTransient<MainWindow>();
        services = collection.BuildServiceProvider();

        services.GetRequiredService<MainWindow>().Show();
    }
}
