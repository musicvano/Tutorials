using System.Windows;

namespace Styles;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void DarkThemeBox_Click(object sender, RoutedEventArgs e)
    {
        string name =
            darkThemeBox.IsChecked == true ? "Dark" : "Light";
        var theme = new ResourceDictionary
        {
            Source = new Uri($"Themes/{name}.xaml", UriKind.Relative)
        };
        Application.Current.Resources.MergedDictionaries[0] = theme;
    }
}
