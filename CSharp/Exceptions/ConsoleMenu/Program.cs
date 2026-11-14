Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

int[] numbers = [];
int failures = 0;

Console.WriteLine("Commands: add N, avg, get I, div A B, q");
while (true)
{
    Console.Write("> ");
    string[] cmd = (Console.ReadLine() ?? "q").Split(' ',
        StringSplitOptions.RemoveEmptyEntries);
    if (cmd.Length == 0)
    {
        continue;
    }
    if (cmd[0] == "q")
    {
        break;
    }

    try
    {
        Execute(cmd, ref numbers);
    }
    catch (Exception ex) when (ex is FormatException
        or IndexOutOfRangeException or DivideByZeroException
        or InvalidOperationException)
    {
        failures++;
        Console.Error.WriteLine($"  Error: {ex.Message}");
    }
}

Console.WriteLine($"Failed commands: {failures}");
Environment.ExitCode = failures > 0 ? 1 : 0;

static void Execute(string[] cmd, ref int[] numbers)
{
    switch (cmd[0])
    {
        case "add":
            int value = int.Parse(cmd[1]);  // parse first
            Array.Resize(ref numbers, numbers.Length + 1);
            numbers[^1] = value;
            Console.WriteLine($"  Elements: {numbers.Length}");
            break;
        case "avg":
            if (numbers.Length == 0)
            {
                throw new InvalidOperationException(
                    "The list is empty.");
            }
            double sum = 0;
            foreach (int n in numbers)
            {
                sum += n;
            }
            double avg = sum / numbers.Length;
            Console.WriteLine($"  Average: {avg:F2}");
            break;
        case "get":
            int index = int.Parse(cmd[1]);
            Console.WriteLine($"  [{index}] = {numbers[index]}");
            break;
        case "div":
            int a = int.Parse(cmd[1]), b = int.Parse(cmd[2]);
            Console.WriteLine($"  {a / b}");
            break;
        default:
            throw new FormatException(
                $"Unknown command “{cmd[0]}”.");
    }
}
