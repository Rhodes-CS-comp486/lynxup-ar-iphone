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
        public string imageName; // The name of the reference image (must match the XR Reference Image Library)
        public string sceneName; // The name of the scene to load
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
            Debug.Log("Detected Image: " + updatedImage.referenceImage.name);

            // Check if the image name is in the list
            foreach (var pair in imageScenePairs)
            {
                if (updatedImage.referenceImage.name == pair.imageName)
                {
                    Debug.Log("Switching to scene: " + pair.sceneName);
                    StartCoroutine(SwitchSceneWithDelay(pair.sceneName));
                    return; // Stop further execution after scene change
                }
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
