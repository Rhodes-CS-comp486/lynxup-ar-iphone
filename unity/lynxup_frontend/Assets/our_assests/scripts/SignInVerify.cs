using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

public class SignInVerify : MonoBehaviour
{
    //public TMP_InputField inputField;
    public TMP_InputField userName;
    //public TMP_InputField password;
    public Button submitButton;
    //private string apiUrl = "http://127.0.0.1:5000/login";
    private string apiUrl = UserSession.BackendURL + "login";

    [System.Serializable]
    public class ServerResponse
    {
        public string username;
        public string id;
        
    }
    
    [System.Serializable]

    class InputFields
    {
        public TMP_InputField userName;
        //public TMP_InputField password;
    }
    class InputData
    {
        public string username;
        //public string password;
    }

    void Start()
    {
        if (submitButton != null)
        {
            InputFields fields = new InputFields();
            fields.userName = userName;
            submitButton.onClick.AddListener(() => StartCoroutine(SendInput(fields)));
        }
    }

    IEnumerator SendInput(InputFields inputFields)
    {
        // TODO: error handling
        // if (string.IsNullOrEmpty(userInput))
        // {
        //     Debug.LogWarning("Input is empty!");
        //     yield break;
        // }

        // Create JSON from C# object
        InputData data = new InputData
        {
            username = inputFields.userName.text,
            //password = inputFields.password.text;
            //fullname = inputFields.fullName.text,
            //location = inputFields.location.text
        };
        string jsonData = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Debug.Log("HTTP Response Code: " + request.responseCode);
        string requestBody = request.downloadHandler.text;
        
        ServerResponse response = JsonUtility.FromJson<ServerResponse>(requestBody);
        
        if (request.responseCode == 200)
        {
            UserSession.Username = response.username;
            UserSession.UserId = response.id;
            Debug.Log("Username: " + UserSession.Username);
            Debug.Log("UserId: " + UserSession.UserId);
            SceneManager.LoadScene("ar_scene");
        }
        else
        {
            Debug.LogError("Error: " + request.downloadHandler.text);
        }
        // if (request.result == UnityWebRequest.Result.Success &&  request.responseCode == 200)
        // {
        //     Debug.Log("Response: " + request.downloadHandler.text);
        //     SceneManager.LoadScene("ar_scene");
        // }
        // else
        // {
        //     Debug.LogError("Error: " + request.error);
        // }

        //inputField.text = ""; // Optionally clear the input field after submission
        //UserSession.UserId = "honk";
        userName.text = "";
        //fullName.text = "";
        //location.text = "";
    }
}