using Npgsql;

namespace Library;

// Репозиторій: увесь SQL для таблиці books в одному класі.
public class BookRepository(NpgsqlDataSource dataSource)
{
    public async Task<long> CountAsync()
    {
        await using NpgsqlCommand cmd =
            dataSource.CreateCommand("SELECT count(*) FROM books");
        return (long)(await cmd.ExecuteScalarAsync())!;
    }

    public async Task<int> AddAsync(string title, int? pubYear,
        string? isbn, int copies)
    {
        await using NpgsqlCommand cmd = dataSource.CreateCommand("""
            INSERT INTO books (title, pub_year, isbn, copies)
            VALUES ($1, $2, $3, $4)
            RETURNING id
            """);
        cmd.Parameters.AddWithValue(title);
        cmd.Parameters.AddWithValue((object?)pubYear ?? DBNull.Value);
        cmd.Parameters.AddWithValue((object?)isbn ?? DBNull.Value);
        cmd.Parameters.AddWithValue(copies);
        return (int)(await cmd.ExecuteScalarAsync())!;
    }

    public async Task<int> UpdateCopiesAsync(int id, int copies)
    {
        await using NpgsqlCommand cmd = dataSource.CreateCommand(
            "UPDATE books SET copies = $1 WHERE id = $2");
        cmd.Parameters.AddWithValue(copies);
        cmd.Parameters.AddWithValue(id);
        return await cmd.ExecuteNonQueryAsync();  // к-сть рядків
    }

    public async Task<List<Book>> FindAsync(string text)
    {
        await using NpgsqlCommand cmd = dataSource.CreateCommand("""
            SELECT id, title, pub_year, isbn, copies
            FROM books
            WHERE title ILIKE $1
            ORDER BY title
            """);
        cmd.Parameters.AddWithValue($"%{text}%");
        await using NpgsqlDataReader reader =
            await cmd.ExecuteReaderAsync();
        var books = new List<Book>();
        while (await reader.ReadAsync())
        {
            books.Add(new Book(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.IsDBNull(2) ? null : reader.GetInt32(2),
                reader.IsDBNull(3) ? null : reader.GetString(3),
                reader.GetFieldValue<int>(4)));
        }
        return books;
    }
}
