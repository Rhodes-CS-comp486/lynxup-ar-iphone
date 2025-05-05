using UnityEngine;
using System.Collections;

public class AutoHide : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(HideAfterDelay());
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(5f);

        // Hide all renderers
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }

        // Optionally disable any colliders too
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
    }
}
