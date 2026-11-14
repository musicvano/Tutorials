using System.Windows;

namespace BookClient;

public partial class MainWindow : Window
{
    public MainWindow(BookSearchViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
