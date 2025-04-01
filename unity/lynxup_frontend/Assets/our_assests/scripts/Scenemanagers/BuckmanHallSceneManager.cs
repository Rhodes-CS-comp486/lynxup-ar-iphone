using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class BuckmanHallSceneManager : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("HasOpenedBuckmanHall", 0) == 0)
        {
            PlayerPrefs.SetInt("HasOpenedBuckmanHall", 1);
            PlayerPrefs.SetInt("LockBuckmanSelected", 0);
            PlayerPrefs.SetInt("BuckmanSelected", 1);
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

        GameObject lockBuckman = allObjects.FirstOrDefault(go =>
            go.name == "LockBuckman" && go.scene == rhodesMap);

        GameObject buckman = allObjects.FirstOrDefault(go =>
            go.name == "Buckman" && go.scene == rhodesMap);

        if (lockBuckman != null)
            lockBuckman.SetActive(false);
        else
            Debug.LogWarning("LockBuckman not found in RhodesMap scene.");

        if (buckman != null)
            buckman.SetActive(true);
        else
            Debug.LogWarning("Buckman not found in RhodesMap scene.");

        SceneManager.UnloadSceneAsync("RhodesMap");
    }
}