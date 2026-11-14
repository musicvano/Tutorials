using SQLite;

namespace PhotoDiary;

public class DiaryEntry
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string PhotoPath { get; set; } = "";
    public string Caption { get; set; } = "";
    public DateTime Date { get; set; }
}

public class DiaryDatabase(string path)
{
    private SQLiteAsyncConnection? connection;

    // Відкрити базу й створити таблицю під час першого звернення.
    private async Task<SQLiteAsyncConnection> GetConnectionAsync()
    {
        if (connection is null)
        {
            connection = new SQLiteAsyncConnection(path,
                SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create
                | SQLiteOpenFlags.SharedCache);
            await connection.CreateTableAsync<DiaryEntry>();
        }
        return connection;
    }

    public async Task<List<DiaryEntry>> GetAllAsync()
    {
        var db = await GetConnectionAsync();
        return await db.Table<DiaryEntry>()
            .OrderByDescending(e => e.Date).ToListAsync();
    }

    public async Task SaveAsync(DiaryEntry entry)
    {
        var db = await GetConnectionAsync();
        if (entry.Id == 0)
        {
            await db.InsertAsync(entry);   // Id заповнюється тут
        }
        else
        {
            await db.UpdateAsync(entry);
        }
    }

    // The book shows these two methods only as a comment.
    public async Task<DiaryEntry?> GetAsync(int id)
    {
        var db = await GetConnectionAsync();
        return await db.Table<DiaryEntry>().Where(e => e.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task DeleteAsync(DiaryEntry entry)
    {
        var db = await GetConnectionAsync();
        await db.DeleteAsync(entry);
    }
}
