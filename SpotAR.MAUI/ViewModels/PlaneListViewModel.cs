using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SpotAR.MAUI.Models;

namespace SpotAR.MAUI.ViewModels;

public partial class PlaneListViewModel : ViewModelBase
{
    [ObservableProperty]
    private List<Plane> _planes = new[] { "Allen", "Mark", "Randy" }.Select(name => new Plane() { Identifier = name }).ToList();
}
