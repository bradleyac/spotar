using System;
using System.Numerics;

namespace SpotAR.Core.Test;

public static class Constants
{
    // The following quaternions represent the camera orientations for various scenarios.
    public static readonly Quaternion CameraFaceDown = Quaternion.Identity; //No rotation, screen facing up so camera is down.
    public static readonly Quaternion CameraFaceUp = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathF.PI); // Rotated 180 degrees around X axis, so camera is up.
    public static readonly Quaternion CameraFacingDueNorth = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathF.PI / 2); // Rotated 90 degrees around X axis.
    public static readonly Quaternion CameraFacingDueEast = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathF.PI / 2)
        * Quaternion.CreateFromAxisAngle(Vector3.UnitY, -MathF.PI / 2); // Rotated 90 degrees around X axis then -90 around Y axis.
    public static readonly Quaternion CameraFacingDueSouth = Quaternion.CreateFromAxisAngle(Vector3.UnitX, -MathF.PI / 2); // Rotated -90 degrees around X axis.
    public static readonly Quaternion CameraFacingDueWest = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathF.PI / 2)
        * Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathF.PI / 2); // Rotated 90 degrees around X axis then 90 around Y axis.

}
