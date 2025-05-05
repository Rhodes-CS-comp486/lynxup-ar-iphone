using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchToAccountCreation : MonoBehaviour
{
    [Header("Button that triggers scene switch")]
    public Button switchButton;

    private void Start()
    {
        if (switchButton != null)
        {
            switchButton.onClick.AddListener(SwitchScene);
        }
        else
        {
          
        }
    }

    private void SwitchScene()
    {
        SceneManager.LoadScene("AccountCreationUI");
    }
}