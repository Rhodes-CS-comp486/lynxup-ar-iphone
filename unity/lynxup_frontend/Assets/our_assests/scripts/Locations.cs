using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class Locations
{
    [System.Serializable]
    public class TargetLocation
    {
        public string name;
        public float latitude;
        public float longitude;
        public List<string> items;

        public TargetLocation(string name, float latitude, float longitude)
        {
            this.name = name;
            this.latitude = latitude;
            this.longitude = longitude;
            this.items = new List<string>();
        }
    }

    [System.Serializable]
    public class LocationListWrapper
    {
        public List<TargetLocation> locations;

        public LocationListWrapper(List<TargetLocation> locations)
        {
            this.locations = locations;
        }
    }

    [Header("List of Targets")]
    public static List<TargetLocation> targets = new List<TargetLocation>
    {
        new TargetLocation("Briggs", 35.15489f, -89.98911f),
        new TargetLocation("Southwestren", 35.15369f, -89.98938f),
        new TargetLocation("Hassell", 35.15551f, -89.98859f),
        new TargetLocation("BCLC", 35.15631f, -89.98866f),
        new TargetLocation("Buckman", 35.15254f, -89.98883f),
        new TargetLocation("Burrow", 35.15312f, -89.98901f),
        new TargetLocation("Clough", 35.15382f, -89.9888f),
        new TargetLocation("FJ", 35.15382f, -89.98925f),
        new TargetLocation("Kennedy", 35.15435f, -89.98986f),
        new TargetLocation("Library", 35.15528f, -89.98979f),
        new TargetLocation("McCoy", 35.15552f, -89.98722f),
        new TargetLocation("Ohlendorf", 35.15462f, -89.98952f),
        new TargetLocation("Rat", 35.15427f, -89.99049f),
        new TargetLocation("Rhodes Tower", 35.15442f, -89.98885f),
        new TargetLocation("Robertson", 35.15516f, -89.98857f),
    };

    public static string SendLocationsToFlask()
    {
        List<TargetLocation> locations = new List<TargetLocation>();
        foreach (TargetLocation location in targets)
        {
            locations.Add(new TargetLocation(location.name, location.latitude, location.longitude));
        }
        
        LocationListWrapper wrapper = new LocationListWrapper(locations);
        string json = JsonUtility.ToJson(wrapper);
        Debug.Log(json);
        return json;
    }

    public static IEnumerator UploadLocations(List<TargetLocation> locationList)
    {
        LocationListWrapper wrapper = new LocationListWrapper(locationList);
        string json = JsonUtility.ToJson(wrapper);

        using (UnityWebRequest request = new UnityWebRequest("http://your-server-url/upload_locations", "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Upload complete! Response: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Upload failed: " + request.error);
            }
        }
    }
}