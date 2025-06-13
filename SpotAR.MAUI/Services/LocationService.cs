using System;
using System.Threading.Tasks;
using R3;
using SpotAR.Core;

namespace SpotAR.MAUI.Services;

public class LocationService(IGeolocation geolocation)
{
    private readonly IGeolocation _geolocation = geolocation;

    public Task<Location?> GetLocationAsync() => _geolocation.GetLocationAsync(new(GeolocationAccuracy.Best));
    public Observable<GPSCoord> LocationChanged =>
        Observable.FromEventHandler<GeolocationLocationChangedEventArgs>(
            handler => _geolocation.LocationChanged += handler,
            handler => _geolocation.LocationChanged -= handler
        ).Select(args => new GPSCoord((float)args.e.Location.Latitude, (float)args.Item2.Location.Longitude, (float?)args.Item2.Location.Altitude ?? 0f));

    public async Task StartMonitoringAsync()
    {
        if (!_geolocation.IsListeningForeground)
        {
            await _geolocation.StartListeningForegroundAsync(new(GeolocationAccuracy.Best, TimeSpan.FromSeconds(5)));
        }
    }
    public void StopMonitoring()
    {
        _geolocation.StopListeningForeground();
    }
}
