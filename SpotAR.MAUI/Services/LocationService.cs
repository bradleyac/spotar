using System;
using System.Threading.Tasks;
using R3;
using SpotAR.Core;
using static SpotAR.Core.Utils;

namespace SpotAR.MAUI.Services;

public class LocationService
{
    private readonly IGeolocation _geolocation;

    public LocationService(IGeolocation geolocation)
    {
        _geolocation = geolocation ?? throw new ArgumentNullException(nameof(geolocation));
        LocationChanged = LocationChangedRaw.Replay(1).RefCount();
    }

    public Task<Location?> GetLocationAsync() => _geolocation.GetLocationAsync(new(GeolocationAccuracy.Best));
    private Observable<GPSCoord> LocationChangedRaw =>
        Observable.FromEventHandler<GeolocationLocationChangedEventArgs>(
            handler => _geolocation.LocationChanged += handler,
            handler => _geolocation.LocationChanged -= handler
        ).Select(args => new GPSCoord((float)args.Item2.Location.Latitude, (float)args.Item2.Location.Longitude, (float?)args.Item2.Location.Altitude ?? 0f));

    public Observable<GPSCoord> LocationChanged { get; init; }

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
