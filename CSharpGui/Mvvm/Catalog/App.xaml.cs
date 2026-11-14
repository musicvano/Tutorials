using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace Catalog;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // Формат чисел і дат у прив’язках – за регіоном Windows.
        FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(
                CultureInfo.CurrentCulture.IetfLanguageTag)));
        base.OnStartup(e);
    }
}
