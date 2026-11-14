using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Notes.Models;
using Notes.Services;

namespace Notes.ViewModels;

public partial class NoteViewModel(INoteStore store)
    : ObservableObject, IQueryAttributable
{
    private Note note = new();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial string Text { get; set; } = "";

    // Отримує параметри навігації: нотатку, передану з NotesPage.
    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        if (query.TryGetValue("note", out object? value)
            && value is Note existing)
        {
            note = existing;
            Text = existing.Text;
        }
    }

    private bool CanSave() => !string.IsNullOrWhiteSpace(Text);

    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsync()
    {
        note.Text = Text.Trim();
        await store.SaveAsync(note);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        store.Delete(note);
        await Shell.Current.GoToAsync("..");
    }
}
