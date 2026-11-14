using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Logging;
using TodoClient;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.SetMinimumLevel(LogLevel.Warning);
builder.Services.AddHttpClient<TodoApiClient>(client =>
    {
        client.BaseAddress = new Uri(
            builder.Configuration["TodoApi:BaseAddress"]
            ?? "http://localhost:5080/");
    })
    .AddStandardResilienceHandler(options =>
        options.Retry.DisableForUnsafeHttpMethods());

using IHost host = builder.Build();
var api = host.Services.GetRequiredService<TodoApiClient>();

try
{
    var milk = await api.CreateAsync(new("Купити молоко",
        Priority.High, new DateOnly(2026, 9, 20)));
    Console.WriteLine($"Створено: #{milk.Id} {milk.Title}");
    await api.CreateAsync(new("Підготувати звіт"));

    try
    {
        await api.CreateAsync(new("Ок"));
    }
    catch (HttpRequestException ex)
        when (ex.StatusCode == HttpStatusCode.BadRequest)
    {
        Console.WriteLine(ex.Message);
    }

    var page = await api.GetPageAsync(pageSize: 5);
    Console.WriteLine($"Усього справ: {page.TotalCount}");
    foreach (var t in page.Items)
    {
        string mark = t.IsDone ? "x" : " ";
        Console.WriteLine($"  [{mark}] #{t.Id} {t.Title,-20} " +
            $"{t.Priority,-6} {t.DueDate:dd.MM.yyyy}");
    }
    var missing = await api.FindAsync(99);
    Console.WriteLine(
        $"Справа #99: {missing?.Title ?? "не знайдено"}");
}
catch (HttpRequestException ex)
{
    Console.Error.WriteLine(ex.StatusCode is null
        ? $"Сервер недоступний ({ex.HttpRequestError})."
        : ex.Message);
    return 1;
}
return 0;
