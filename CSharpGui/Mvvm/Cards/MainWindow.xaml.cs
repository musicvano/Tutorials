using System.Windows;

namespace Cards;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new List<Contact>
        {
            new("Petro Bondar", "+380 67 111 2233", null),
            new("Iryna Melnyk", "+380 50 444 5566", "Brussels Soft"),
            new("Andrii Koval", "+380 63 777 8899", null),
            new("Oksana Lysenko", "+380 97 222 3344", "Rudna Trans"),
            new("Denys Shevchuk", "+380 66 555 6677", null)
        };
    }
}
