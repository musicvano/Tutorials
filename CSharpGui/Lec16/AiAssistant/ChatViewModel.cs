using System.Collections.ObjectModel;
using System.Net.Http;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.AI;

namespace AiAssistant;

public partial class MessageItem(string author) : ObservableObject
{
    public string Author { get; } = author;

    [ObservableProperty]
    public partial string Text { get; set; } = "";
}

public partial class ChatViewModel(IChatClient client)
    : ObservableObject
{
    private readonly List<ChatMessage> history =
    [
        new(ChatRole.System,
            "Ти – помічник студента. Відповідай українською, стисло.")
    ];

    public ObservableCollection<MessageItem> Messages { get; } = [];
    public string[] Models { get; } =
        ["qwen3:4b-instruct", "llama3.2:3b"];

    [ObservableProperty]
    public partial string SelectedModel { get; set; } =
        "qwen3:4b-instruct";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SendCommand))]
    public partial string Prompt { get; set; } = "";

    [ObservableProperty]
    public partial string Status { get; set; } = "Ready";

    private bool CanSend() => !string.IsNullOrWhiteSpace(Prompt);

    [RelayCommand(CanExecute = nameof(CanSend),
        IncludeCancelCommand = true)]
    private async Task SendAsync(CancellationToken token)
    {
        string text = Prompt.Trim();
        Prompt = "";
        Messages.Add(new MessageItem("You") { Text = text });
        var answer = new MessageItem("Assistant");
        Messages.Add(answer);
        history.Add(new ChatMessage(ChatRole.User, text));

        var options = new ChatOptions { ModelId = SelectedModel };
        List<ChatResponseUpdate> updates = [];
        Status = "Generating...";
        try
        {
            var stream = client.GetStreamingResponseAsync(
                history, options, token);
            await foreach (ChatResponseUpdate update in stream)
            {
                answer.Text += update.Text;   // UI-потік: прив’язка
                updates.Add(update);
            }
            history.AddMessages(updates);
            UsageDetails? usage = updates.ToChatResponse().Usage;
            Status = $"Tokens: {usage?.TotalTokenCount ?? 0}";
        }
        catch (OperationCanceledException)
        {
            answer.Text += " [cancelled]";
            history.RemoveAt(history.Count - 1);
            Status = "Cancelled";
        }
        catch (HttpRequestException ex)
        {
            answer.Text = "Model is not available.";
            history.RemoveAt(history.Count - 1);
            Status = ex.Message;
        }
    }
}
