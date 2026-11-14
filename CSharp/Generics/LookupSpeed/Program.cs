using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;

const int Size = 1_000_000;
const int Lookups = 2_000;

List<int> list = new(Size);
for (int i = 0; i < Size; i++)
{
    list.Add(i * 2);                       // even numbers only
}
HashSet<int> set = [.. list];

Random random = new(42);
int[] queries = new int[Lookups];
for (int i = 0; i < Lookups; i++)
{
    queries[i] = random.Next(2 * Size);    // half are odd
}

Stopwatch watch = Stopwatch.StartNew();
int foundInList = 0;
foreach (int q in queries)
{
    if (list.Contains(q)) foundInList++;
}
double listMs = watch.Elapsed.TotalMilliseconds;

watch.Restart();
int foundInSet = 0;
foreach (int q in queries)
{
    if (set.Contains(q)) foundInSet++;
}
double setMs = watch.Elapsed.TotalMilliseconds;

Console.WriteLine($"Lookups: {Lookups:N0} among {Size:N0} elements");
Console.WriteLine($"List<int>:    {foundInList}, {listMs,8:F2} ms");
Console.WriteLine($"HashSet<int>: {foundInSet}, {setMs,8:F2} ms");
Console.WriteLine($"HashSet is {listMs / setMs:F0} times faster");
