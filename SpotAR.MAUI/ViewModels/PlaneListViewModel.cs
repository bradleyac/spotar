using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ObservableCollections;
using R3;
using SpotAR.MAUI.Models;
using SpotAR.MAUI.Services;

using Device = SpotAR.Core.Device;

namespace SpotAR.MAUI.ViewModels;

public partial class PlaneListViewModel : ViewModelBase
{
    private readonly AircraftService _aircraftService;
    private readonly LocationService _locationService;
    private readonly OrientationService _orientationService;

    private IDisposable? _aircraftSubscription;
    private IDisposable? _viewFilterSubscription;
    private ObservableList<Aircraft> _aircraft = new();
    private ISynchronizedView<Aircraft, Aircraft> _filterableView;

    [ObservableProperty]
    private NotifyCollectionChangedSynchronizedViewList<Aircraft> _planesView;

    [ObservableProperty]
    private Device _device;

    public PlaneListViewModel(AircraftService aircraftService, LocationService locationService, OrientationService orientationService)
    {
        _aircraftService = aircraftService ?? throw new ArgumentNullException(nameof(aircraftService));
        _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
        _orientationService = orientationService ?? throw new ArgumentNullException(nameof(orientationService));
        _filterableView = _aircraft.CreateView(x => x);
        _planesView = _filterableView.ToNotifyCollectionChanged();
        Device = new Device(orientationService.OrientationChanged, locationService.LocationChanged, Math.PI / 2, Math.PI / 2);

        _viewFilterSubscription = Device.GetIsInViewFilterFunc()
            .Subscribe(filterFunc => _filterableView.AttachFilter(aircraft => filterFunc(aircraft.GPSLocation)));
    }

    [RelayCommand]
    private async Task Appearing()
    {
        await _locationService.StartMonitoringAsync();
        _orientationService.StartMonitoring();
        _aircraftSubscription?.Dispose();

        Observable<List<Aircraft>>? planesObservable = Observable
            .Timer(TimeSpan.FromSeconds(2), TimeSpan.FromMinutes(1))
            .WithLatestFrom(_locationService.LocationChanged, (unit, location) => location)
            .SelectAwait(async (location, cancel) => await _aircraftService.GetAircraftNearLocationAsync(location));

        _aircraftSubscription = planesObservable
            .ObserveOnCurrentSynchronizationContext()
            .Subscribe(planes =>
            {
                _aircraft.Clear();

                if (planes is not null)
                {
                    _aircraft.AddRange(planes);
                }
            });
    }

    [RelayCommand]
    private void Disappearing()
    {
        _aircraftSubscription?.Dispose();
        _viewFilterSubscription?.Dispose();
        _locationService.StopMonitoring();
        _orientationService.StopMonitoring();
        _aircraft.Clear();
    }
}
