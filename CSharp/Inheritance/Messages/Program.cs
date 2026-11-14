Console.OutputEncoding = System.Text.Encoding.UTF8;

(string Kind, string To, string Text)[] requests =
[
    ("email", "olena@knu.edu.ua", "Tomorrow's schedule has been updated"),
    ("sms", "+380671234567", "Confirmation code: 4821"),
    ("email", "olena.knu.edu.ua", "A letter with an address error"),
    ("sms", "+38067", "Short message"),
    ("sms", "+380501112233", new string('★', 75)),
];

decimal cost = 0;
foreach (var (kind, to, text) in requests)
{
    try
    {
        Message m = kind == "email"
            ? new Email(to, text)
            : new Sms(to, text);
        cost += m.Cost;
        Console.WriteLine($"{m.Send()}  ({m.Cost:N2} UAH)");
    }
    catch (InvalidRecipientException ex)
    {
        Console.WriteLine(
            $"Rejected: {ex.Message} “{ex.Recipient}”");
    }
}
Console.WriteLine($"Sending cost: {cost:N2} UAH");

class Message
{
    protected Message(string to, string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        To = to;
        Text = text;
    }

    public string To { get; }
    public string Text { get; }
    public virtual decimal Cost => 0m;

    public virtual string Send() => $"→ {To}: {Text}";
}

class Email : Message
{
    public Email(string to, string text) : base(to, text)
    {
        int at = to.IndexOf('@');
        if (at <= 0 || to.IndexOf('.', at) < 0)
        {
            throw new InvalidRecipientException(
                "invalid e-mail address", to);
        }
    }

    public override string Send() => "E-mail " + base.Send();
}

sealed class Sms : Message
{
    private const int PartLength = 70;   // Unicode text: 70 characters
    private const decimal PricePerPart = 0.85m;

    public Sms(string to, string text) : base(to, text)
    {
        if (to.Length != 13 || !to.StartsWith("+380"))
        {
            throw new InvalidRecipientException(
                "invalid number", to);
        }
    }

    public int Parts => (Text.Length + PartLength - 1) / PartLength;
    public override decimal Cost => Parts * PricePerPart;

    public override string Send()
    {
        string preview = Text.Length > 20 ? Text[..20] + "…" : Text;
        return $"SMS → {To}: {preview} [{Parts} part(s)]";
    }
}

class InvalidRecipientException : Exception
{
    public InvalidRecipientException(string message, string recipient)
        : base(message)
    {
        Recipient = recipient;
    }

    public string Recipient { get; }
}
