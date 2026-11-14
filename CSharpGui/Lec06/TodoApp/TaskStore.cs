namespace TodoApp;

public interface ITaskStore
{
    IReadOnlyList<string> Tasks { get; }
    void Add(string task);
}

// Сховище завдань у пам’яті.
public class MemoryTaskStore : ITaskStore
{
    private readonly List<string> tasks = [];

    public IReadOnlyList<string> Tasks => tasks;

    public void Add(string task) => tasks.Add(task);
}
