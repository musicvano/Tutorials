using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PhotoDiary;

public partial class EntryViewModel(DiaryDatabase database)
    : ObservableObject, IQueryAttributable
{
    private DiaryEntry? entry;

    [ObservableProperty]
    public partial string Caption { get; set; } = "";

    [ObservableProperty]
    public partial string PhotoPath { get; set; } = "";

    // Значення id надходить як рядок: GoToAsync($"entry?id={id}").
    public async void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out object? value)
            && int.TryParse(value?.ToString(), out int id))
        {
            entry = await database.GetAsync(id);
            if (entry is not null)
            {
                Caption = entry.Caption;
                PhotoPath = entry.PhotoPath;
            }
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (entry is not null)
        {
            entry.Caption = Caption.Trim();
            await database.SaveAsync(entry);
        }
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (entry is not null)
        {
            await database.DeleteAsync(entry);
            File.Delete(entry.PhotoPath);
        }
        await Shell.Current.GoToAsync("..");
    }
}
