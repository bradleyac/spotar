using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SpotAR.Core;

namespace SpotAR.MAUI.Models;

public class Aircraft
{
    [JsonPropertyName("flight")]
    public required string Flight { get; set; }
    [JsonPropertyName("lat")]
    public float Latitude { get; set; }
    [JsonPropertyName("lon")]
    public float Longitude { get; set; }
    [JsonPropertyName("alt_geom")]
    public float? AltGeomFt { get; set; }
    [JsonPropertyName("alt_baro")]
    public float? AltBaroFt { get; set; }

    [NotMapped]
    public GPSCoord GPSLocation => new GPSCoord(Latitude, Longitude, AltGeomFt ?? AltBaroFt ?? 0);
}
