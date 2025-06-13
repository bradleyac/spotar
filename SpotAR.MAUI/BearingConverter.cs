using System;
using System.Globalization;

namespace SpotAR.MAUI;

public class BearingConverter : IValueConverter
{
    public static readonly BearingConverter Instance = new();
    private BearingConverter() { }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => (value as double?) switch
    {
        double bearing => (bearing % 360.0) switch
        {
            < 11 => "N",
            < 34 => "NNE",
            < 56 => "NE",
            < 79 => "ENE",
            < 101 => "E",
            < 124 => "ESE",
            < 146 => "SE",
            < 169 => "SSE",
            < 191 => "S",
            < 214 => "SSW",
            < 236 => "SW",
            < 259 => "WSW",
            < 281 => "W",
            < 304 => "WNW",
            < 326 => "NW",
            < 348 => "NNW",
            _ => "N"
        },
        _ => "N/A"
    };

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
