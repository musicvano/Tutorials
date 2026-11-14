using Microsoft.Extensions.AI;
using OllamaSharp;

Console.InputEncoding = System.Text.Encoding.UTF8;
Console.OutputEncoding = System.Text.Encoding.UTF8;

const int MaxHistory = 10;   // повідомлень, крім системного

IChatClient client = new OllamaApiClient(
    new Uri("http://localhost:11434"), "qwen3:4b-instruct");

List<ChatMessage> history =
[
    new(ChatRole.System,
        "Ти – помічник студента з програмування на C#. " +
        "Відповідай українською, коротко, 3–5 речень. " +
        "Якщо не знаєш відповіді, так і скажи.")
];
var options = new ChatOptions
{
    Temperature = 0.3f,
    MaxOutputTokens = 500
};
long totalTokens = 0;

while (true)
{
    Console.Write("\nВи: ");
    string? question = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(question) || question == "/exit")
        break;
    history.Add(new ChatMessage(ChatRole.User, question));

    Console.Write("Помічник: ");
    List<ChatResponseUpdate> updates = [];
    try
    {
        await foreach (ChatResponseUpdate update in
            client.GetStreamingResponseAsync(history, options))
        {
            Console.Write(update.Text);
            updates.Add(update);
        }
    }
    catch (HttpRequestException ex)
    {
        Console.Error.WriteLine($"Немає зв’язку: {ex.Message}");
        history.RemoveAt(history.Count - 1);
        continue;
    }

    ChatResponse response = updates.ToChatResponse();
    history.AddMessages(response);

    UsageDetails? usage = response.Usage;
    totalTokens += usage?.TotalTokenCount ?? 0;
    Console.WriteLine();
    Console.WriteLine($"[токени: запит {usage?.InputTokenCount}, " +
        $"відповідь {usage?.OutputTokenCount}, " +
        $"за сеанс {totalTokens}]");

    // Найстаріші пари «питання – відповідь» видаляються.
    while (history.Count > MaxHistory + 1)
        history.RemoveRange(1, 2);
}
