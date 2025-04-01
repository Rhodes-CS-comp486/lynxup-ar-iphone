using UnityEngine;
using UnityEngine.EventSystems;

public class PinchZoomPan : MonoBehaviour
{
    [Header("Assign the RectTransform of your Image here")]
    public RectTransform target;

    [Header("Zoom Settings")]
    public float zoomSpeed = 0.01f;
    public float minZoom = 0.5f;
    public float maxZoom = 3f;

    private Vector2 lastTouchPos;
    private bool isPanning;

    void Update()
    {
        if (target == null)
        {
            Debug.LogWarning("PinchZoomPan: No target assigned.");
            return;
        }

        if (Input.touchCount == 2)
        {
            // Pinch to zoom
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 prevTouch0 = touch0.position - touch0.deltaPosition;
            Vector2 prevTouch1 = touch1.position - touch1.deltaPosition;

            float prevMagnitude = (prevTouch0 - prevTouch1).magnitude;
            float currMagnitude = (touch0.position - touch1.position).magnitude;

            float diff = currMagnitude - prevMagnitude;
            Zoom(diff * zoomSpeed);
        }
        else if (Input.touchCount == 1 && !IsTouchOverUI())
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPos = touch.position;
                isPanning = true;
            }
            else if (touch.phase == TouchPhase.Moved && isPanning)
            {
                Vector2 delta = touch.position - lastTouchPos;
                Pan(delta);
                lastTouchPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isPanning = false;
            }
        }
    }

    void Zoom(float increment)
    {
        float newScale = Mathf.Clamp(target.localScale.x + increment, minZoom, maxZoom);
        target.localScale = new Vector3(newScale, newScale, 1f);
    }

    void Pan(Vector2 delta)
    {
        target.anchoredPosition += delta;
    }

    bool IsTouchOverUI()
    {
        if (Input.touchCount > 0)
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);

        return false;
    }
}
