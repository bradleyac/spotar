using Microsoft.Extensions.Configuration;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SpotAR.MAUI;

public partial class MainPage : ContentPage
{
	public MainPage(IConfiguration config)
	{
		InitializeComponent();
	}
}
