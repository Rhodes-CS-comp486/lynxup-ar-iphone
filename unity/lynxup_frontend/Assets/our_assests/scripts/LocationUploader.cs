using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class LocationUploader : MonoBehaviour
{

    public IEnumerator UploadLocations(List<Locations.TargetLocation> locationList)
    {
        Locations.LocationListWrapper wrapper = new Locations.LocationListWrapper(locationList);
        string json = JsonUtility.ToJson(wrapper);

        using (UnityWebRequest request = new UnityWebRequest("http://localhost:5000/push_locations", "POST"))
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
