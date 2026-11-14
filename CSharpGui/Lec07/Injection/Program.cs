using Microsoft.Extensions.Configuration;
using Npgsql;

IConfiguration config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build();
string connectionString = config.GetConnectionString("Library")
    ?? throw new InvalidOperationException(
        "Connection string 'Library' is not configured.");

await using NpgsqlDataSource dataSource =
    NpgsqlDataSource.Create(connectionString);

Console.Write("Reader name: ");
string name = Console.ReadLine() ?? "";

// НЕБЕЗПЕЧНО: введений текст стає частиною команди SQL.
string sql = "SELECT id, name FROM readers "
    + $"WHERE name = '{name}' ORDER BY id";
Console.WriteLine(sql);
await using (NpgsqlCommand unsafeCmd = dataSource.CreateCommand(sql))
{
    await PrintReadersAsync("Concatenated query", unsafeCmd);
}

// Безпечно: значення передається окремо від тексту команди.
await using (NpgsqlCommand safeCmd = dataSource.CreateCommand(
    "SELECT id, name FROM readers WHERE name = $1 ORDER BY id"))
{
    safeCmd.Parameters.AddWithValue(name);
    await PrintReadersAsync("Parameterized query", safeCmd);
}

static async Task PrintReadersAsync(string title, NpgsqlCommand cmd)
{
    Console.WriteLine($"{title}:");
    await using NpgsqlDataReader reader =
        await cmd.ExecuteReaderAsync();
    int count = 0;
    while (await reader.ReadAsync())
    {
        Console.WriteLine(
            $"  {reader.GetInt32(0)} {reader.GetString(1)}");
        count++;
    }
    Console.WriteLine($"  rows: {count}");
}
