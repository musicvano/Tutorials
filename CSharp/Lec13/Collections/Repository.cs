interface IEntity
{
    int Id { get; }
}

record Student(int Id, string Name) : IEntity;

record Course(int Id, string Title, int Credits) : IEntity;

// Узагальнений клас з обмеженнями: посилальний тип з Id.
class Repository<T> where T : class, IEntity
{
    private readonly List<T> items = [];

    public int Count => items.Count;

    public void Add(T item)
    {
        if (Find(item.Id) is not null)
        {
            throw new InvalidOperationException(
                $"{typeof(T).Name} з Id {item.Id} уже існує");
        }
        items.Add(item);
    }

    public T? Find(int id)
    {
        foreach (T item in items)
        {
            if (item.Id == id)
            {
                return item;
            }
        }
        return null;
    }

    public bool Remove(int id) =>
        Find(id) is T item && items.Remove(item);

    // Назовні – лише читання.
    public IReadOnlyList<T> GetAll() => items;
}
