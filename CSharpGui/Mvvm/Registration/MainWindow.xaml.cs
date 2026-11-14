using System.Windows;

namespace Registration;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        var viewModel = (RegistrationViewModel)DataContext;
        MessageBox.Show(this, $"Saved: {viewModel.Email}, "
            + $"{viewModel.Age}", Title);
    }
}
