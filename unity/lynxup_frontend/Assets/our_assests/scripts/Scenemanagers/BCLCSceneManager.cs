using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class BCLCSceneManager : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("HasOpenedBCLC", 0) == 0)
        {
            PlayerPrefs.SetInt("HasOpenedBCLC", 1);
            PlayerPrefs.SetInt("LockBCLCSelected", 0);
            PlayerPrefs.SetInt("BCLCSelected", 1);
            PlayerPrefs.Save();

            SceneManager.LoadSceneAsync("RhodesMap", LoadSceneMode.Additive).completed += OnRhodesMapLoaded;
        }
    }

    private void OnRhodesMapLoaded(AsyncOperation op)
    {
        Scene rhodesMap = SceneManager.GetSceneByName("RhodesMap");
        if (!rhodesMap.isLoaded) return;

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        GameObject lockObj = allObjects.FirstOrDefault(go => go.name == "LockBCLC" && go.scene == rhodesMap);
        GameObject obj = allObjects.FirstOrDefault(go => go.name == "BCLC" && go.scene == rhodesMap);

        if (lockObj != null) lockObj.SetActive(false);
        if (obj != null) obj.SetActive(true);

        SceneManager.UnloadSceneAsync("RhodesMap");
    }
}