using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Notes.Models;
using Notes.Services;

namespace Notes.ViewModels;

public partial class NotesViewModel(INoteStore store)
    : ObservableObject
{
    public ObservableCollection<Note> Notes { get; } = [];

    [ObservableProperty]
    public partial Note? SelectedNote { get; set; }

    [RelayCommand]
    private async Task LoadAsync()
    {
        Notes.Clear();
        foreach (Note note in await store.LoadAllAsync())
        {
            Notes.Add(note);
        }
    }

    [RelayCommand]
    private Task AddAsync() => Shell.Current.GoToAsync("note");

    // Викликається згенерованою властивістю SelectedNote.
    async partial void OnSelectedNoteChanged(Note? value)
    {
        if (value is null)
        {
            return;
        }
        SelectedNote = null;      // зняти виділення в списку
        await Shell.Current.GoToAsync("note",
            new ShellNavigationQueryParameters { ["note"] = value });
    }
}
