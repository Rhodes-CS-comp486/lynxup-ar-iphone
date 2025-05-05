using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;  // <-- IMPORTANT: Add this for TMP

public class SwitchToARSceneWithPassword : MonoBehaviour
{
    [Header("UI References")]
    public Button yourButton;
    public TMP_InputField passwordInputField;  // <-- Use TMP_InputField here
    public GameObject errorImage;

    void Start()
    {
        if (yourButton != null && passwordInputField != null && errorImage != null)
        {
            yourButton.onClick.AddListener(CheckPasswordAndSwitch);
            errorImage.SetActive(false); // Hide error at start
        }
        else
        {
            Debug.LogError("Assign all UI elements (Button, InputField, Error Image) in the Inspector!");
        }
    }

    void CheckPasswordAndSwitch()
    {
        string enteredPassword = passwordInputField.text.Trim();

        if (enteredPassword == "test")
        {
            SceneManager.LoadScene("ar_scene");
        }
        else
        {
            errorImage.SetActive(true);
        }
    }
}