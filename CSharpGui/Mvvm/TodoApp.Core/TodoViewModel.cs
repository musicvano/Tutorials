using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TodoApp.Core;

public partial class TodoViewModel(ITodoStore store)
    : ObservableObject
{
    public ObservableCollection<TodoItem> Items { get; } = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddCommand))]
    public partial string NewTitle { get; set; } = "";

    [RelayCommand(CanExecute = nameof(CanAdd))]
    private void Add()
    {
        Items.Add(new TodoItem { Title = NewTitle.Trim() });
        NewTitle = "";
    }

    private bool CanAdd() => !string.IsNullOrWhiteSpace(NewTitle);

    [RelayCommand]
    private void Remove(TodoItem item) => Items.Remove(item);

    [RelayCommand]
    private async Task LoadAsync(CancellationToken token)
    {
        Items.Clear();
        foreach (TodoItem item in await store.LoadAsync(token))
        {
            Items.Add(item);
        }
    }

    [RelayCommand]
    private Task SaveAsync(CancellationToken token) =>
        store.SaveAsync(Items, token);
}
