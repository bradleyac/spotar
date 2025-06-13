using SpotAR.MAUI.ViewModels;

namespace SpotAR.MAUI.Pages;

public partial class PlaneListPage : ContentPage
{
	public PlaneListPage()
	{
		BindingContext = MauiProgram.Services.GetRequiredService<PlaneListViewModel>();
		InitializeComponent();
	}
}