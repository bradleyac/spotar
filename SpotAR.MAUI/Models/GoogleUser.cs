namespace SpotAR.MAUI.Models;

public class GoogleUser(string authenticationToken, string idToken, string accessToken) : IUser
{
    public AuthProvider AuthProvider => AuthProvider.Google;
    public string IdToken => idToken;
    public string AccessToken => accessToken;

    public string AuthenticationToken => authenticationToken;
}