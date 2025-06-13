using SpotAR.Core;


namespace SpotAR.MAUI;

public static class Extensions
{
    public static GPSCoord ToGPSCoord(this Location location)
    {
        _ = location ?? throw new ArgumentNullException(nameof(location));

        return new GPSCoord((float)location.Latitude, (float)location.Longitude, (float?)location.Altitude ?? 0.0f);
    }
}
