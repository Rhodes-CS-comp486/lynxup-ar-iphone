using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class McCoySceneManager : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("HasOpenedMcCoy", 0) == 0)
        {
            PlayerPrefs.SetInt("HasOpenedMcCoy", 1);
            PlayerPrefs.SetInt("LockMcCoySelected", 0);
            PlayerPrefs.SetInt("McCoySelected", 1);
            PlayerPrefs.Save();

            SceneManager.LoadSceneAsync("RhodesMap", LoadSceneMode.Additive).completed += OnRhodesMapLoaded;
        }
    }

    private void OnRhodesMapLoaded(AsyncOperation op)
    {
        Scene rhodesMap = SceneManager.GetSceneByName("RhodesMap");
        if (!rhodesMap.isLoaded) return;

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        GameObject lockObj = allObjects.FirstOrDefault(go => go.name == "LockMcCoy" && go.scene == rhodesMap);
        GameObject obj = allObjects.FirstOrDefault(go => go.name == "McCoy" && go.scene == rhodesMap);

        if (lockObj != null) lockObj.SetActive(false);
        if (obj != null) obj.SetActive(true);

        SceneManager.UnloadSceneAsync("RhodesMap");
    }
}