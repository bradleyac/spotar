using System;
using System.Numerics;
using Microsoft.VisualBasic;
using R3;

namespace SpotAR.MAUI.Services;

public class OrientationService
{
    private readonly IOrientationSensor _orientation;
    public OrientationService(IOrientationSensor orientation)
    {
        _orientation = orientation ?? throw new ArgumentNullException(nameof(orientation));
        OrientationChanged = OrientationChangedRaw
            .ThrottleLast(TimeSpan.FromMilliseconds(100))
            .ObserveOnCurrentSynchronizationContext()
            .Replay(1)
            .RefCount();
    }

    public Observable<Quaternion> OrientationChangedRaw =>
        Observable.FromEventHandler<OrientationSensorChangedEventArgs>(
            handler => _orientation.ReadingChanged += handler,
            handler => _orientation.ReadingChanged -= handler
        ).Select(args => args.Item2.Reading.Orientation);

    public Observable<Quaternion> OrientationChanged { get; init; }

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
