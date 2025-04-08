using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class HassellSceneManager : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("HasOpenedHassell", 0) == 0)
        {
            PlayerPrefs.SetInt("HasOpenedHassell", 1);
            PlayerPrefs.SetInt("LockHassellSelected", 0);
            PlayerPrefs.SetInt("HassellSelected", 1);
            PlayerPrefs.Save();

            SceneManager.LoadSceneAsync("RhodesMap", LoadSceneMode.Additive).completed += OnRhodesMapLoaded;
        }
    }

    private void OnRhodesMapLoaded(AsyncOperation op)
    {
        Scene rhodesMap = SceneManager.GetSceneByName("RhodesMap");
        if (!rhodesMap.isLoaded) return;

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        GameObject lockObj = allObjects.FirstOrDefault(go => go.name == "LockHassell" && go.scene == rhodesMap);
        GameObject obj = allObjects.FirstOrDefault(go => go.name == "Hassell" && go.scene == rhodesMap);

        if (lockObj != null) lockObj.SetActive(false);
        if (obj != null) obj.SetActive(true);

        SceneManager.UnloadSceneAsync("RhodesMap");
    }
}