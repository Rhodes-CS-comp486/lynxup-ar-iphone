using UnityEngine;

public class RhodesMapSelectionRestorer : MonoBehaviour
{
    public GameObject lockBriggsRef;
    public GameObject briggsRef;
    public GameObject lockBuckmanRef;
    public GameObject buckmanRef;
    public GameObject lockBurrowRef;
    public GameObject burrowRef;

    void Start()
    {
        if (lockBriggsRef != null)
            lockBriggsRef.SetActive(PlayerPrefs.GetInt("LockBriggsSelected", 1) == 1);

        if (briggsRef != null)
            briggsRef.SetActive(PlayerPrefs.GetInt("BriggsSelected", 0) == 1);

        if (lockBuckmanRef != null)
            lockBuckmanRef.SetActive(PlayerPrefs.GetInt("LockBuckmanSelected", 1) == 1);

        if (buckmanRef != null)
            buckmanRef.SetActive(PlayerPrefs.GetInt("BuckmanSelected", 0) == 1);

        if (lockBurrowRef != null)
            lockBurrowRef.SetActive(PlayerPrefs.GetInt("LockBurrowSelected", 1) == 1);

        if (burrowRef != null)
            burrowRef.SetActive(PlayerPrefs.GetInt("BurrowSelected", 0) == 1);
    }
}