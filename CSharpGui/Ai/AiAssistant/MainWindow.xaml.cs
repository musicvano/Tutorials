using System.Windows;
using OllamaSharp;

namespace AiAssistant;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var ollama = new OllamaApiClient(
            new Uri("http://localhost:11434"));
        DataContext = new ChatViewModel(ollama);
    }
}
