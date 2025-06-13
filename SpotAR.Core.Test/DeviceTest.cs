using System;
using System.Numerics;
using ObservableCollections;
using R3;

namespace SpotAR.Core.Test;

public class DeviceTest
{
    [Fact]
    public void GetIsInViewFilterFunc_ReturnedFuncFiltersListCorrectly()
    {
        BehaviorSubject<Quaternion> deviceOrientation = new BehaviorSubject<Quaternion>(Constants.CameraFaceDown);
        BehaviorSubject<GPSCoord> deviceLocation = new BehaviorSubject<GPSCoord>(new GPSCoord(0, 0, 0));
        // Arrange
        var device = new Device(
            deviceOrientation,
            deviceLocation,
            Math.PI / 4,
            Math.PI / 4);

        var filterFunc = device.GetIsInViewFilterFunc();

        // Act
        var aircraftLocationUnderDevice = new GPSCoord(0, 0, -1);
        var aircraftLocationSouthOfDevice = new GPSCoord(-1, 0, 0);

        ObservableList<GPSCoord> aircraftLocations = new ObservableList<GPSCoord>
        {
            aircraftLocationUnderDevice,
            aircraftLocationSouthOfDevice
        };

        var view = aircraftLocations.CreateView(x => x);

        using var filterSub = filterFunc.Subscribe(func =>
        {
            view.AttachFilter(func);
        });

        Assert.Equivalent(new[] { aircraftLocationUnderDevice }, view);

        deviceOrientation.OnNext(Constants.CameraFacingDueSouth);

        Assert.Equivalent(new[] { aircraftLocationSouthOfDevice }, view);

        deviceLocation.OnNext(new GPSCoord(-2, 0, 0));

        Assert.Equivalent(Enumerable.Empty<GPSCoord>(), view);

        var aircraft3DegreesSouth = new GPSCoord(-3, 0, 0);

        aircraftLocations.Add(aircraft3DegreesSouth);

        Assert.Equivalent(new[] { aircraft3DegreesSouth }, view);
    }
}