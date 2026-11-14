using Library;
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
var books = new BookRepository(dataSource);
var loans = new LoanService(dataSource);

try
{
    Console.WriteLine($"Books: {await books.CountAsync()}");
    int id = await books.AddAsync("Code Complete", 2004,
        "978-0735619678", 2);
    Console.WriteLine($"Added book {id}");
    Console.WriteLine(
        $"Updated: {await books.UpdateCopiesAsync(id, 5)}");
    foreach (Book b in await books.FindAsync("code"))
    {
        Console.WriteLine(
            $"{b.Id,3} {b.Title,-15} {b.PubYear,5} {b.Copies,3}");
    }

    long loanId = await loans.IssueAsync(id, readerId: 3, days: 14);
    Console.WriteLine($"Loan {loanId} issued");
    await loans.IssueAsync(bookId: 4, readerId: 3, days: 14);
}
catch (InvalidOperationException ex)
{
    Console.Error.WriteLine(ex.Message);
}
catch (PostgresException ex)
    when (ex.SqlState == PostgresErrorCodes.UniqueViolation)
{
    Console.Error.WriteLine($"Duplicate: {ex.ConstraintName}");
}
catch (NpgsqlException ex)
{
    Console.Error.WriteLine($"Database error: {ex.Message}");
}
