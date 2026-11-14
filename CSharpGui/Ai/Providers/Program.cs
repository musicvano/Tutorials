using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OllamaSharp;
using OpenAI;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddUserSecrets<Program>(optional: true);
IConfigurationSection ai = builder.Configuration.GetSection("AI");
string model = ai["Model"] ?? "qwen3:4b-instruct";

// Єдине місце, яке знає конкретного постачальника.
IChatClient provider = ai["Provider"] switch
{
    "OpenAI" => new OpenAIClient(ai["ApiKey"] ?? throw
            new InvalidOperationException("Немає ключа AI:ApiKey"))
        .GetChatClient(model)
        .AsIChatClient(),
    _ => new OllamaApiClient(
        new Uri(ai["Endpoint"] ?? "http://localhost:11434"), model)
};

builder.Services.AddDistributedMemoryCache();
builder.Services.AddChatClient(provider)
    .UseLogging()
    .UseDistributedCache()
    .Use(inner => new TokenLimitChatClient(inner, limit: 20_000));

using IHost host = builder.Build();
IChatClient client = host.Services.GetRequiredService<IChatClient>();

// Перевірка: одне питання до обраного постачальника.
Console.OutputEncoding = System.Text.Encoding.UTF8;
ChatResponse response = await client.GetResponseAsync("Що таке DI?");
Console.WriteLine(response.Text);
