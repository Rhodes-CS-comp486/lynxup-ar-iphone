using UnityEngine;

public class NorthPointingArrow : MonoBehaviour
{
    void Start()
    {
        // Start location and compass services
        Input.location.Start();
        Input.compass.enabled = true;
    }

    void Update()
    {
        if (Input.compass.enabled)
        {
            // Get the current heading (in degrees) from magnetic north
            float magneticHeading = Input.compass.magneticHeading;

            // Optional: Correct for true north if GPS data is available
            float trueHeading = Input.compass.trueHeading;
            Debug.Log("Compass working: " + trueHeading);

            // Apply rotation to the arrow to point north
            transform.localRotation = Quaternion.Euler(0, -trueHeading, 0);
        }
    }
}