using CommunityToolkit.Mvvm.ComponentModel;

namespace TodoApp.Core;

public partial class TodoItem : ObservableObject
{
    [ObservableProperty]
    public partial string Title { get; set; } = "";

    [ObservableProperty]
    public partial bool IsDone { get; set; }
}
