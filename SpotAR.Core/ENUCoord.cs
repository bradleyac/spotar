using System.Numerics;

namespace SpotAR.Core;

public record ENUCoord(float East, float North, float Up)
{
    public Vector3 ToVector3() => new Vector3(East, North, Up);
};