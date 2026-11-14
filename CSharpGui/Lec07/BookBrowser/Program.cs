using Microsoft.Extensions.Configuration;
using Npgsql;

namespace BookBrowser;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        IConfiguration config = new ConfigurationBuilder()
            .AddUserSecrets(typeof(Program).Assembly)
            .Build();
        string connectionString = config.GetConnectionString("Library")
            ?? throw new InvalidOperationException(
                "Connection string 'Library' is not configured.");

        using NpgsqlDataSource dataSource =
            NpgsqlDataSource.Create(connectionString);
        Application.Run(new MainForm(dataSource));
    }
}
