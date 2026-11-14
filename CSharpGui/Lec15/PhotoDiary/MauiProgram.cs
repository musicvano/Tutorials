using Microsoft.Extensions.Logging;

namespace PhotoDiary;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		string dbPath = Path.Combine(FileSystem.AppDataDirectory, "diary.db3");
		builder.Services.AddSingleton(new DiaryDatabase(dbPath));
		builder.Services.AddSingleton(MediaPicker.Default);
		builder.Services.AddSingleton<DiaryViewModel>();
		builder.Services.AddSingleton<DiaryPage>();
		builder.Services.AddTransient<EntryViewModel>();
		builder.Services.AddTransient<EntryPage>();

		return builder.Build();
	}
}
