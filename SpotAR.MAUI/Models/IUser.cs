namespace SpotAR.MAUI.Models;

public interface IUser
{
    public AuthProvider AuthProvider { get; }
    public string AuthenticationToken { get; }
    public string IdToken { get; }
    public string AccessToken { get; }
}
