using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class CloughSceneManager : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("HasOpenedClough", 0) == 0)
        {
            PlayerPrefs.SetInt("HasOpenedClough", 1);
            PlayerPrefs.SetInt("LockCloughSelected", 0);
            PlayerPrefs.SetInt("CloughSelected", 1);
            PlayerPrefs.Save();

            SceneManager.LoadSceneAsync("RhodesMap", LoadSceneMode.Additive).completed += OnRhodesMapLoaded;
        }
    }

    private void OnRhodesMapLoaded(AsyncOperation op)
    {
        Scene rhodesMap = SceneManager.GetSceneByName("RhodesMap");
        if (!rhodesMap.isLoaded) return;

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        GameObject lockObj = allObjects.FirstOrDefault(go => go.name == "LockClough" && go.scene == rhodesMap);
        GameObject obj = allObjects.FirstOrDefault(go => go.name == "Clough" && go.scene == rhodesMap);

        if (lockObj != null) lockObj.SetActive(false);
        if (obj != null) obj.SetActive(true);

        SceneManager.UnloadSceneAsync("RhodesMap");
    }
}