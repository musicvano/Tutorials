using System.Windows;
using System.Windows.Input;

namespace EventDemo;

public partial class MainWindow : Window
{
    private readonly FrameworkElement[] elements;
    private readonly MouseButtonEventHandler previewHandler;
    private readonly MouseButtonEventHandler bubbleHandler;

    public MainWindow()
    {
        InitializeComponent();
        elements = [this, border, panel, button, icon];
        previewHandler = OnPreviewMouseDown;
        bubbleHandler = OnMouseDown;
        Subscribe(handledEventsToo: false);
    }

    private void Subscribe(bool handledEventsToo)
    {
        foreach (FrameworkElement element in elements)
        {
            element.RemoveHandler(PreviewMouseDownEvent,
                previewHandler);
            element.RemoveHandler(MouseDownEvent, bubbleHandler);
            element.AddHandler(PreviewMouseDownEvent, previewHandler,
                handledEventsToo);
            element.AddHandler(MouseDownEvent, bubbleHandler,
                handledEventsToo);
        }
    }

    private void OnPreviewMouseDown(object sender,
        MouseButtonEventArgs e)
    {
        if (!IsInDemo(e))
        {
            return;
        }
        if (sender == this && logList.Items.Count > 0)
        {
            logList.Items.Add("");          // нове натискання
        }
        Log("Preview", sender, e);
        if (sender == panel && stopBox.IsChecked == true)
        {
            e.Handled = true;
        }
    }

    private void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (IsInDemo(e))
        {
            Log("MouseDown", sender, e);
        }
    }

    // Лише натискання в рамці border, а не на прапорцях чи списку.
    private bool IsInDemo(RoutedEventArgs e) =>
        e.OriginalSource is DependencyObject source
        && (source == border || border.IsAncestorOf(source));

    private void Log(string name, object sender, RoutedEventArgs e)
    {
        string element = ((FrameworkElement)sender).Name;
        string source = (e.OriginalSource as FrameworkElement)?.Name
            ?? e.OriginalSource.GetType().Name;
        string handled = e.Handled ? "handled" : "";
        logList.Items.Add(
            $"{name,-9} {element,-6} from {source,-6} {handled}");
    }

    private void HandledTooBox_Click(object sender,
        RoutedEventArgs e) =>
        Subscribe(handledTooBox.IsChecked == true);
}
