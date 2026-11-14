using System.Runtime.InteropServices;

namespace ProgramInfo;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        string clr = RuntimeInformation.FrameworkDescription;
        string os = RuntimeInformation.OSDescription;

        Console.WriteLine($"Version: {Environment.Version}");
        Console.WriteLine($"Runtime: {clr}");
        Console.WriteLine($"OS: {os}");
        Console.WriteLine($"Arguments: {args.Length}");

        for (int i = 0; i < args.Length; i++)
        {
            Console.WriteLine($"  args[{i}] = {args[i]}");
        }
    }
}
