using UnityEngine;

public class RhodesMapSelectionRestorer : MonoBehaviour
{
    public GameObject lockBriggsRef;
    public GameObject briggsRef;
    public GameObject lockBuckmanRef;
    public GameObject buckmanRef;
    public GameObject lockBurrowRef;
    public GameObject burrowRef;

    public GameObject lockSouthwesternRef;
    public GameObject southwesternRef;
    public GameObject lockCloughRef;
    public GameObject cloughRef;
    public GameObject lockFJRef;
    public GameObject fJRef;
    public GameObject lockHassellRef;
    public GameObject hassellRef;
    public GameObject lockKennedyRef;
    public GameObject kennedyRef;
    public GameObject lockMcCoyRef;
    public GameObject mcCoyRef;
    public GameObject lockOhlendorfRef;
    public GameObject ohlendorfRef;
    public GameObject lockRobertsonRef;
    public GameObject robertsonRef;
    public GameObject lockRatRef;
    public GameObject RatRef;
    public GameObject lockRhodesTowerRef;
    public GameObject rhodesTowerRef;
    public GameObject lockLibraryRef;
    public GameObject libraryRef;
    public GameObject lockBCLCRef;
    public GameObject bCLCRef;

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

        if (lockSouthwesternRef != null)
            lockSouthwesternRef.SetActive(PlayerPrefs.GetInt("LockSouthwesternSelected", 1) == 1);
        if (southwesternRef != null)
            southwesternRef.SetActive(PlayerPrefs.GetInt("SouthwesternSelected", 0) == 1);

        if (lockCloughRef != null)
            lockCloughRef.SetActive(PlayerPrefs.GetInt("LockCloughSelected", 1) == 1);
        if (cloughRef != null)
            cloughRef.SetActive(PlayerPrefs.GetInt("CloughSelected", 0) == 1);

        if (lockFJRef != null)
            lockFJRef.SetActive(PlayerPrefs.GetInt("LockFJSelected", 1) == 1);
        if (fJRef != null)
            fJRef.SetActive(PlayerPrefs.GetInt("FJSelected", 0) == 1);

        if (lockHassellRef != null)
            lockHassellRef.SetActive(PlayerPrefs.GetInt("LockHassellSelected", 1) == 1);
        if (hassellRef != null)
            hassellRef.SetActive(PlayerPrefs.GetInt("HassellSelected", 0) == 1);

        if (lockKennedyRef != null)
            lockKennedyRef.SetActive(PlayerPrefs.GetInt("LockKennedySelected", 1) == 1);
        if (kennedyRef != null)
            kennedyRef.SetActive(PlayerPrefs.GetInt("KennedySelected", 0) == 1);

        if (lockMcCoyRef != null)
            lockMcCoyRef.SetActive(PlayerPrefs.GetInt("LockMcCoySelected", 1) == 1);
        if (mcCoyRef != null)
            mcCoyRef.SetActive(PlayerPrefs.GetInt("McCoySelected", 0) == 1);

        if (lockOhlendorfRef != null)
            lockOhlendorfRef.SetActive(PlayerPrefs.GetInt("LockOhlendorfSelected", 1) == 1);
        if (ohlendorfRef != null)
            ohlendorfRef.SetActive(PlayerPrefs.GetInt("OhlendorfSelected", 0) == 1);

        if (lockRobertsonRef != null)
            lockRobertsonRef.SetActive(PlayerPrefs.GetInt("LockRobertsonSelected", 1) == 1);
        if (robertsonRef != null)
            robertsonRef.SetActive(PlayerPrefs.GetInt("RobertsonSelected", 0) == 1);

        if (lockRatRef != null)
            lockRatRef.SetActive(PlayerPrefs.GetInt("LockRatSelected", 1) == 1);
        if (RatRef != null)
            RatRef.SetActive(PlayerPrefs.GetInt("RatSelected", 0) == 1);

        if (lockRhodesTowerRef != null)
            lockRhodesTowerRef.SetActive(PlayerPrefs.GetInt("LockRhodesTowerSelected", 1) == 1);
        if (rhodesTowerRef != null)
            rhodesTowerRef.SetActive(PlayerPrefs.GetInt("RhodesTowerSelected", 0) == 1);

        if (lockLibraryRef != null)
            lockLibraryRef.SetActive(PlayerPrefs.GetInt("LockLibrarySelected", 1) == 1);
        if (libraryRef != null)
            libraryRef.SetActive(PlayerPrefs.GetInt("LibrarySelected", 0) == 1);

        if (lockBCLCRef != null)
            lockBCLCRef.SetActive(PlayerPrefs.GetInt("LockBCLCSelected", 1) == 1);
        if (bCLCRef != null)
            bCLCRef.SetActive(PlayerPrefs.GetInt("BCLCSelected", 0) == 1);
    }
}
