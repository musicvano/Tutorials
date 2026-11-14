Console.OutputEncoding = System.Text.Encoding.UTF8;

BoundedBuffer<int> buffer = new(capacity: 3);
const int Producers = 2, Consumers = 3, ItemsPerProducer = 20;
long consumedSum = 0;
int[] consumedBy = new int[Consumers];

var threads = new List<Thread>();
for (int p = 0; p < Producers; p++)
{
    int id = p;
    threads.Add(new Thread(() =>
    {
        for (int i = 1; i <= ItemsPerProducer; i++)
        {
            buffer.Put(id * 100 + i);   // 1…20 та 101…120
            if (i % 5 == 0) Thread.Sleep(30);  // пауза після партії
        }
    }));
}
for (int c = 0; c < Consumers; c++)
{
    int id = c;
    threads.Add(new Thread(() =>
    {
        // Порожній буфер після завершення виробників – кінець.
        while (buffer.TryTake(out int item))
        {
            Interlocked.Add(ref consumedSum, item);
            consumedBy[id]++;
            Thread.Sleep(1);            // обробка елемента
        }
    }));
}
foreach (var t in threads) t.Start();
foreach (var t in threads.Take(Producers)) t.Join();
buffer.Complete();                      // більше елементів не буде
foreach (var t in threads.Skip(Producers)) t.Join();

long expected = 0;
for (int p = 0; p < Producers; p++)
    for (int i = 1; i <= ItemsPerProducer; i++)
        expected += p * 100 + i;

Console.WriteLine($"Спожито: {string.Join(" + ", consumedBy)} = " +
    $"{consumedBy.Sum()} елементів");
Console.WriteLine($"Сума: {consumedSum} (очікувано {expected})");
Console.WriteLine($"Максимальна заповненість: {buffer.MaxCount} " +
    $"з {buffer.Capacity}");
Console.WriteLine($"Очікувань: виробники {buffer.PutWaits}, " +
    $"споживачі {buffer.TakeWaits}");

// Обмежений буфер на основі монітора об’єкта gate.
class BoundedBuffer<T>(int capacity)
{
    private readonly Queue<T> items = new();
    private readonly object gate = new();
    private bool completed;

    public int Capacity { get; } = capacity;
    public int MaxCount { get; private set; }
    public int PutWaits { get; private set; }
    public int TakeWaits { get; private set; }

    public void Put(T item)
    {
        lock (gate)
        {
            while (items.Count == Capacity)   // while, а не if
            {
                PutWaits++;
                Monitor.Wait(gate);           // звільняє gate
            }
            items.Enqueue(item);
            MaxCount = Math.Max(MaxCount, items.Count);
            Monitor.PulseAll(gate);           // будить споживачів
        }
    }

    public bool TryTake(out T item)
    {
        lock (gate)
        {
            while (items.Count == 0 && !completed)
            {
                TakeWaits++;
                Monitor.Wait(gate);
            }
            if (items.Count == 0)             // завершено й порожньо
            {
                item = default!;
                return false;
            }
            item = items.Dequeue();
            Monitor.PulseAll(gate);           // будить виробників
            return true;
        }
    }

    public void Complete()
    {
        lock (gate)
        {
            completed = true;
            Monitor.PulseAll(gate);
        }
    }
}
