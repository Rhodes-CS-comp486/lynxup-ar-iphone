using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class LocationTrigger : MonoBehaviour
{
    // public GameObject arItemPrefab; // The item to place
    // public double targetLatitude;
    // public double targetLongitude;
    public List<GameObject> itemPrefabs;
    public float triggerRadius = 5f; // meters

    private bool itemPlaced = false;
    private double originLat;
    private double originLon;

    // include all possible locations where items can spawn
    private List<Locations.TargetLocation> locations = Locations.targets;
    [System.Serializable]
    public class ARItem
    {
        public GameObject prefab;
        public double latitude;
        public double longitude;
        public bool hasSpawned = false;
    }

    public List<ARItem> arItems;
    IEnumerator Start()
    {
        // Start location service
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location service not enabled");
            yield break;
        }

        Input.location.Start();
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (Input.location.status != LocationServiceStatus.Running)
        {
            Debug.Log("Location service failed to start");
            yield break;
        }

        originLat = Input.location.lastData.latitude;
        originLon = Input.location.lastData.longitude;

        StartCoroutine(CheckProximity());
    }

    IEnumerator CheckProximity()
    {
        while (true)
        {
            double userLat = Input.location.lastData.latitude;
            double userLon = Input.location.lastData.longitude;

            foreach (var item in arItems)
            {
                if (item.hasSpawned) continue;

                float distance = CalculateDistance(userLat, userLon, item.latitude, item.longitude);
                if (distance <= triggerRadius)
                {
                    Vector3 spawnPos = LatLongToUnityPosition(item.latitude, item.longitude, originLat, originLon);
                    Instantiate(item.prefab, spawnPos, Quaternion.identity);
                    item.hasSpawned = true;
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }

    float CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        float R = 6371000f; // Earth radius in meters
        float dLat = Mathf.Deg2Rad * (float)(lat2 - lat1);
        float dLon = Mathf.Deg2Rad * (float)(lon2 - lon1);

        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
                  Mathf.Cos(Mathf.Deg2Rad * (float)lat1) * Mathf.Cos(Mathf.Deg2Rad * (float)lat2) *
                  Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);

        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        return R * c;
    }

    Vector3 LatLongToUnityPosition(double lat, double lon, double refLat, double refLon)
    {
        float earthRadius = 6378137f;

        double latDiff = lat - refLat;
        double lonDiff = lon - refLon;

        float xOffset = (float)(lonDiff * Mathf.Deg2Rad * earthRadius * Mathf.Cos((float)refLat * Mathf.Deg2Rad));
        float zOffset = (float)(latDiff * Mathf.Deg2Rad * earthRadius);

        return new Vector3(xOffset, 0, zOffset);
    }
    
    public static Vector2 GenerateRandomCoordinateNear(double lat, double lon, float radiusMeters)
    {
        float radiusInDegrees = radiusMeters / 111000f;

        float u = UnityEngine.Random.value;
        float v = UnityEngine.Random.value;
        float w = radiusInDegrees * Mathf.Sqrt(u);
        float t = 2 * Mathf.PI * v;
        float deltaLat = w * Mathf.Cos(t);
        float deltaLon = w * Mathf.Sin(t) / Mathf.Cos(Mathf.Deg2Rad * (float)lat);

        double newLat = lat + deltaLat;
        double newLon = lon + deltaLon;

        return new Vector2((float)newLat, (float)newLon);
    }

    
}
