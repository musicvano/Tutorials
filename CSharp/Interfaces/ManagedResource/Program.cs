Console.OutputEncoding = System.Text.Encoding.UTF8;

// using statement: Dispose is called at the end of the block.
using (var session = new DeviceSession("Scanner"))
{
    session.Send("SCAN 300dpi");
}

// Dispose is called even if an exception occurs.
try
{
    using var printer = new DeviceSession("Printer");
    printer.Send("PRINT page 1");
    printer.Send("");                   // invalid command
    printer.Send("PRINT page 2");       // not executed
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Error: empty argument {ex.ParamName}");
}

var closed = new DeviceSession("Plotter");
closed.Dispose();
closed.Dispose();                        // a repeated call is safe
try
{
    closed.Send("PLOT");
}
catch (ObjectDisposedException ex)
{
    Console.WriteLine($"Error: object {ex.ObjectName} is already closed");
}

class DeviceSession : IDisposable
{
    private readonly string device;
    private bool disposed;

    public DeviceSession(string device)
    {
        this.device = device;
        Console.WriteLine($"[{device}] connection opened");
    }

    public void Send(string command)
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        ArgumentException.ThrowIfNullOrWhiteSpace(command);
        Console.WriteLine($"[{device}] → {command}");
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }
        disposed = true;
        Console.WriteLine($"[{device}] connection closed");
    }
}
