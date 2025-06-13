using System;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using SpotAR.MAUI.Authentication;
using SpotAR.MAUI.Models;

namespace SpotAR.MAUI.Services;

public class UserLoginService(HttpClient httpClient, OAuthClient oauthClient, LoggedInUserService loggedInUserService)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly OAuthClient _oauthClient = oauthClient;
    private readonly LoggedInUserService _loggedInUserService = loggedInUserService;

    internal async Task LoginAppleAsync()
    {
        throw new NotImplementedException();
    }

    internal async Task LoginGoogleAsync()
    {
        var result = await _oauthClient.LoginAsync();

        var apiResponse = await _httpClient.PostAsync(".auth/login/google", JsonContent.Create(new { id_token = result.IdentityToken, access_troken = result.AccessToken }));

        if (apiResponse.IsSuccessStatusCode)
        {
            var responseData = await apiResponse.Content.ReadFromJsonAsync<AuthResponse>() ?? throw new SpotARException("Failed to authenticate with API after successful oauth2 transaction.");
            _loggedInUserService.CurrentUser = new GoogleUser(responseData.authenticationToken, result.IdentityToken, result.AccessToken);
        }
    }

    private record AuthResponse(string authenticationToken);
}