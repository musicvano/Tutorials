using System.Windows;

namespace Registration;

public partial class App : Application
{
    // "RegistrationFluent.exe dark" (or `dotnet run -- dark`) starts in the
    // dark mode, so App.xaml need not be edited for the second capture.
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
#pragma warning disable WPF0001   // ThemeMode – експериментальний API
        if (e.Args.Contains("dark", StringComparer.OrdinalIgnoreCase))
        {
            ThemeMode = ThemeMode.Dark;
        }
#pragma warning restore WPF0001
    }
}
