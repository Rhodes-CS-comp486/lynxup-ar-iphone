using Mono.Cecil.Cil;
using UnityEngine;
public class ARItemTrigger : MonoBehaviour
{
    public string itemName;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("are we triggering anything here?");
        if (other.CompareTag("Player"))
        {
            // Test to verify that items can be scanned properly using this method.
            Debug.Log($"Scanned {itemName}!");
        }
    }
}