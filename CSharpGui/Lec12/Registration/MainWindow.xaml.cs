using System.Windows;
using System.Windows.Controls;

namespace Registration;

public partial class MainWindow : Window
{
    private const decimal BasePrice = 1000m, WorkshopPrice = 500m;

    public MainWindow()
    {
        InitializeComponent();
        datePicker.DisplayDateStart = DateTime.Today;
    }

    private void Register_Click(object sender, RoutedEventArgs e)
    {
        var errors = new List<string>();
        string name = nameBox.Text.Trim();
        string email = emailBox.Text.Trim();
        if (name.Length < 2)
        {
            errors.Add("Enter your name (at least 2 letters).");
        }
        if (!email.Contains('@') || email.EndsWith('@'))
        {
            errors.Add("Enter a valid email.");
        }
        if (datePicker.SelectedDate is not DateTime date
            || date < DateTime.Today)
        {
            errors.Add("Choose a date from today on.");
            date = default;
        }
        if (errors.Count > 0)
        {
            messageText.Text = string.Join("\n", errors);
            return;
        }

        var track = (ComboBoxItem)trackBox.SelectedItem;
        decimal price = BasePrice
            + (workshopBox.IsChecked == true ? WorkshopPrice : 0);
        messageText.Text = $"Registered: {name}, {track.Content}, "
            + $"{date:d}, total {price:N0} UAH.";
    }

    private void Cancel_Click(object sender, RoutedEventArgs e) =>
        Close();
}
