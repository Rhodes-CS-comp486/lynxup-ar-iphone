using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    // void Start()
    // {
    //     // Get the Button component and add a listener for click event
    //     Button button = GetComponent<Button>();
    //     if (button != null)
    //     {
    //         button.onClick.AddListener(SwitchScene);
    //     }
    //     
    //     
    // }

    public void SwitchScene(string scene)
    {
        Debug.Log("hello world");
        SceneManager.LoadScene(scene);
    }
}
