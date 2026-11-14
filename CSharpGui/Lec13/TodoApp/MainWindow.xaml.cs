using System.Windows;
using TodoApp.Core;

namespace TodoApp;

public partial class MainWindow : Window
{
    public MainWindow(TodoViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        Loaded += (s, e) => viewModel.LoadCommand.Execute(null);
    }
}
