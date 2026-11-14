using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using OllamaSharp;

Console.InputEncoding = System.Text.Encoding.UTF8;
Console.OutputEncoding = System.Text.Encoding.UTF8;

using ILoggerFactory loggers = LoggerFactory.Create(b => b
    .AddSimpleConsole(o => o.SingleLine = true)
    .SetMinimumLevel(LogLevel.Debug));

IChatClient client = new ChatClientBuilder(
        new OllamaApiClient(
            new Uri("http://localhost:11434"), "qwen3:4b-instruct"))
    .UseFunctionInvocation(loggers, f =>
        f.MaximumIterationsPerRequest = 5)
    .UseLogging(loggers)
    .Build();

var library = new LibraryTools();
var options = new ChatOptions
{
    Temperature = 0f,
    Tools =
    [
        AIFunctionFactory.Create(library.SearchBooks),
        AIFunctionFactory.Create(library.ReserveBook)
    ]
};
List<ChatMessage> history =
[
    new(ChatRole.System,
        "Ти – помічник бібліотеки. Про книги відповідай лише " +
        "за даними функцій, нічого не вигадуй. " +
        "Відповідай українською.")
];

while (true)
{
    Console.Write("\nЧитач: ");
    string? text = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(text)) break;
    history.Add(new ChatMessage(ChatRole.User, text));

    ChatResponse response =
        await client.GetResponseAsync(history, options);
    history.AddMessages(response);
    Console.WriteLine($"Бібліотека: {response.Text}");
}
