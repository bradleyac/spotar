using SpotAR.MAUI.ViewModels;

namespace SpotAR.MAUI.Pages;

public partial class PlaneListPage : ContentView
{
	public PlaneListPage()
	{
		var vm = MauiProgram.Services.GetRequiredService<PlaneListViewModel>();
		BindingContext = vm;
		InitializeComponent();
	}
}