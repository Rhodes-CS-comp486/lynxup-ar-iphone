// ImageSceneSwitcher.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;

public class ImageSceneSwitcher : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    [System.Serializable]
    public class XPUpdate
    {
        public string user_id;
        public int xp;
    }
    [System.Serializable]
    public class ImageScenePair
    {
        public string imageName;
        public string sceneName;
    }

    [Header("Assign Image-Scene Pairs in Inspector")]
    public List<ImageScenePair> imageScenePairs = new List<ImageScenePair>();

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
            foreach (var pair in imageScenePairs)
            {
                if (updatedImage.referenceImage.name == pair.imageName)
                {
                    StartCoroutine(SwitchSceneWithDelay(pair.imageName, pair.sceneName));
                    // TODO: we'd also like to update XP for the user in the backend
                    StartCoroutine(UpdateXP(50));
                    return;
                }
            }
        }
    }

    IEnumerator SwitchSceneWithDelay(string imageName, string sceneName)
    {
        if (trackedImageManager != null)
            trackedImageManager.enabled = false;

        yield return new WaitForSeconds(1f);

        string sceneKey = "HasOpened" + sceneName;

        // Only allow switching scene if it's never been opened
        if (PlayerPrefs.GetInt(sceneKey, 0) == 1)
        {
            yield break;
        }

        PlayerPrefs.SetInt(sceneKey, 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(sceneName);
    }

    public void ReactivateARTracking()
    {
        if (trackedImageManager != null)
        {
            StartCoroutine(EnableARTracking());
        }
    }

    IEnumerator EnableARTracking()
    {
        yield return new WaitForSeconds(1f);
        trackedImageManager.enabled = true;
    }

    IEnumerator UpdateXP(int xp)
    {
        // TODO: error handling
        // if (string.IsNullOrEmpty(userInput))
        // {
        //     Debug.LogWarning("Input is empty!");
        //     yield break;
        // }

        // Create JSON from C# object
        XPUpdate data = new XPUpdate
        {
            user_id = UserSession.UserId,
            xp = xp 
            //password = inputFields.password.text;
            //fullname = inputFields.fullName.text,
            //location = inputFields.location.text
        };
        string jsonData = JsonUtility.ToJson(data);

        string apiURL = UserSession.BackendURL + "add_xp";
        UnityWebRequest request = new UnityWebRequest(apiURL, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Debug.Log("HTTP Response Code: " + request.responseCode);
        string requestBody = request.downloadHandler.text;
        
        //ImageSceneSwitcher.ServerResponse response = JsonUtility.FromJson<ImageSceneSwitcher.ServerResponse>(requestBody);
        
        if (request.responseCode == 200)
        {
            //UserSession.Username = response.username;
            //UserSession.UserId = response.id;
            //Debug.Log("Username: " + UserSession.Username);
            //Debug.Log("UserId: " + UserSession.UserId);
            Debug.Log("successful");
            //SceneManager.LoadScene("ar_scene2");
        }
        else
        {
            Debug.LogError("Error: " + request.downloadHandler.text);
        }
        // if (request.result == UnityWebRequest.Result.Success &&  request.responseCode == 200)
        // {
        //     Debug.Log("Response: " + request.downloadHandler.text);
        //     SceneManager.LoadScene("ar_scene");
        // }
        // else
        // {
        //     Debug.LogError("Error: " + request.error);
        // }

        //inputField.text = ""; // Optionally clear the input field after submission
        //UserSession.UserId = "honk";
        //userName.text = "";
        //fullName.text = "";
        //location.text = "";
    }

}
