using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SpotAR.Core;
using static SpotAR.Core.Utils;

namespace SpotAR.MAUI.Models;

public class Aircraft
{
    public string Flight { get; init; }
    public float Latitude { get; init; }
    public float Longitude { get; init; }
    public float Altitude { get; init; }
    public GPSCoord GPSLocation { get; init; }
    public double Distance { get; init; }
    public double RelativeBearing { get; init; } // Degrees from North

    public Aircraft(AircraftDTO dto, GPSCoord deviceLocation)
    {
        Flight = dto.Flight;
        Latitude = dto.Latitude;
        Longitude = dto.Longitude;
        Altitude = (dto.AltGeomFt ?? dto.AltBaroFt ?? 0) * 0.3048f; // Convert feet to meters
        GPSLocation = new GPSCoord(Latitude, Longitude, Altitude);
        var deviceLocationLocation = new Location(deviceLocation.Lat, deviceLocation.Lon, deviceLocation.Alt);
        var distance = Location.CalculateDistance(deviceLocationLocation, new Location(Latitude, Longitude, Altitude), DistanceUnits.Kilometers);
        var bearing = GetBearing(deviceLocation.Lat, deviceLocation.Lon, Latitude, Longitude);
        Distance = distance;
        RelativeBearing = bearing;
    }

    private double GetBearing(double latitude1, double longitude1, float latitude2, float longitude2)
    {
        double lat1Rad = latitude1 * MathF.PI / 180;
        double lon1Rad = longitude1 * MathF.PI / 180;
        double lat2Rad = latitude2 * MathF.PI / 180;
        double lon2Rad = longitude2 * MathF.PI / 180;

        double dLon = lon2Rad - lon1Rad;

        double x = Math.Sin(dLon) * Math.Cos(lat2Rad);
        double y = Math.Cos(lat1Rad) * Math.Sin(lat2Rad) - Math.Sin(lat1Rad) * Math.Cos(lat2Rad) * Math.Cos(dLon);

        double bearing = Math.Atan2(x, y);

        return ((bearing * 180 / MathF.PI) + 360) % 360; // Normalize to 0-360 degrees
    }
}
