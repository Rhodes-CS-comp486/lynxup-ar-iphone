using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.Networking;

public class LocationTrigger : MonoBehaviour
{
    // public GameObject arItemPrefab; // The item to place
    // public double targetLatitude;
    // public double targetLongitude;
    public List<GameObject> itemPrefabs;
    public float triggerRadius = 20f; // meters

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

    [System.Serializable]
    public class Item
    {
        public string description;
        public string name;
        public int xp;
    }

    [System.Serializable]
    public class Items
    {
        public List<Item> items;
    }
    
    [System.Serializable]
    public class BackendItem
    {
        public double latitude;
        public double longitude;
        public string name;
    }

    [System.Serializable]
    public class BackendLocation
    {
        public double latitude;
        public double longitude;
        public string name;
        public List<BackendItem> items;
    }

    [System.Serializable]
    public class BackendLocations
    {
        public List<BackendLocation> locations;
    }

    public BackendLocations backendLocationsList;
    public Items itemsList;
    public Dictionary<string,Item> itemsDictionary;

    private HashSet<string> triggeredLocations = new();
    
    public List<ARItem> arItems;
    IEnumerator Start()
    {
        // Start location service
        StartCoroutine(FetchLocations());
        StartCoroutine(FetchItems());
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
        StartCoroutine(CheckProximityRoutine());
    }

    IEnumerator CheckProximityRoutine()
    {
        while (true)
        {
            Vector2 playerGPS = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);

            foreach (var location in backendLocationsList.locations)
            {
                string locationId = location.name;
                if (triggeredLocations.Contains(locationId)) continue;

                Vector2 targetGPS = new Vector2((float)location.latitude, (float)location.longitude);
                float distance = DistanceInMeters(playerGPS, targetGPS);

                if (distance <= triggerRadius)
                {
                    Debug.Log($"Player reached {location.name}, spawning items!");
                    SpawnItemsAt(location);
                    triggeredLocations.Add(locationId);
                }
            }

            yield return new WaitForSeconds(2f); // Check every 2 seconds
        }
    }
    
    public float DistanceInMeters(Vector2 coord1, Vector2 coord2)
    {
        float EarthRadius = 6371000f;
        float dLat = Mathf.Deg2Rad * (coord2.x - coord1.x);
        float dLon = Mathf.Deg2Rad * (coord2.y - coord1.y);

        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
                  Mathf.Cos(Mathf.Deg2Rad * coord1.x) * Mathf.Cos(Mathf.Deg2Rad * coord2.x) *
                  Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);

        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        return EarthRadius * c;
    }
    
    void SpawnItemsAt(BackendLocation location)
    {
        foreach (var item in location.items)
        {
            if (ItemManager.prefabs.TryGetValue(item.name, out GameObject prefab))
            {
                // Spawn item near the player, random offset
                Vector3 spawnPos = player.transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                Instantiate(prefab, spawnPos, Quaternion.identity);
            }
        }
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

    IEnumerator FetchItems()
    {
        UnityWebRequest request = UnityWebRequest.Get(UserSession.BackendURL + "get_items");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            itemsList = JsonUtility.FromJson<Items>(request.downloadHandler.text);
            Debug.Log("Successfully retrieved items");
            itemsDictionary = itemsList.items.ToDictionary(item => item.name);
            foreach (var item in itemsList.items)
            {
                Debug.Log("Item ID: " + item.name);
            }
            // foreach (BackendLocation location in backendLocationsList.locations)
            // {
            //     Debug.Log(location.latitude + " " + location.longitude + " " + location.name);
            // }
        }
        else
        {
            Debug.LogError("Failed to fetch items: " + request.error);
        }
    }
    
    IEnumerator FetchLocations()
    {
        UnityWebRequest request = UnityWebRequest.Get(UserSession.BackendURL + "get_locations");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            backendLocationsList = JsonUtility.FromJson<BackendLocations>(request.downloadHandler.text);
            Debug.Log("Successfully retrieved locations");
            foreach (BackendLocation location in backendLocationsList.locations)
            {
                Debug.Log(location.latitude + " " + location.longitude + " " + location.name);
            }
        }
        else
        {
            Debug.LogError("Failed to fetch locations: " + request.error);
        }
    }

    
}
