namespace Notes.Views;

public partial class AboutPage : ContentPage
{
    public AboutPage()
    {
        InitializeComponent();
        versionLabel.Text = $"Version: {AppInfo.Current.VersionString}";
        platformLabel.Text = $"Platform: {DeviceInfo.Current.Platform}";
        folderLabel.Text =
            $"Data folder: {FileSystem.Current.AppDataDirectory}";
    }
}
