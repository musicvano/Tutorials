using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TodoClient;

// Типізований клієнт: HttpClient приходить від IHttpClientFactory.
public class TodoApiClient(HttpClient http)
{
    private static readonly JsonSerializerOptions Json =
        new(JsonSerializerOptions.Web)
        {
            Converters = { new JsonStringEnumConverter(
                JsonNamingPolicy.CamelCase) }
        };

    public async Task<PagedResult<TodoItem>> GetPageAsync(
        bool? done = null, int page = 1, int pageSize = 10,
        CancellationToken ct = default)
    {
        string url = $"api/todos?page={page}&pageSize={pageSize}";
        if (done != null)
            url += done.Value ? "&done=true" : "&done=false";
        return (await http.GetFromJsonAsync<PagedResult<TodoItem>>(
            url, Json, ct))!;
    }

    public async Task<TodoItem?> FindAsync(int id,
        CancellationToken ct = default)
    {
        using var response =
            await http.GetAsync($"api/todos/{id}", ct);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;                         // 404 – не помилка
        await EnsureSuccessAsync(response, ct);
        return await response.Content.ReadFromJsonAsync<TodoItem>(
            Json, ct);
    }

    public async Task<TodoItem> CreateAsync(TodoCreateDto dto,
        CancellationToken ct = default)
    {
        using var response = await http.PostAsJsonAsync(
            "api/todos", dto, Json, ct);
        await EnsureSuccessAsync(response, ct);
        return (await response.Content.ReadFromJsonAsync<TodoItem>(
            Json, ct))!;
    }

    // Як EnsureSuccessStatusCode, але з текстом ProblemDetails.
    private static async Task EnsureSuccessAsync(
        HttpResponseMessage response, CancellationToken ct)
    {
        if (response.IsSuccessStatusCode) return;
        string message = $"HTTP {(int)response.StatusCode}";
        if (response.Content.Headers.ContentType?.MediaType
            == "application/problem+json")
        {
            var problem = await response.Content
                .ReadFromJsonAsync<ApiProblem>(Json, ct);
            message += $": {problem?.Title}";
            foreach (string error in problem?.Errors?.Values
                         .SelectMany(e => e) ?? [])
                message += $"{Environment.NewLine}  {error}";
        }
        throw new HttpRequestException(message, null,
            response.StatusCode);
    }
}
