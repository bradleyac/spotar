using System.Diagnostics;
using System.Reflection;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SpotAR.MAUI.Authentication;
using SpotAR.MAUI.Pages;
using SpotAR.MAUI.Services;
using SpotAR.MAUI.ViewModels;

namespace SpotAR.MAUI;

public static class MauiProgram
{

	public static IServiceProvider Services { get; set; } = default!;
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		var currentAssembly = Assembly.GetExecutingAssembly();

		using var stream = currentAssembly.GetManifestResourceStream($"{currentAssembly.GetName().Name}.Resources.config.json");

		var config = new ConfigurationBuilder()
			.AddJsonStream(stream!)
			.Build();

		builder.Configuration.AddConfiguration(config);

		var apiSettings = builder.Configuration.GetRequiredSection("ApiSettings").Get<ApiSettings>();

		builder.Services.AddSingleton<PlaneListViewModel>();
		builder.Services.AddSingleton<LoginPageViewModel>();

		builder.Services.AddHttpClient<AircraftService>(conf =>
		{
			conf.BaseAddress = new Uri(apiSettings!.Url);
		});

		builder.Services.AddHttpClient<UserLoginService>(conf =>
		{
			conf.BaseAddress = new Uri(apiSettings!.Url);
		});

		var authSettings = builder.Configuration.GetRequiredSection("GoogleAuthSettings").Get<GoogleAuthSettings>();

		var oauthClientConfiguration = new OAuthClientConfiguration
		{
			Authority = "https://accounts.google.com",
			Browser = new WebBrowserAuthenticator(),
			ClientId = authSettings.GoogleOAuthClientId,
			RedirectUri = authSettings.GoogleOAuthRedirectUri,
		};

		builder.Services.AddSingleton(new OAuthClient(oauthClientConfiguration));

		builder.Services.AddSingleton<LoggedInUserService>();
		builder.Services.AddSingleton<LocationService>();
		builder.Services.AddSingleton<OrientationService>();
		builder.Services.AddSingleton(Geolocation.Default);
		builder.Services.AddSingleton(OrientationSensor.Default);

#if DEBUG
		builder.Logging.AddDebug();
#endif

		var built = builder.Build();

		Services = built.Services;

		return built;
	}
}
