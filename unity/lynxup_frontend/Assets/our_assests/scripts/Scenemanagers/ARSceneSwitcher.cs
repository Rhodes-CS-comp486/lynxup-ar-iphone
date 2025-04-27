using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
//using Unity.Tutorials.Core.Editor;

public class ARSceneSwitcher : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(VerifyLocationIsEnabled());
        SceneManager.LoadScene("ar_scene");
    }

    IEnumerator VerifyLocationIsEnabled()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location services not enabled");
            yield break;
        }

        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (Input.location.status != LocationServiceStatus.Running)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }

        Debug.Log("Location: " + Input.location.lastData.latitude + ", " + Input.location.lastData.longitude);
    }
}