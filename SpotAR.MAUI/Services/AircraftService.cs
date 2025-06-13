using System;
using System.Net.Http.Json;
using SpotAR.MAUI.Models;

namespace SpotAR.MAUI.Services;

public class AircraftService(HttpClient httpClient, LoggedInUserService userService, LocationService locationService)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly LoggedInUserService _userService = userService;
    private readonly LocationService _locationService = locationService;

    public async Task<List<Aircraft>> GetAircraftNearMeAsync()
    {
        if (!_httpClient.DefaultRequestHeaders.Contains("X-ZUMO-AUTH"))
        {
            _httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", _userService.CurrentUser?.AuthenticationToken ?? throw new SpotARException("User must be logged in."));
        }

        var location = await _locationService.GetLocationAsync() ?? throw new SpotARException("Failed to retrieve user location.");

        return await _httpClient.GetFromJsonAsync<List<Aircraft>>($"api/planes/nearby/{(float)location.Latitude}/{(float)location.Longitude}") ?? throw new SpotARException("Failed to retrieve nearby planes.");
    }
}
