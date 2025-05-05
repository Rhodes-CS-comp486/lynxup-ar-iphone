using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class handle_tracke_images : MonoBehaviour
{
    ARTrackedImageManager trackedImageManager;
    List<GameObject> ARObjects = new List<GameObject>();
    public GameObject[] ArPrefabs;
    
    private InventoryUIManager inventoryUIManager;

    void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        inventoryUIManager = FindObjectOfType<InventoryUIManager>(); // 🔥 Grab reference at start

    }

    void OnEnable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackablesChanged.AddListener(OnChanged);
        }
    }
    void OnDisable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackablesChanged.RemoveListener(OnChanged);
        }
    }

    void OnChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        Debug.Log("OnChanged");
        
        foreach (var updatedImage in eventArgs.updated)
        {
            // Handle updated event
            //Debug.Log("Image name is " + updatedImage.referenceImage.name);
            foreach (var arPrefab in ArPrefabs)
            {
                if(updatedImage.referenceImage.name == arPrefab.name)
                {
                    var newPrefab = Instantiate(arPrefab, updatedImage.transform);
                    ARObjects.Add(newPrefab);
                    if (inventoryUIManager != null)
                    {
                        inventoryUIManager.AddItem(arPrefab.name); // 🔥 Add Potion
                    }

                }
            }
            
            
        } 
    }
}
