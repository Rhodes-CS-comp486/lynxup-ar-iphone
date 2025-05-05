using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class SouthwesternSceneManager : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("HasOpenedSouthwestern", 0) == 0)
        {
            PlayerPrefs.SetInt("HasOpenedSouthwestern", 1);
            PlayerPrefs.SetInt("LockSouthwesternSelected", 0);
            PlayerPrefs.SetInt("SouthwesternSelected", 1);
            PlayerPrefs.Save();

            SceneManager.LoadSceneAsync("RhodesMap", LoadSceneMode.Additive).completed += OnRhodesMapLoaded;
        }
    }

    private void OnRhodesMapLoaded(AsyncOperation op)
    {
        Scene rhodesMap = SceneManager.GetSceneByName("RhodesMap");
        if (!rhodesMap.isLoaded) return;

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        GameObject lockObj = allObjects.FirstOrDefault(go => go.name == "LockSouthwestern" && go.scene == rhodesMap);
        GameObject obj = allObjects.FirstOrDefault(go => go.name == "Southwestern" && go.scene == rhodesMap);

        if (lockObj != null) lockObj.SetActive(false);
        if (obj != null) obj.SetActive(true);

        SceneManager.UnloadSceneAsync("RhodesMap");
    }
}