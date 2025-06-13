using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Xunit;
using static SpotAR.Core.Test.Constants;

using static SpotAR.Core.Utils;

namespace SpotAR.Core.Test
{
    public class UtilsTest
    {
        [Theory]
        [ClassData(typeof(CameraPointingAtPlaneDataSimple))]
        [ClassData(typeof(PlaneBehindCameraDataSimple))]
        [ClassData(typeof(PlaneAtEdgeOfFovData))]
        public void IsInView_VariousOrientations_ReturnsExpectedResult(string datasetName, Quaternion orientation, GPSCoord deviceLocation, GPSCoord aircraftLocation, double halfHorizontalFov, double halfVerticalFov, bool expected)
        {
            bool isInView = IsInView(orientation, deviceLocation, aircraftLocation, halfHorizontalFov, halfVerticalFov);

            Assert.True(expected == isInView,
                $"Expected {expected} but got {isInView} for {datasetName}: orientation {orientation}, deviceLocation {deviceLocation}, aircraftLocation {aircraftLocation}, halfHorizontalFov {halfHorizontalFov}, halfVerticalFov {halfVerticalFov}");
        }


        public class CameraPointingAtPlaneDataSimple : IsInViewTheoryData
        {
            public CameraPointingAtPlaneDataSimple()
            {
                Add(CameraFaceDown, new GPSCoord(0, 0, 0), new GPSCoord(0, 0, -1), MathF.PI / 4, MathF.PI / 4, true);
                Add(CameraFaceUp, new GPSCoord(0, 0, 0), new GPSCoord(0, 0, 1), MathF.PI / 4, MathF.PI / 4, true);
                Add(CameraFacingDueNorth, new GPSCoord(0, 0, 0), new GPSCoord(1, 0, 0), MathF.PI / 4, MathF.PI / 4, true);
                Add(CameraFacingDueEast, new GPSCoord(0, 0, 0), new GPSCoord(0, 1, 0), MathF.PI / 4, MathF.PI / 4, true);
                Add(CameraFacingDueSouth, new GPSCoord(0, 0, 0), new GPSCoord(-1, 0, 0), MathF.PI / 4, MathF.PI / 4, true);
                Add(CameraFacingDueWest, new GPSCoord(0, 0, 0), new GPSCoord(0, -1, 0), MathF.PI / 4, MathF.PI / 4, true);
            }
        }

        public class PlaneBehindCameraDataSimple : IsInViewTheoryData
        {
            public PlaneBehindCameraDataSimple()
            {
                Add(CameraFaceDown, new GPSCoord(0, 0, 0), new GPSCoord(0, 0, 1), MathF.PI / 4, MathF.PI / 4, false);
                Add(CameraFaceUp, new GPSCoord(0, 0, 0), new GPSCoord(0, 0, -1), MathF.PI / 4, MathF.PI / 4, false);
                Add(CameraFacingDueNorth, new GPSCoord(0, 0, 0), new GPSCoord(-1, 0, 0), MathF.PI / 4, MathF.PI / 4, false);
                Add(CameraFacingDueEast, new GPSCoord(0, 0, 0), new GPSCoord(0, -1, 0), MathF.PI / 4, MathF.PI / 4, false);
                Add(CameraFacingDueSouth, new GPSCoord(0, 0, 0), new GPSCoord(1, 0, 0), MathF.PI / 4, MathF.PI / 4, false);
                Add(CameraFacingDueWest, new GPSCoord(0, 0, 0), new GPSCoord(0, 1, 0), MathF.PI / 4, MathF.PI / 4, false);
            }
        }

        public class PlaneAtEdgeOfFovData : IsInViewTheoryData
        {
            private const int LessThanOneDegreeMeters = 100000; // 1 degree in meters at the equator is approximately 111,139 meters, so this is a bit less than 1 degree.
            private const int MoreThanOneDegreeMeters = 120000; // 1 degree in meters at the equator is approximately 111,139 meters, so this is a bit more than 1 degree.

            public PlaneAtEdgeOfFovData()
            {
                // Camera face down at 1 degree of longitude, the plane is at the edge of the camera's field of view when it is around -111,139 meters in the air.
                Add(CameraFaceDown, new GPSCoord(0, 0, 0), new GPSCoord(0, 1, -LessThanOneDegreeMeters), MathF.PI / 4, MathF.PI / 4, false);
                Add(CameraFaceDown, new GPSCoord(0, 0, 0), new GPSCoord(0, 1, -MoreThanOneDegreeMeters), MathF.PI / 4, MathF.PI / 4, true);

                // With 45deg aperture instead of 90, the plane has to farther away to be in view (2.414 times as far).
                Add(CameraFaceDown, new GPSCoord(0, 0, 0), new GPSCoord(0, 1, -2.4f * LessThanOneDegreeMeters), MathF.PI / 8, MathF.PI / 8, false);
                Add(CameraFaceDown, new GPSCoord(0, 0, 0), new GPSCoord(0, 1, -2.5f * MoreThanOneDegreeMeters), MathF.PI / 8, MathF.PI / 8, true);

                // Camera face up at 1 degree of longitude, the plane is at the edge of the camera's field of view when it is around 111,139 meters in the air.
                Add(CameraFaceUp, new GPSCoord(0, 0, 0), new GPSCoord(0, 1, LessThanOneDegreeMeters), MathF.PI / 4, MathF.PI / 4, false);
                Add(CameraFaceUp, new GPSCoord(0, 0, 0), new GPSCoord(0, 1, MoreThanOneDegreeMeters), MathF.PI / 4, MathF.PI / 4, true);

                // Camera facing due North at 1 degree of latitude, the plane is at the edge of the camera's field of view when it is around 111,139 meters in the air.
                Add(CameraFacingDueNorth, new GPSCoord(0, 0, 0), new GPSCoord(1, 0, MoreThanOneDegreeMeters), MathF.PI / 4, MathF.PI / 4, false);
                Add(CameraFacingDueNorth, new GPSCoord(0, 0, 0), new GPSCoord(1, 0, LessThanOneDegreeMeters), MathF.PI / 4, MathF.PI / 4, true);

                // Camera facing due South at -1 degree of latitude, the plane is at the edge of the camera's field of view when it is around 111,139 meters in the air.
                Add(CameraFacingDueSouth, new GPSCoord(0, 0, 0), new GPSCoord(-1, 0, MoreThanOneDegreeMeters), MathF.PI / 4, MathF.PI / 4, false);
                Add(CameraFacingDueSouth, new GPSCoord(0, 0, 0), new GPSCoord(-1, 0, LessThanOneDegreeMeters), MathF.PI / 4, MathF.PI / 4, true);

                // Camera facing due East at 1 degree of longitude, the plane is at the edge of the camera's field of view when it is around 111,139 meters in the air.
                Add(CameraFacingDueEast, new GPSCoord(0, 0, 0), new GPSCoord(0, 1, MoreThanOneDegreeMeters), MathF.PI / 4, MathF.PI / 4, false);
                Add(CameraFacingDueEast, new GPSCoord(0, 0, 0), new GPSCoord(0, 1, LessThanOneDegreeMeters), MathF.PI / 4, MathF.PI / 4, true);

                // Camera facing due West at -1 degree of longitude, the plane is at the edge of the camera's field of view when it is around 111,139 meters in the air.
                Add(CameraFacingDueWest, new GPSCoord(0, 0, 0), new GPSCoord(0, -1, MoreThanOneDegreeMeters), MathF.PI / 4, MathF.PI / 4, false);
                Add(CameraFacingDueWest, new GPSCoord(0, 0, 0), new GPSCoord(0, -1, LessThanOneDegreeMeters), MathF.PI / 4, MathF.PI / 4, true);
            }
        }

        public class IsInViewTheoryData : TheoryData<string, Quaternion, GPSCoord, GPSCoord, double, double, bool>
        {
            public void Add(Quaternion orientation, GPSCoord deviceLocation, GPSCoord aircraftLocation, double halfHorizontalFov, double halfVerticalFov, bool expected)
            {
                Add(GetType().Name, orientation, deviceLocation, aircraftLocation, halfHorizontalFov, halfVerticalFov, expected);
            }
        }
    }
}