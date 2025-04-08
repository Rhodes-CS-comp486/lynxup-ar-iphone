using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class RobertsonSceneManager : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("HasOpenedRobertson", 0) == 0)
        {
            PlayerPrefs.SetInt("HasOpenedRobertson", 1);
            PlayerPrefs.SetInt("LockRobertsonSelected", 0);
            PlayerPrefs.SetInt("RobertsonSelected", 1);
            PlayerPrefs.Save();

            SceneManager.LoadSceneAsync("RhodesMap", LoadSceneMode.Additive).completed += OnRhodesMapLoaded;
        }
    }

    private void OnRhodesMapLoaded(AsyncOperation op)
    {
        Scene rhodesMap = SceneManager.GetSceneByName("RhodesMap");
        if (!rhodesMap.isLoaded) return;

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        GameObject lockObj = allObjects.FirstOrDefault(go => go.name == "LockRobertson" && go.scene == rhodesMap);
        GameObject obj = allObjects.FirstOrDefault(go => go.name == "Robertson" && go.scene == rhodesMap);

        if (lockObj != null) lockObj.SetActive(false);
        if (obj != null) obj.SetActive(true);

        SceneManager.UnloadSceneAsync("RhodesMap");
    }
}