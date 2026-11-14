namespace TodoApi;

public interface ITodoRepository
{
    Task<PagedResult<TodoItem>> GetPageAsync(bool? done,
        int page, int pageSize);
    Task<TodoItem?> FindAsync(int id);
    Task<TodoItem> AddAsync(TodoItem item);
    Task<bool> UpdateAsync(TodoItem item);
    Task<bool> DeleteAsync(int id);
}

// Дані в пам’яті процесу: зникають після перезапуску.
public class InMemoryTodoRepository : ITodoRepository
{
    private readonly List<TodoItem> items = [];
    private readonly Lock sync = new();   // запити паралельні
    private int nextId = 1;

    public Task<PagedResult<TodoItem>> GetPageAsync(bool? done,
        int page, int pageSize)
    {
        lock (sync)
        {
            var filtered = items
                .Where(t => done == null || t.IsDone == done)
                .ToList();
            var slice = filtered.Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();
            return Task.FromResult(new PagedResult<TodoItem>(
                slice, page, pageSize, filtered.Count));
        }
    }

    public Task<TodoItem?> FindAsync(int id)
    {
        lock (sync)
            return Task.FromResult(items.Find(t => t.Id == id));
    }

    public Task<TodoItem> AddAsync(TodoItem item)
    {
        lock (sync) { item.Id = nextId++; items.Add(item); }
        return Task.FromResult(item);
    }

    public Task<bool> UpdateAsync(TodoItem item)
    {
        lock (sync)
        {
            int i = items.FindIndex(t => t.Id == item.Id);
            if (i >= 0) items[i] = item;
            return Task.FromResult(i >= 0);
        }
    }

    public Task<bool> DeleteAsync(int id)
    {
        lock (sync)
            return Task.FromResult(
                items.RemoveAll(t => t.Id == id) > 0);
    }
}
