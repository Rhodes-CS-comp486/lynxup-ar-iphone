using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class BriggsUISceneManager : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("HasOpenedBriggsUI", 0) == 0)
        {
            PlayerPrefs.SetInt("HasOpenedBriggsUI", 1);
            PlayerPrefs.SetInt("LockBriggsSelected", 0);
            PlayerPrefs.SetInt("BriggsSelected", 1);
            PlayerPrefs.Save();

            SceneManager.LoadSceneAsync("RhodesMap", LoadSceneMode.Additive).completed += OnRhodesMapLoaded;
        }
    }

    private void OnRhodesMapLoaded(AsyncOperation op)
    {
        Scene rhodesMap = SceneManager.GetSceneByName("RhodesMap");

        if (!rhodesMap.isLoaded)
        {
            Debug.LogError("RhodesMap scene not loaded.");
            return;
        }

        // Find all GameObjects including inactive ones
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        GameObject lockBriggs = allObjects.FirstOrDefault(go =>
            go.name == "LockBriggs" && go.scene == rhodesMap);

        GameObject briggs = allObjects.FirstOrDefault(go =>
            go.name == "Briggs" && go.scene == rhodesMap);

        if (lockBriggs != null)
        {
            lockBriggs.SetActive(false);
        }
        else
        {
            Debug.LogWarning("LockBriggs not found in RhodesMap scene.");
        }

        if (briggs != null)
        {
            briggs.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Briggs not found in RhodesMap scene.");
        }

        // Optional: unload again
        SceneManager.UnloadSceneAsync("RhodesMap");
    }
}