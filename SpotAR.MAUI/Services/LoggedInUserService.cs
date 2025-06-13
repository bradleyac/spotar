using System;
using SpotAR.MAUI.Models;

namespace SpotAR.MAUI.Services;

public class LoggedInUserService
{
    public IUser? CurrentUser { get; set; }
}
