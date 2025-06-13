using System;
using System.Net.Http.Json;
using SpotAR.MAUI.Models;
using SpotAR.Core;

namespace SpotAR.MAUI.Services;

public class AircraftService(HttpClient httpClient, LoggedInUserService userService)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly LoggedInUserService _userService = userService;

    public async Task<List<Aircraft>> GetAircraftNearLocationAsync(GPSCoord location)
    {
        if (!_httpClient.DefaultRequestHeaders.Contains("X-ZUMO-AUTH"))
        {
            _httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", _userService.CurrentUser?.AuthenticationToken ?? throw new SpotARException("User must be logged in."));
        }

        var aircraft = await _httpClient.GetFromJsonAsync<List<AircraftDTO>>($"api/planes/nearby/{location.Lat}/{location.Lon}") ?? throw new SpotARException("Failed to retrieve nearby planes.");

        return aircraft.Select(a => new Aircraft(a, location)).OrderBy(aircraft => aircraft.Distance).ToList();
    }
}
