using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject popupPanel;  // Assign your UI panel
    public TMP_Text popupText;  // Assign the Text component
    public float popupDuration = 2f;

    public void ShowPopup(string message)
    {
        popupText.text = message;
        popupPanel.SetActive(true);
        Invoke("HidePopup", popupDuration);
    }

    void HidePopup()
    {
        popupPanel.SetActive(false);
    }
}