using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("Phone number (a blank line ends input):");
string? input = Console.ReadLine();

while (!string.IsNullOrWhiteSpace(input))
{
    // Keep only digits.
    var digits = new StringBuilder();
    foreach (char c in input)
    {
        if (char.IsDigit(c))
        {
            digits.Append(c);
        }
    }
    string d = digits.ToString();

    // Normalize to 12 digits, 380XXXXXXXXX.
    d = d.Length switch
    {
        10 when d.StartsWith('0') => "38" + d,
        11 when d.StartsWith("80") => "3" + d,
        _ => d,
    };

    if (d.Length == 12 && d.StartsWith("380"))
    {
        string code = d.Substring(3, 2);
        string formatted =
            $"+380 {code} {d[5..8]} {d[8..10]} {d[10..]}";
        string masked = $"+380 {code} *** ** {d[10..]}";
        Console.WriteLine($"  {formatted}   (masked: {masked})");
    }
    else
    {
        Console.WriteLine($"  “{input.Trim()}” — invalid number");
    }
    input = Console.ReadLine();
}
