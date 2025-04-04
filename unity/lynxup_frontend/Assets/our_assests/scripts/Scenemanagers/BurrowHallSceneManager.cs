using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class BurrowHallSceneManager : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("HasOpenedBurrowHall", 0) == 0)
        {
            PlayerPrefs.SetInt("HasOpenedBurrowHall", 1);
            PlayerPrefs.SetInt("LockBurrowSelected", 0);
            PlayerPrefs.SetInt("BurrowSelected", 1);
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

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        GameObject lockBurrow = allObjects.FirstOrDefault(go =>
            go.name == "LockBurrow" && go.scene == rhodesMap);

        GameObject burrow = allObjects.FirstOrDefault(go =>
            go.name == "Burrow" && go.scene == rhodesMap);

        if (lockBurrow != null)
            lockBurrow.SetActive(false);
        else
            Debug.LogWarning("LockBurrow not found in RhodesMap scene.");

        if (burrow != null)
            burrow.SetActive(true);
        else
            Debug.LogWarning("Burrow not found in RhodesMap scene.");

        SceneManager.UnloadSceneAsync("RhodesMap");
    }
}