using TodoApp.Core;

namespace TodoApp.Tests;

public class TodoViewModelTests
{
    // Фейкове сховище: без файлів, дані в пам’яті.
    private class FakeStore : ITodoStore
    {
        public List<TodoItem> Saved { get; } = [];

        public Task<List<TodoItem>> LoadAsync(CancellationToken t) =>
            Task.FromResult(new List<TodoItem>
            {
                new() { Title = "Buy milk" },
                new() { Title = "Read lecture 13", IsDone = true }
            });

        public Task SaveAsync(IEnumerable<TodoItem> items,
            CancellationToken t)
        {
            Saved.AddRange(items);
            return Task.CompletedTask;
        }
    }

    private readonly FakeStore store = new();

    [Fact]
    public void AddCommand_AddsItemAndClearsTitle()
    {
        var vm = new TodoViewModel(store) { NewTitle = " Walk " };
        var changed = new List<string?>();
        vm.PropertyChanged += (s, e) => changed.Add(e.PropertyName);

        vm.AddCommand.Execute(null);

        TodoItem item = Assert.Single(vm.Items);
        Assert.Equal("Walk", item.Title);
        Assert.Equal("", vm.NewTitle);
        Assert.Contains(nameof(TodoViewModel.NewTitle), changed);
        Assert.False(vm.AddCommand.CanExecute(null));
    }

    // Три тести нижче описано в лекції лише текстом.
    [Fact]
    public void AddCommand_EmptyTitle_CannotExecute()
    {
        var vm = new TodoViewModel(store) { NewTitle = "   " };

        Assert.False(vm.AddCommand.CanExecute(null));
    }

    [Fact]
    public async Task LoadAndRemove_UpdateItems()
    {
        var vm = new TodoViewModel(store);

        await vm.LoadCommand.ExecuteAsync(null);
        vm.RemoveCommand.Execute(vm.Items[0]);

        TodoItem item = Assert.Single(vm.Items);
        Assert.Equal("Read lecture 13", item.Title);
    }

    [Fact]
    public async Task SaveCommand_PassesItemsToStore()
    {
        var vm = new TodoViewModel(store) { NewTitle = "Walk" };
        vm.AddCommand.Execute(null);

        await vm.SaveCommand.ExecuteAsync(null);

        TodoItem saved = Assert.Single(store.Saved);
        Assert.Equal("Walk", saved.Title);
    }
}
