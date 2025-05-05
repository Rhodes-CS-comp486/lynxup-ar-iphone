using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public static class GPSConverter
{
    private static float earthRadius = 6371000f; // in meters

    public static Vector3 GPS2UnityPosition(float latOrigin, float lonOrigin, float latTarget, float lonTarget)
    {
        float dLat = Mathf.Deg2Rad * (latTarget - latOrigin);
        float dLon = Mathf.Deg2Rad * (lonTarget - lonOrigin);

        float latAvg = Mathf.Deg2Rad * (latTarget + latOrigin) / 2f;

        float x = earthRadius * dLon * Mathf.Cos(latAvg);
        float z = earthRadius * dLat;

        return new Vector3(x, 0f, z); // assuming flat ground
    }
}