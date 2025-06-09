using System.Diagnostics;
using System.Reflection;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SpotAR.MAUI.Pages;
using SpotAR.MAUI.ViewModels;

namespace SpotAR.MAUI;

public static class MauiProgram
{

	public static IServiceProvider Services { get; set; }
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

		var a = Assembly.GetExecutingAssembly();

		using var stream = a.GetManifestResourceStream($"{a.GetName().Name}.Resources.config.json");

		var config = new ConfigurationBuilder()
			.AddJsonStream(stream)
			.Build();

		builder.Configuration.AddConfiguration(config);

		builder.Services.AddSingleton<PlaneListViewModel>();
		builder.Services.AddSingleton<PlaneListPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		var built = builder.Build();

		Services = built.Services;

		return built;
	}
}
