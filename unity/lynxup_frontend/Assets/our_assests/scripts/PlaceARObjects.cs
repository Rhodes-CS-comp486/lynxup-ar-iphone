using UnityEngine;

public class PlaceARObjects : MonoBehaviour
{
    public GameObject itemPrefab;
    public Vector3 fixedPosition = new Vector3(0, 0, 2);

    void Start()
    {
        Instantiate(itemPrefab, fixedPosition, Quaternion.identity);
    }
    
}