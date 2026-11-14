using System.Windows;
using System.Windows.Controls;

namespace UnitConverter;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        foreach (Category category in Category.All)
        {
            categoryTabs.Items.Add(new TabItem
            {
                Header = category.Name,
                Content = new ConverterPanel(category)
            });
        }
        categoryTabs.SelectedIndex = 0;
    }
}
