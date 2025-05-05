// ImageSceneSwitcher.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;

public class ImageSceneSwitcher : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

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
}
