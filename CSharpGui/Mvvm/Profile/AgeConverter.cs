using System.Globalization;
using System.Windows.Data;

namespace Profile;

// DateTime? (дата народження) → рядок «19 years».
public class AgeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType,
        object? parameter, CultureInfo culture)
    {
        if (value is not DateTime birth)
        {
            return null;   // → TargetNullValue
        }
        DateTime today = DateTime.Today;
        int age = today.Year - birth.Year;
        if (birth.Date > today.AddYears(-age))
        {
            age--;
        }
        return $"{age} years";
    }

    public object? ConvertBack(object? value, Type targetType,
        object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
