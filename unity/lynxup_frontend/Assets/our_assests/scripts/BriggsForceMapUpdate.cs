using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;

public class BriggsForceMapUpdate : MonoBehaviour
{
    void Start()
    {
        // Always update the map when BriggsUI scene is opened
        SceneManager.LoadSceneAsync("RhodesMap", LoadSceneMode.Additive).completed += OnRhodesMapLoaded;
    }

    private void OnRhodesMapLoaded(AsyncOperation op)
    {
        StartCoroutine(ApplyBriggsMarkerChange());
    }

    private IEnumerator ApplyBriggsMarkerChange()
    {
        // Wait to ensure objects are fully loaded
        yield return new WaitForSeconds(0.5f);

        Scene rhodesMap = SceneManager.GetSceneByName("RhodesMap");

        if (!rhodesMap.isLoaded)
        {
            Debug.LogError("RhodesMap not loaded.");
            yield break;
        }

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        GameObject lockBriggs = allObjects.FirstOrDefault(go =>
            go.name == "LockBriggs" && go.scene == rhodesMap);
        GameObject briggs = allObjects.FirstOrDefault(go =>
            go.name == "Briggs" && go.scene == rhodesMap);

        if (lockBriggs != null)
            lockBriggs.SetActive(false);
        else
            Debug.LogWarning("LockBriggs not found.");

        if (briggs != null)
            briggs.SetActive(true);
        else
            Debug.LogWarning("Briggs not found.");

        SceneManager.UnloadSceneAsync("RhodesMap");
    }
}