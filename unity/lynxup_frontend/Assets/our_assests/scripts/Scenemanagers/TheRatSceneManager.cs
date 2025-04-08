using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class TheRatSceneManager : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("HasOpenedTheRat", 0) == 0)
        {
            PlayerPrefs.SetInt("HasOpenedTheRat", 1);
            PlayerPrefs.SetInt("LockTheRatSelected", 0);
            PlayerPrefs.SetInt("TheRatSelected", 1);
            PlayerPrefs.Save();

            SceneManager.LoadSceneAsync("RhodesMap", LoadSceneMode.Additive).completed += OnRhodesMapLoaded;
        }
    }

    private void OnRhodesMapLoaded(AsyncOperation op)
    {
        Scene rhodesMap = SceneManager.GetSceneByName("RhodesMap");
        if (!rhodesMap.isLoaded) return;

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        GameObject lockObj = allObjects.FirstOrDefault(go => go.name == "LockTheRat" && go.scene == rhodesMap);
        GameObject obj = allObjects.FirstOrDefault(go => go.name == "TheRat" && go.scene == rhodesMap);

        if (lockObj != null) lockObj.SetActive(false);
        if (obj != null) obj.SetActive(true);

        SceneManager.UnloadSceneAsync("RhodesMap");
    }
}