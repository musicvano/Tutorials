using System.Text.Json;

namespace TodoApp.Core;

// Сховище справ у файлі JSON (описане в лекції лише текстом).
public class JsonTodoStore(string path) : ITodoStore
{
    public async Task<List<TodoItem>> LoadAsync(CancellationToken token)
    {
        if (!File.Exists(path))
        {
            return [];
        }
        await using FileStream stream = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<List<TodoItem>>(
            stream, cancellationToken: token) ?? [];
    }

    public async Task SaveAsync(IEnumerable<TodoItem> items,
        CancellationToken token)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        await using FileStream stream = File.Create(path);
        await JsonSerializer.SerializeAsync(stream, items.ToList(),
            cancellationToken: token);
    }
}
