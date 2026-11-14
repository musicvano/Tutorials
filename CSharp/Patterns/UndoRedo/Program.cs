Console.OutputEncoding = System.Text.Encoding.UTF8;

TextDocument document = new();
CommandHistory history = new();

history.Execute(new AppendCommand(document, "Hello"));
history.Execute(new AppendCommand(document, ", world"));
history.Execute(new AppendCommand(document, "!!!"));
Show("3 commands");

history.Undo();
Show("Undo");
history.Undo();
Show("Undo");
history.Redo();
Show("Redo");

history.Execute(new DeleteLastCommand(document, 2));
Show("Delete 2");
Console.WriteLine($"Redo possible: {history.CanRedo}");  // false
history.Undo();
Show("Undo");

void Show(string action) =>
    Console.WriteLine($"{action,-10} → “{document.Text}”");

class TextDocument
{
    public string Text { get; set; } = "";
}

interface ICommand
{
    void Execute();
    void Undo();
}

class AppendCommand(TextDocument document, string text) : ICommand
{
    public void Execute() => document.Text += text;
    public void Undo() =>
        document.Text = document.Text[..^text.Length];
}

class DeleteLastCommand(TextDocument document, int count) : ICommand
{
    private string removed = "";

    public void Execute()
    {
        int n = Math.Min(count, document.Text.Length);
        removed = document.Text[^n..];     // remember for Undo
        document.Text = document.Text[..^n];
    }

    public void Undo() => document.Text += removed;
}

// The invoker: executes commands and keeps the history.
class CommandHistory
{
    private readonly Stack<ICommand> undo = new();
    private readonly Stack<ICommand> redo = new();

    public bool CanRedo => redo.Count > 0;

    public void Execute(ICommand command)
    {
        command.Execute();
        undo.Push(command);
        redo.Clear();                   // a new action clears “forward”
    }

    public void Undo()
    {
        if (undo.TryPop(out ICommand? command))
        {
            command.Undo();
            redo.Push(command);
        }
    }

    public void Redo()
    {
        if (redo.TryPop(out ICommand? command))
        {
            command.Execute();
            undo.Push(command);
        }
    }
}
