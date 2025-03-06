using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

public class InputSubmitter : MonoBehaviour
{
    //public TMP_InputField inputField;
    public TMP_InputField userName;
    public TMP_InputField location;
    public TMP_InputField fullName;
    public Button submitButton;
    private string apiUrl = "http://127.0.0.1:5000/add_user";

    [System.Serializable]

    class InputFields
    {
        public TMP_InputField userName;
        public TMP_InputField fullName;
        public TMP_InputField location;
    }
    class InputData
    {
        public string username;
        public string fullname;
        public string location;
    }

    void Start()
    {
        if (submitButton != null)
        {
            InputFields fields = new InputFields();
            fields.userName = userName;
            fields.fullName = fullName;
            fields.location = location;
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
            fullname = inputFields.fullName.text,
            location = inputFields.location.text
        };
        string jsonData = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            SceneManager.LoadScene("UI");
        }
        else
        {
            Debug.LogError("Error: " + request.downloadHandler.text);
        }

        //inputField.text = ""; // Optionally clear the input field after submission
        userName.text = "";
        fullName.text = "";
        location.text = "";
    }
}