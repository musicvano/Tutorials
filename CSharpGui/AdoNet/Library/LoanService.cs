using Npgsql;

namespace Library;

public class LoanService(NpgsqlDataSource dataSource)
{
    // Видача книги: зменшити copies і додати запис у loans
    // або не зробити нічого.
    public async Task<long> IssueAsync(int bookId, int readerId,
        int days)
    {
        await using NpgsqlConnection conn =
            await dataSource.OpenConnectionAsync();
        await using NpgsqlTransaction tx =
            await conn.BeginTransactionAsync();

        await using var take = new NpgsqlCommand("""
            UPDATE books SET copies = copies - 1
            WHERE id = $1 AND copies > 0
            """, conn, tx);
        take.Parameters.AddWithValue(bookId);
        if (await take.ExecuteNonQueryAsync() == 0)
        {
            await tx.RollbackAsync();
            throw new InvalidOperationException(
                $"Book {bookId} is not available.");
        }

        await using var insert = new NpgsqlCommand("""
            INSERT INTO loans (book_id, reader_id, due)
            VALUES ($1, $2, CURRENT_DATE + $3)
            RETURNING id
            """, conn, tx);
        insert.Parameters.AddWithValue(bookId);
        insert.Parameters.AddWithValue(readerId);
        insert.Parameters.AddWithValue(days);
        long loanId = (long)(await insert.ExecuteScalarAsync())!;

        await tx.CommitAsync();   // зафіксувати обидві зміни
        return loanId;
    }
}
