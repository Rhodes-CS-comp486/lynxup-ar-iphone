using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class FJSceneManager : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("HasOpenedFJ", 0) == 0)
        {
            PlayerPrefs.SetInt("HasOpenedFJ", 1);
            PlayerPrefs.SetInt("LockFJSelected", 0);
            PlayerPrefs.SetInt("FJSelected", 1);
            PlayerPrefs.Save();

            SceneManager.LoadSceneAsync("RhodesMap", LoadSceneMode.Additive).completed += OnRhodesMapLoaded;
        }
    }

    private void OnRhodesMapLoaded(AsyncOperation op)
    {
        Scene rhodesMap = SceneManager.GetSceneByName("RhodesMap");
        if (!rhodesMap.isLoaded) return;

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        GameObject lockObj = allObjects.FirstOrDefault(go => go.name == "LockFJ" && go.scene == rhodesMap);
        GameObject obj = allObjects.FirstOrDefault(go => go.name == "FJ" && go.scene == rhodesMap);

        if (lockObj != null) lockObj.SetActive(false);
        if (obj != null) obj.SetActive(true);

        SceneManager.UnloadSceneAsync("RhodesMap");
    }
}