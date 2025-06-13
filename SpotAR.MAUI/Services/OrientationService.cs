using System;
using System.Numerics;
using R3;

namespace SpotAR.MAUI.Services;

public class OrientationService(IOrientationSensor orientation)
{
    private readonly IOrientationSensor _orientation = orientation;

    public Observable<Quaternion> OrientationChanged =>
        Observable.FromEventHandler<OrientationSensorChangedEventArgs>(
            handler => _orientation.ReadingChanged += handler,
            handler => _orientation.ReadingChanged -= handler
        ).Select(args => args.Item2.Reading.Orientation);

    public void StartMonitoring()
    {
        if (!_orientation.IsMonitoring)
        {
            _orientation.Start(SensorSpeed.UI);
        }
    }
    public void StopMonitoring()
    {
        if (_orientation.IsMonitoring)
        {
            _orientation.Stop();
        }
    }
}
