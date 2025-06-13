using System;
using System.Text.Json.Serialization;

namespace SpotAR.MAUI.Models;

public class AircraftDTO
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
}
