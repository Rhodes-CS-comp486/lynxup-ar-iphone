using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class RhodesTowerSceneManager : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("HasOpenedRhodesTower", 0) == 0)
        {
            PlayerPrefs.SetInt("HasOpenedRhodesTower", 1);
            PlayerPrefs.SetInt("LockRhodesTowerSelected", 0);
            PlayerPrefs.SetInt("RhodesTowerSelected", 1);
            PlayerPrefs.Save();

            SceneManager.LoadSceneAsync("RhodesMap", LoadSceneMode.Additive).completed += OnRhodesMapLoaded;
        }
    }

    private void OnRhodesMapLoaded(AsyncOperation op)
    {
        Scene rhodesMap = SceneManager.GetSceneByName("RhodesMap");
        if (!rhodesMap.isLoaded) return;

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        GameObject lockObj = allObjects.FirstOrDefault(go => go.name == "LockRhodesTower" && go.scene == rhodesMap);
        GameObject obj = allObjects.FirstOrDefault(go => go.name == "RhodesTower" && go.scene == rhodesMap);

        if (lockObj != null) lockObj.SetActive(false);
        if (obj != null) obj.SetActive(true);

        SceneManager.UnloadSceneAsync("RhodesMap");
    }
}