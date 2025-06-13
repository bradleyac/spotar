using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SpotAR.MAUI.Services;

namespace SpotAR.MAUI.ViewModels;

public partial class LoginPageViewModel(UserLoginService userService) : ViewModelBase
{
    private UserLoginService _userService = userService;

    [RelayCommand]
    public async Task<bool> LoginGoogle()
    {
        try
        {
            await _userService.LoginGoogleAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    [RelayCommand]
    public async Task<bool> LoginApple()
    {
        try
        {
            await _userService.LoginAppleAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}
