using System.Globalization;
using System.IO.Compression;
using System.Numerics;
using static SpotAR.Core.Constants;

namespace SpotAR.Core;

public static partial class Utils
{
    public static bool IsInView(Quaternion deviceOrientation, GPSCoord deviceLocation, GPSCoord aircraftLocation, double halfHorizontalFov, double halfVerticalFov)
    {
        // Calculate the vector from the device to the aircraft
        var planeVectorEnu = aircraftLocation.GPSToENUInDeviceFrame(deviceLocation);

        var rotMatrix = Matrix4x4.CreateFromQuaternion(Quaternion.Inverse(deviceOrientation));

        var planeVectorLocalUncorrected = Vector3.Transform(planeVectorEnu.ToVector3(), rotMatrix);
        var planeVectorLocal = planeVectorLocalUncorrected with { Z = -planeVectorLocalUncorrected.Z }; // Invert Z because the camera is on the back of the device.

        if (planeVectorLocal.Z < 0)
        {
            // If the aircraft is behind the device, it cannot be in view
            return false;
        }

        // Calculate the horizontal and vertical angles
        var horizontalAngle = Math.Atan2(planeVectorLocal.X, planeVectorLocal.Z);
        var verticalAngle = Math.Atan2(planeVectorLocal.Y, planeVectorLocal.Z);

        // Check if the angles are within the specified field of view
        return Math.Abs(horizontalAngle) < halfHorizontalFov && Math.Abs(verticalAngle) < halfVerticalFov;
    }

    private static ENUCoord GPSToENUInDeviceFrame(this GPSCoord aircraftLocation, GPSCoord deviceLocation)
    {
        // Differences in radians
        var dLat = (aircraftLocation.Lat - deviceLocation.Lat) * MathF.PI / 180;
        var dLon = (aircraftLocation.Lon - deviceLocation.Lon) * MathF.PI / 180;
        var dAlt = aircraftLocation.Alt - deviceLocation.Alt;

        // Radius of curvature
        var M = WGS84_a * (1 - WGS84_e2) / MathF.Pow(1 - WGS84_e2 * MathF.Pow(MathF.Sin(deviceLocation.Lon), 2f), 1.5f); // meridional
        var N = WGS84_a / MathF.Sqrt(1 - WGS84_e2 * MathF.Pow(MathF.Sin(deviceLocation.Lat), 2f)); // prime vertical

        // ENU coordinates
        var east = dLon * N * MathF.Cos(deviceLocation.Lon);
        var north = dLat * M;
        var up = dAlt;

        return new ENUCoord(east, north, up);
    }
}
