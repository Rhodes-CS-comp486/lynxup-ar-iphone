using UnityEngine;
using System.Collections;

public class TestLocationService : MonoBehaviour
{
    private float _latitude = 0f;
    private float _longitude = 0f;

    void Start()
    {
        // Start the location coroutine automatically on start
        StartCoroutine(LocationCoroutine());
    }

    void Update()
    {
        // Continuously log latitude and longitude if location is enabled
        if (UnityEngine.Input.location.status == LocationServiceStatus.Running)
        {
            _latitude = UnityEngine.Input.location.lastData.latitude;
            _longitude = UnityEngine.Input.location.lastData.longitude;
            Debug.Log($"Latitude: {_latitude}, Longitude: {_longitude}");
        }
    }

    IEnumerator LocationCoroutine()
    {
#if UNITY_EDITOR
        Debug.Log("Running in Unity Editor. Location services not available.");

#elif UNITY_IOS
        if (!UnityEngine.Input.location.isEnabledByUser)
        {
            Debug.LogWarning("iOS: Location services not enabled by the user.");
            yield break;
        }
#endif

        // Start the location service
        UnityEngine.Input.location.Start(500f, 500f);

        // Wait for service initialization
        int maxWait = 15;
        while (UnityEngine.Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            maxWait--;
        }

        // Extra wait for Editor in case of issues
#if UNITY_EDITOR
        int editorMaxWait = 15;
        while (UnityEngine.Input.location.status == LocationServiceStatus.Stopped && editorMaxWait > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            editorMaxWait--;
        }
#endif

        // Timed out
        if (maxWait < 1)
        {
            Debug.LogError("Timed out waiting for location service.");
            yield break;
        }

        // Failed to start service
        if (UnityEngine.Input.location.status != LocationServiceStatus.Running)
        {
            Debug.LogError($"Unable to determine device location. Status: {UnityEngine.Input.location.status}");
            yield break;
        }

        // Successfully obtained location
        Debug.Log("Location service is running.");
        _latitude = UnityEngine.Input.location.lastData.latitude;
        _longitude = UnityEngine.Input.location.lastData.longitude;

        Debug.Log($"Initial Location: Latitude = {_latitude}, Longitude = {_longitude}");

        // You can do something with the location here
        HandleLocationSuccess(_latitude, _longitude);

        // Stop service after getting location to save battery
        UnityEngine.Input.location.Stop();
    }

    // Handle successful location retrieval
    private void HandleLocationSuccess(float latitude, float longitude)
    {
        Debug.Log($"Success! Location found: Latitude = {latitude}, Longitude = {longitude}");
        // Add your logic here, e.g., send to server or update UI
    }
}
