using System;

namespace SpotAR.MAUI.Authentication;

public class OAuthClientConfiguration
{
    public required string ClientId { get; set; }

    public required string RedirectUri { get; set; }

    public string? PostLogoutRedirectUri { get; set; }

    public IList<string> Scope { get; set; } = ["openid email"];

    public required string Authority { get; set; }

    public Duende.IdentityModel.OidcClient.Browser.IBrowser Browser { get; set; }

}
