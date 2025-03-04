using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class InputSubmitter : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button submitButton;
    private string apiUrl = "http://127.0.0.1:5000/add_user";

    [System.Serializable]
    class InputData
    {
        public string username;
    }

    void Start()
    {
        if (submitButton != null)
        {
            submitButton.onClick.AddListener(() => StartCoroutine(SendInput(inputField.text)));
        }
    }

    IEnumerator SendInput(string userInput)
    {
        if (string.IsNullOrEmpty(userInput))
        {
            Debug.LogWarning("Input is empty!");
            yield break;
        }

        // Create JSON from C# object
        InputData data = new InputData { username = userInput };
        string jsonData = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }

        inputField.text = ""; // Optionally clear the input field after submission
    }
}