using System.Text.Json;
using System.Text.Json.Serialization;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();   // помилки за RFC 9457
builder.Services.AddValidation();       // перевірка DTO (.NET 10)
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(   // "high", а не 2
        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)));
builder.Services.AddSingleton<ITodoRepository,
    InMemoryTodoRepository>();

var app = builder.Build();

app.UseExceptionHandler(new ExceptionHandlerOptions
{
    // Некоректний JSON у тілі – 400, решта винятків – 500.
    StatusCodeSelector = ex => ex is BadHttpRequestException bad
        ? bad.StatusCode : StatusCodes.Status500InternalServerError
});
app.UseStatusCodePages();     // порожні 404, 405 -> ProblemDetails
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();         // документ /openapi/v1.json
}
app.UseHttpsRedirection();

app.MapTodoEndpoints();
app.Run();
