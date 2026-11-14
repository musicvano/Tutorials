namespace Common.Input;

/// <summary>Safe reading of data from the console.</summary>
public static class ConsoleInput
{
    public const string DefaultError =
        "Invalid value, try again.";

    public static int ReadInt(string prompt, int min = int.MinValue,
        int max = int.MaxValue)
    {
        while (true)
        {
            string text = ReadText(prompt);
            if (int.TryParse(text, out int value)
                && value >= min && value <= max)
            {
                return value;
            }
            Console.WriteLine($"{DefaultError} ({min}…{max})");
        }
    }

    public static double ReadDouble(string prompt)
    {
        while (true)
        {
            string text = ReadText(prompt).Replace('.', ',');
            if (double.TryParse(text, out double value))
            {
                return value;
            }
            Console.WriteLine(DefaultError);
        }
    }

    public static int ReadChoice(string prompt,
        params string[] options)
    {
        for (int i = 0; i < options.Length; i++)
        {
            Console.WriteLine($"  {i + 1}. {options[i]}");
        }
        return ReadInt(prompt, 1, options.Length) - 1;
    }

    // Internal method: library users do not need it.
    internal static string ReadText(string prompt)
    {
        Console.Write(prompt);
        return (Console.ReadLine() ?? throw new EndOfStreamException(
            "Input ended")).Trim();
    }
}
