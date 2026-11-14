Console.OutputEncoding = System.Text.Encoding.UTF8;

string names = string.Join(", ", Enum.GetNames<Season>());
Console.WriteLine($"Seasons: {names}");
while (true)
{
    Console.Write("Season (empty line to exit): ");
    string? input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
    {
        break;
    }

    // TryParse also accepts numbers, so we additionally check IsDefined.
    if (Enum.TryParse(input.Trim(), true, out Season season)
        && Enum.IsDefined(season))
    {
        Console.WriteLine(
            $"  {season} ({(int)season}): {Describe(season)}");
    }
    else
    {
        Console.WriteLine($"  “{input}” is an unknown season");
    }
}

static string Describe(Season season) => season switch
{
    Season.Winter => "December – February",
    Season.Spring => "March – May",
    Season.Summer => "June – August",
    Season.Autumn => "September – November",
    _ => throw new ArgumentOutOfRangeException(nameof(season)),
};

enum Season
{
    Winter = 1,
    Spring,
    Summer,
    Autumn,
}
