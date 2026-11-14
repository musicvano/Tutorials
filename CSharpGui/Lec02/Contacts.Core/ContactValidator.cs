using System.Text.RegularExpressions;

namespace Contacts.Core;

public static partial class ContactValidator
{
    [GeneratedRegex(@"^(\+38)?0\d{2}[ -]?\d{3}([ -]?\d{2}){2}$")]
    private static partial Regex Phone { get; }

    public static bool IsValidPhone(string text) =>
        Phone.IsMatch(text);
}
