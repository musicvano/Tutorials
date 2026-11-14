Console.OutputEncoding = System.Text.Encoding.UTF8;

// 23 products generated with LINQ.
List<string> catalog = Enumerable.Range(1, 23)
    .Select(i => $"Product {i:D2}")
    .ToList();
const int PageSize = 5;
int pageCount = (int)Math.Ceiling(catalog.Count / (double)PageSize);

Console.WriteLine($"Products: {catalog.Count}, pages: {pageCount}");
foreach (int page in new[] { 1, 5, 6 })
{
    string items = string.Join(", ", GetPage(page));
    Console.WriteLine($"Page {page}: {items}");
}

// Chunk splits a sequence into arrays of the same size.
foreach (var (index, chunk) in catalog.Chunk(PageSize).Index())
{
    Console.WriteLine(
        $"  [{index + 1}] {chunk[0]} … {chunk[^1]} ({chunk.Length})");
}

IEnumerable<string> GetPage(int page)
{
    if (page < 1 || page > pageCount)
    {
        return [$"no page {page}"];
    }
    return catalog.Skip((page - 1) * PageSize).Take(PageSize);
}
