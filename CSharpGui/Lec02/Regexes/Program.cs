using System.Text.RegularExpressions;

var date = new Regex(@"^(?<day>\d{2})\.(?<month>\d{2})\.(?<year>\d{4})$");
Match m = date.Match("17.09.2026");
Console.WriteLine(m.Groups["year"].Value);
