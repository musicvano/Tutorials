using Microsoft.Extensions.AI;

// Проміжний клієнт: рахує токени й зупиняє запити понад ліміт.
public sealed class TokenLimitChatClient(
    IChatClient inner, long limit) : DelegatingChatClient(inner)
{
    public long UsedTokens { get; private set; }

    public override async Task<ChatResponse> GetResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ThrowIfExhausted();
        ChatResponse response = await base.GetResponseAsync(
            messages, options, cancellationToken);
        UsedTokens += response.Usage?.TotalTokenCount ?? 0;
        return response;
    }

    private void ThrowIfExhausted()
    {
        if (UsedTokens >= limit)
            throw new InvalidOperationException(
                $"Ліміт {limit} токенів вичерпано.");
    }
}
