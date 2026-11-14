using Notes.Views;

namespace Notes;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        // Сторінка нотатки поза TabBar: глобальний маршрут.
        Routing.RegisterRoute("note", typeof(NotePage));
    }
}
