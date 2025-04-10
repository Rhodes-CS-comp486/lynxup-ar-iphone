using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchtoAR : MonoBehaviour
{
    // This function can be linked to a UI Button's OnClick() event
    public void SwitchToARScene()
    {
        SceneManager.LoadScene("AR_Scene");
    }
}

