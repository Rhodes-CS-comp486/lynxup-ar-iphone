using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class GPSDirectionArrow : MonoBehaviour
{
    [System.Serializable]
    public class TargetLocation
    {
        public string name;
        public float latitude;
        public float longitude;

        public TargetLocation(string name, float latitude, float longitude)
        {
            this.name = name;
            this.latitude = latitude;
            this.longitude = longitude;
        }
    }
    
    [Header("List of Targets")]
    public List<TargetLocation> targets = new List<TargetLocation>
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
    
    [Header("UI Elements")]
    public TMP_Text distanceText;
    public TMP_Text arrivalText;
    public TMP_Text targetNameText;
    public Button nextTargetButton;

    [Header("Settings")]
    public float arrivalThreshold = 5f;

    [Header("Runtime Info")]
    private int currentTargetIndex = 0;
    private float distanceToTarget;
    private bool hasArrived = false;

    private void Start()
    {
        PrintTargetListForHardcoding();
        
        Input.location.Start();
        Input.compass.enabled = true;

        if (arrivalText != null)
            arrivalText.gameObject.SetActive(false);

        if (nextTargetButton != null)
            nextTargetButton.onClick.AddListener(SwitchToNextTarget);

        UpdateTargetUI();
        
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

        currentTargetIndex = index;
        UpdateTargetUI();

    }
    public void PrintTargetListForHardcoding()
    {
        string output = "";
        foreach (var target in targets)
        {
            output += $"new TargetLocation(\"{target.name}\", {target.latitude}f, {target.longitude}f),\n";
        }
        Debug.Log(output);
    }

    public void SetTargetByName(string name)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (string.Equals(targets[i].name, name))
            {
                currentTargetIndex = i;
                hasArrived = false;

                if (arrivalText != null)
                    arrivalText.gameObject.SetActive(false);

                UpdateTargetUI();
                Debug.Log("Target switched to: " + name);
                return;
            }
        }

        Debug.LogWarning("Target not found: " + name);
    }

    
    private void Update()
    {
        if (Input.location.status == LocationServiceStatus.Running && Input.compass.enabled && targets.Count > 0)
        {
            float currentLat = Input.location.lastData.latitude;
            float currentLon = Input.location.lastData.longitude;

            var target = targets[currentTargetIndex];

            float bearingToTarget = CalculateBearing(currentLat, currentLon, target.latitude, target.longitude);
            float deviceHeading = Input.compass.trueHeading;

            float direction = bearingToTarget - deviceHeading;
            transform.localRotation = Quaternion.Euler(0, direction, 0);

            distanceToTarget = CalculateDistance(currentLat, currentLon, target.latitude, target.longitude);

            if (distanceText != null)
                distanceText.text = distanceToTarget.ToString("F1") + " m";

            if (!hasArrived && distanceToTarget <= arrivalThreshold)
            {
                hasArrived = true;
                if (arrivalText != null)
                    arrivalText.gameObject.SetActive(true);
            }
        }
    }

    public void SwitchToNextTarget()
    {
        if (targets.Count == 0) return;

        currentTargetIndex = (currentTargetIndex + 1) % targets.Count;
        hasArrived = false;

        if (arrivalText != null)
            arrivalText.gameObject.SetActive(false);

        UpdateTargetUI();
    }

    private void UpdateTargetUI()
    {
        if (targetNameText != null && targets.Count > 0)
        {
            targetNameText.text = "Target: " + targets[currentTargetIndex].name;
        }
    }

    private float CalculateBearing(float lat1, float lon1, float lat2, float lon2)
    {
        float degToRad = Mathf.Deg2Rad;
        float radToDeg = Mathf.Rad2Deg;

        float dLon = (lon2 - lon1) * degToRad;
        lat1 *= degToRad;
        lat2 *= degToRad;

        float y = Mathf.Sin(dLon) * Mathf.Cos(lat2);
        float x = Mathf.Cos(lat1) * Mathf.Sin(lat2) -
                  Mathf.Sin(lat1) * Mathf.Cos(lat2) * Mathf.Cos(dLon);

        float bearing = Mathf.Atan2(y, x) * radToDeg;
        return (bearing + 360f) % 360f;
    }

    private float CalculateDistance(float lat1, float lon1, float lat2, float lon2)
    {
        float earthRadius = 6371000f; // meters
        float dLat = Mathf.Deg2Rad * (lat2 - lat1);
        float dLon = Mathf.Deg2Rad * (lon2 - lon1);

        lat1 = Mathf.Deg2Rad * lat1;
        lat2 = Mathf.Deg2Rad * lat2;

        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
                  Mathf.Cos(lat1) * Mathf.Cos(lat2) *
                  Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);

        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        return earthRadius * c;
    }
}