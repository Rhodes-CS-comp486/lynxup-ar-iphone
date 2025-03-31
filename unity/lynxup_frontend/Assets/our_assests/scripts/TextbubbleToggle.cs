using UnityEngine;
using UnityEngine.EventSystems;

public class TextbubbleToggleUI : MonoBehaviour, IPointerClickHandler
{
    public GameObject textbubble;

    private void Start()
    {
        if (textbubble == null)
        {
            textbubble = transform.Find("Textbubble")?.gameObject;
            if (textbubble == null)
            {
                Debug.LogWarning("Textbubble child not found!");
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (textbubble != null)
        {
            // Toggle visibility
            textbubble.SetActive(!textbubble.activeSelf);
        }
    }
}