using System;
using System.Numerics;
using ObservableCollections;
using R3;

namespace SpotAR.Core;

public class Device
{
    public Observable<Quaternion> Orientation { get; }
    public Observable<GPSCoord> GPSLocation { get; }
    public double HorizontalFov { get; }
    public double VerticalFov { get; }
    public double HalfHorizontalFov { get; }
    public double HalfVerticalFov { get; }

    public Device(Observable<Quaternion> orientation, Observable<GPSCoord> gpsLocation, double horizontalFov, double verticalFov)
    {
        Orientation = orientation;
        GPSLocation = gpsLocation;
        HorizontalFov = horizontalFov;
        VerticalFov = verticalFov;
        HalfHorizontalFov = horizontalFov / 2;
        HalfVerticalFov = verticalFov / 2;
    }

    public Observable<Func<GPSCoord, bool>> GetIsInViewFilterFunc()
    {
        return Orientation.CombineLatest(GPSLocation, (orientation, deviceLocation) =>
        {
            return new Func<GPSCoord, bool>(aircraftLocation =>
                Utils.IsInView(orientation, deviceLocation, aircraftLocation, HalfHorizontalFov, HalfVerticalFov));
        });
    }
}
