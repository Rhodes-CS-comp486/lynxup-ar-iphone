using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class LibrarySceneManager : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("HasOpenedLibrary", 0) == 0)
        {
            PlayerPrefs.SetInt("HasOpenedLibrary", 1);
            PlayerPrefs.SetInt("LockLibrarySelected", 0);
            PlayerPrefs.SetInt("LibrarySelected", 1);
            PlayerPrefs.Save();

            SceneManager.LoadSceneAsync("RhodesMap", LoadSceneMode.Additive).completed += OnRhodesMapLoaded;
        }
    }

    private void OnRhodesMapLoaded(AsyncOperation op)
    {
        Scene rhodesMap = SceneManager.GetSceneByName("RhodesMap");
        if (!rhodesMap.isLoaded) return;

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        GameObject lockObj = allObjects.FirstOrDefault(go => go.name == "LockLibrary" && go.scene == rhodesMap);
        GameObject obj = allObjects.FirstOrDefault(go => go.name == "Library" && go.scene == rhodesMap);

        if (lockObj != null) lockObj.SetActive(false);
        if (obj != null) obj.SetActive(true);

        SceneManager.UnloadSceneAsync("RhodesMap");
    }
}