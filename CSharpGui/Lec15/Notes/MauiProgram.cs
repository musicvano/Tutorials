using Notes.Services;
using Notes.ViewModels;
using Notes.Views;

namespace Notes;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        builder.Services.AddSingleton<INoteStore>(
            new FileNoteStore(FileSystem.AppDataDirectory));
        builder.Services.AddSingleton<NotesViewModel>();
        builder.Services.AddSingleton<NotesPage>();
        builder.Services.AddTransient<NoteViewModel>();
        builder.Services.AddTransient<NotePage>();

        return builder.Build();
    }
}
