namespace TodoApp.Core;

public interface ITodoStore
{
    Task<List<TodoItem>> LoadAsync(CancellationToken token);
    Task SaveAsync(IEnumerable<TodoItem> items,
        CancellationToken token);
}
