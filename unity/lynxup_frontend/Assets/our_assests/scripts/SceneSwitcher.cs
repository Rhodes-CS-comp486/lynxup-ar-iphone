using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    void Start()
    {
        // Get the Button component and add a listener for click event
        Button button = GetComponent<Button>();
        Debug.Log(button.name);
        if (button != null)
        {
            button.onClick.AddListener(SwitchScene);
        }
        
        
    }

    public void SwitchScene()
    {
        SceneManager.LoadScene("ar_scene");
    }
}
