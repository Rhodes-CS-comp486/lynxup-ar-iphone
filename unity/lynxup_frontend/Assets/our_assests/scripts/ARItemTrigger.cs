using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ARItemTrigger : MonoBehaviour
{
    public string itemName;
    private UIManager uiManager;

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("are we triggering anything here?");
        if (other.CompareTag("Player"))
        {
            // Test to verify that items can be scanned properly using this method.
            // Update: this is finally working omg!
            Debug.Log($"Scanned {itemName}!");

            if (uiManager != null)
                uiManager.ShowPopup("You collected: " + itemName);

            // gameObject.SetActive(true);
            // now we need to send to the backend an indication that 
            // we have successfully retrieved the item

            //gameObject.SetActive(false);
            Debug.Log($"Sending {itemName} to {UserSession.UserId}'s inventory");
            StartCoroutine(AddItemToInventory(itemName));
            
            gameObject.SetActive(false);
        }
    }
    
    IEnumerator AddItemToInventory(string imageName)
    {
        string userId = UserSession.UserId;
        string backendUrl = "http://127.0.0.1:5000/add_items";
        // Note: need to find some way to get the userId
        ItemScanToInventory.ScanData data = new ItemScanToInventory.ScanData { userId = userId, itemId = imageName };
        string jsonData = JsonUtility.ToJson(data);
        
        using (UnityWebRequest request = new UnityWebRequest(backendUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Item successfully added to inventory: " + imageName);
                //ShowScanSuccessMessage(imageName);
                //StartCoroutine(SwitchSceneWithDelay(sceneName));
            }
            else
            {
                Debug.LogError("Error sending scan data: " + request.error);
            }
        }
    }
 
    
}