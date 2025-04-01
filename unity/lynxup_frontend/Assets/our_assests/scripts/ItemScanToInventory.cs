using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


public class ItemScanToInventory : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    [System.Serializable]
    public class ImageItemPair
    {
        public string imageName;
        public string itemName;
    }
    
    [System.Serializable]
    public class ScanData
    {
        public string userId;
        public string itemId;
    }
	
    //[Header("Assign Image-Scene Pairs in Inspector")]
    //public List<ImageScenePair> imageScenePairs = new List<ImageScenePair>();
    
    [Header("Image-Item Pairs")]
    public List<ImageItemPair> imageItemPairs = new List<ImageItemPair>();
    

    void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged += OnChanged;
        }
    }

    void OnDisable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged -= OnChanged;
        }
    }

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var updatedImage in eventArgs.updated)
        {
            Debug.Log("Detected Image: " + updatedImage.referenceImage.name);

            // Check if the image name is in the list
            // The name of the image will be the name of the item
            //TODO: need a separate function for interacting with backend
            foreach (var pair in imageItemPairs)
            {
                if (updatedImage.referenceImage.name == pair.imageName)
                {
                    StartCoroutine(AddItemToInventory(pair.imageName));
                    //Debug.Log("Switching to scene: " + pair.sceneName);
                    //StartCoroutine(SwitchSceneWithDelay(pair.sceneName));
                    return; // Stop further execution after scene change
                }
            }
        }
    }

    IEnumerator AddItemToInventory(string imageName)
    {
        string userId = UserSession.UserId;
        string backendUrl = "http://127.0.0.1:5000/add_items";
        // Note: need to find some way to get the userId
        ScanData data = new ScanData { userId = userId, itemId = imageName };
        string jsonData = JsonUtility.ToJson(data);
        
        using (UnityWebRequest request = new UnityWebRequest(backendUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Item successfully added to inventory: " + imageName);
                //ShowScanSuccessMessage(imageName);
                //StartCoroutine(SwitchSceneWithDelay(sceneName));
            }
            else
            {
                Debug.LogError("Error sending scan data: " + request.error);
            }
        }
    }
    IEnumerator SwitchSceneWithDelay(string sceneName)
    {
        // Temporarily disable AR tracking to avoid instant retriggering
        if (trackedImageManager != null)
        {
            trackedImageManager.enabled = false;
        }

        yield return new WaitForSeconds(1f); // Small delay to avoid image re-detection

        SceneManager.LoadScene(sceneName);
    }

    // Call this method when returning to the AR scene
    public void ReactivateARTracking()
    {
        if (trackedImageManager != null)
        {
            StartCoroutine(EnableARTracking());
        }
    }

    IEnumerator EnableARTracking()
    {
        yield return new WaitForSeconds(1f); // Wait before re-enabling tracking
        trackedImageManager.enabled = true;
    }
}
