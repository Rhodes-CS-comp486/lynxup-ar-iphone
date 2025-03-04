using System;
using System.Collections.Generic;
using ARMagicBar.Resources.Scripts.Debugging;
using ARMagicBar.Resources.Scripts.Gizmo;
using ARMagicBar.Resources.Scripts.PlacementBar;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ARMagicBar.Resources.Scripts.TransformLogic
{
    /// <summary>
    /// Handles Raycasting and setting transformable Object to selected
    /// </summary>
    public class TransformableObjectsSelectLogic : MonoBehaviour
    {
        // private GameObject selectedObject;
        private TransformableObject selectedObject;
        
        private Camera mainCam;

        public static TransformableObjectsSelectLogic Instance;
        public event Action OnDeselectAll;
        public event Action OnSelectObject;

        public event Action<GameObject> OnGizmoSelected;

        // public event Action<Vector3> OnGizmoMoved;  

        private bool isManipulating;
        private bool isDragging; 
        
        private Vector3 initialPosition;
        private Vector3 axis; 
        
        private bool disableTransformOptions;
        
        public bool DisableTransformOptions
        {
            get => disableTransformOptions;
            set => disableTransformOptions = value;
        }
        
        private void Awake()
        {
            Instance = this;
            mainCam = FindObjectOfType<Camera>();
        }

        public TransformableObject GetSelectedObject()
        {
            return selectedObject;
        }

        public void DeleteSelectedObject()
        {
            selectedObject.Delete();
        }
        
    
        void Update()
        {
            //If any object from the bar is selected 
            if(PlacementBarLogic.Instance.GetPlacementObject() != null) return;
            
            //If the player is currently manipulating a placed objects
            if(GlobalSelectState.Instance.GetTransformstate() ==
                             SelectState.manipulating) return;
            
            
            if(disableTransformOptions) return;
            
            
            CustomLog.Instance.InfoLog("Transf. object select logic , disable => " +  disableTransformOptions);

            
            
    #if UNITY_EDITOR
            //If not manipulating objects transform
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    CustomLog.Instance.InfoLog("UI Hit was recognized");
                    return;
                }
                TouchToRayCasting(Input.mousePosition);
            }
            
    #endif
    #if UNITY_IOS || UNITY_ANDROID
            if (Input.touchCount > 0 && Input.touchCount < 2 &&
                Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Touch touch = Input.GetTouch(0);
                
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                pointerData.position = touch.position;

                List<RaycastResult> results = new List<RaycastResult>();

                EventSystem.current.RaycastAll(pointerData, results);

                if (results.Count > 0) {
                    // We hit a UI element
                    Debug.Log("We hit an UI Element");
                    return;
                }
                TouchToRayCasting(touch.position);
            }
    #endif
        }
        //Shoot ray from the touch position 
        void TouchToRayCasting(Vector3 touch)
        {
            Ray ray = mainCam.ScreenPointToRay(touch);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                SelectObject(hit.collider.gameObject);
            }
            //Else, deselect all objects
            else 
            {
                OnDeselectAll?.Invoke();
                selectedObject = null;
            }
        }

        void SelectObject(GameObject objectThatWasHit)
        {
            CustomLog.Instance.InfoLog("SelectObject");

            //Select the specific Axis to move 
            if (isManipulating)
            {
                GizmoObject gizmoObject;
                if (objectThatWasHit.TryGetComponent(out gizmoObject))
                {
                    CustomLog.Instance.InfoLog("Gizmo was selected");
                    OnGizmoSelected?.Invoke(objectThatWasHit);
                }
                return;
            }
            
            CustomLog.Instance.InfoLog("SelectObject, Object that was hit" + 
                      objectThatWasHit.name);
            
            //Only one objects should be selected at a time
            TransformableObject obj;
            if (objectThatWasHit.GetComponentInParent<TransformableObject>())
            {
                OnDeselectAll?.Invoke();
                selectedObject = null;

                
                obj = objectThatWasHit.GetComponentInParent<TransformableObject>(); 
                selectedObject = obj;
                if (obj.GetSelected())
                {
                    obj.SetSelected(false);
                    return;
                }
                
                obj.SetSelected(true);
                OnSelectObject?.Invoke();
            }
            else
            {
                OnDeselectAll?.Invoke();
                selectedObject = null;
            }
        }
    }
}