using SpotAR.MAUI.ViewModels;

namespace SpotAR.MAUI.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		BindingContext = MauiProgram.Services.GetRequiredService<LoginPageViewModel>();
		InitializeComponent();
	}
}