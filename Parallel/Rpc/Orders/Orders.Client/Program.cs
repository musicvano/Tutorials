using Grpc.Core;
using Grpc.Net.Client;
using Orders;

Console.OutputEncoding = System.Text.Encoding.UTF8;
using GrpcChannel channel = GrpcChannel.ForAddress(
    "http://localhost:5001");
OrderService.OrderServiceClient client = new(channel);
Metadata headers = new() { { "client-name", "lecture-demo" } };

// 1. Унарні виклики: успіх і помилка з кодом статусу.
foreach (int id in new[] { 2, 42 })
{
    try
    {
        Print(await client.GetOrderAsync(new() { Id = id }, headers));
    }
    catch (RpcException ex)
    {
        Fail(ex);
    }
}

// 2. Серверний потік без дедлайну і з дедлайном 1 с.
await ListAsync("Від 1000 грн:", new() { MinTotal = 1000 }, null);
DateTime deadline = DateTime.UtcNow.AddSeconds(1);
await ListAsync("Усі, дедлайн 1 с:", new(), deadline);

async Task ListAsync(string title, ListRequest req, DateTime? until)
{
    Console.WriteLine(title);
    try
    {
        using var call = client.ListOrders(req, headers, until);
        await foreach (var item in call.ResponseStream.ReadAllAsync())
        {
            Print(item);
        }
    }
    catch (RpcException ex)
    {
        Fail(ex);
    }
}

static void Print(Order o)
{
    double total = o.Lines.Sum(line => line.Quantity * line.Price);
    Console.WriteLine($"  №{o.Id} {o.Customer,-14} " +
        $"{o.Lines.Count} поз. {total,9:N2} грн  {o.Status}");
}

static void Fail(RpcException ex) =>
    Console.WriteLine($"помилка {ex.StatusCode} {ex.Status.Detail}");
