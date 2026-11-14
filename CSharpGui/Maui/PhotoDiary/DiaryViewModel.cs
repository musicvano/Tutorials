using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PhotoDiary;

public partial class DiaryViewModel(DiaryDatabase database,
    IMediaPicker mediaPicker) : ObservableObject
{
    public ObservableCollection<DiaryEntry> Entries { get; } = [];

    // The book shows LoadAsync only as a comment.
    [RelayCommand]
    private async Task LoadAsync()
    {
        Entries.Clear();
        foreach (DiaryEntry entry in await database.GetAllAsync())
        {
            Entries.Add(entry);
        }
    }

    // Executed by the TapGestureRecognizer of a grid cell.
    [RelayCommand]
    private Task OpenAsync(DiaryEntry entry) =>
        Shell.Current.GoToAsync($"entry?id={entry.Id}");

    [RelayCommand]
    private async Task PickAsync()
    {
        List<FileResult> photos = await mediaPicker.PickPhotosAsync(
            new MediaPickerOptions
            {
                SelectionLimit = 1,
                MaximumWidth = 1600,
                MaximumHeight = 1600
            });
        await AddAsync(photos.FirstOrDefault());
    }

    [RelayCommand]
    private async Task CaptureAsync()
    {
        if (!mediaPicker.IsCaptureSupported)
        {
            await Shell.Current.DisplayAlertAsync("Camera",
                "This device has no camera.", "OK");
            return;
        }
        await AddAsync(await mediaPicker.CapturePhotoAsync());
    }

    private async Task AddAsync(FileResult? photo)
    {
        if (photo is null)
        {
            return;                        // користувач скасував
        }
        // Копія фото в папці застосунку не залежить від галереї.
        string target = Path.Combine(FileSystem.AppDataDirectory,
            $"{Guid.NewGuid():N}{Path.GetExtension(photo.FileName)}");
        using (Stream source = await photo.OpenReadAsync())
        using (FileStream file = File.Create(target))
        {
            await source.CopyToAsync(file);
        }
        var entry = new DiaryEntry
        {
            PhotoPath = target,
            Date = DateTime.Now
        };
        await database.SaveAsync(entry);
        await Shell.Current.GoToAsync($"entry?id={entry.Id}");
    }
}
