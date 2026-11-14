using System.Globalization;

namespace WhereAmI;

public partial class MainPage : ContentPage
{
    private static readonly Location London = new(51.5074, -0.1278);
    private Location? current;

    public MainPage() => InitializeComponent();

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Connectivity.Current.ConnectivityChanged += OnConnectivity;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Connectivity.Current.ConnectivityChanged -= OnConnectivity;
    }

    // Подія може надійти не з головного потоку.
    private void OnConnectivity(object? sender,
        ConnectivityChangedEventArgs e) =>
        MainThread.BeginInvokeOnMainThread(() =>
            networkLabel.Text = $"Network: {e.NetworkAccess}");

    private async void OnLocateClicked(object? sender, EventArgs e)
    {
        PermissionStatus status = await Permissions
            .CheckStatusAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions
                .RequestAsync<Permissions.LocationWhenInUse>();
        }
        if (status != PermissionStatus.Granted)
        {
            await DisplayAlertAsync("Location",
                "Allow location access in the app settings.", "OK");
            return;
        }
        busyIndicator.IsRunning = true;
        try
        {
            current = await Geolocation.Default.GetLocationAsync(
                new GeolocationRequest(GeolocationAccuracy.Medium,
                    TimeSpan.FromSeconds(10)));
            if (current is null)
            {
                coordinatesLabel.Text = "Location is unavailable";
                return;
            }
            coordinatesLabel.Text =
                $"{current.Latitude:F5}; {current.Longitude:F5}";
            double km = Location.CalculateDistance(
                current, London, DistanceUnits.Kilometers);
            distanceLabel.Text = $"Distance to London: {km:F1} km";
            shareButton.IsEnabled = true;
        }
        catch (FeatureNotEnabledException)
        {
            coordinatesLabel.Text = "Turn on location services";
        }
        finally
        {
            busyIndicator.IsRunning = false;
        }
    }

    private async void OnShareClicked(object? sender, EventArgs e)
    {
        // Крапка в координатах за будь-яких регіональних налаштувань.
        var inv = CultureInfo.InvariantCulture;
        string lat = current!.Latitude.ToString("F5", inv);
        string lon = current.Longitude.ToString("F5", inv);
        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Title = "My location",
            Uri = $"https://www.openstreetmap.org/?mlat={lat}"
                + $"&mlon={lon}"
        });
    }
}
