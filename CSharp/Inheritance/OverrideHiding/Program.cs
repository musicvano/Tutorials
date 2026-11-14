Console.OutputEncoding = System.Text.Encoding.UTF8;

Logger viaBase = new TimestampLogger();
TimestampLogger viaDerived = new TimestampLogger();

viaBase.Write("override: via Logger");
viaDerived.Write("override: via TimestampLogger");

viaBase.Level("new: via Logger");
viaDerived.Level("new: via TimestampLogger");

class Logger
{
    public virtual void Write(string message) =>
        Console.WriteLine($"[Logger] {message}");

    public void Level(string message) =>
        Console.WriteLine($"[INFO] {message}");
}

class TimestampLogger : Logger
{
    // Overriding: called according to the actual type of the object.
    public override void Write(string message) =>
        base.Write($"12:00:00 {message}");

    // Hiding: called according to the type of the variable.
    public new void Level(string message) =>
        Console.WriteLine($"[INFO 12:00:00] {message}");
}
