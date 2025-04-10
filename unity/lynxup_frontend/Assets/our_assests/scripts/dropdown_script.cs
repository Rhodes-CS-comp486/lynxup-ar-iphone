using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TargetDropdownSelector : MonoBehaviour
{
    [Header("Dropdown UI Reference")]
    public TMP_Dropdown dropdown;

    [Header("List of Target Names")]
    public List<string> targetNames = new List<string>
    {
        "Briggs",
        "Southwestren",
        "Hassell",
        "BCLC",
        "Buckman",
        "Burrow",
        "Clough",
        "FJ",
        "Kennedy",
        "Library",
        "McCoy",
        "Ohlendorf",
        "Rat",
        "Rhodes Tower",
        "Robertson"
    };
    [System.Serializable]
    public class TargetSelectedEvent : UnityEvent<string> { }

    [Header("Event Called When Target Selected")]
    public TargetSelectedEvent onTargetSelected;

    void Start()
    {
        if (dropdown == null)
        {
            Debug.LogError("Dropdown not assigned!");
            return;
        }

        // Clear any existing options
        dropdown.ClearOptions();
        dropdown.AddOptions(targetNames);

        // Add listener to handle selection change
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void OnDropdownValueChanged(int index)
    {
        string selectedTarget = targetNames[index];
        Debug.Log("Selected target: " + selectedTarget);

        // Fire the custom event
        onTargetSelected.Invoke(selectedTarget);
    }
}