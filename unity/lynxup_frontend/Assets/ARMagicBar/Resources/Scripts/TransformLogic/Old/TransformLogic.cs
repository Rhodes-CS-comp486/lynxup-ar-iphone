using System;
using System.Collections.Generic;
using ARMagicBar.Resources.Scripts.Debugging;
using ARMagicBar.Resources.Scripts.Gizmo;
using ARMagicBar.Resources.Scripts.GizmoUI;
using ARMagicBar.Resources.Scripts.PlacementBar;
using UnityEngine;

namespace ARMagicBar.Resources.Scripts.TransformLogic.Old
{
    public class TransformLogic : MonoBehaviour
    {
        private Camera mainCam;

        [SerializeField] private bool enableLocalSpace = true;
        [SerializeField] private bool showDebugPlanes = false;

        [SerializeField] private List<GameObject> RotateObjects;
        [SerializeField] private List<GameObject> MoveObjects; 
        
        [SerializeField] private PlacementObjectVisual.PlacementObjectVisual _placementObjectVisual;
        
        [SerializeField] private GameObject X;
        [SerializeField] private GameObject Y;
        [SerializeField] private GameObject Z;
        [SerializeField] private GameObject debugPlane;

        [SerializeField] private GameObject[] lockScaleObjects;

        private float rotatesensitivity = .005f;
       private const float SCALESENSITIVITY = .0085f;
       private const float MOVESENSITIVITY = .5f;
        
        private bool isDragging;
        private bool lockRotationStartingPoint;

        private SelectedAxis _selectedAxis;
        private Vector3 axisDirection;
        private Vector3 rotationDirection;
        
        private Vector3 initialWorldHitPosition;
        
        private Vector2 lastScreenPosition;

        //transform.forward
        private Vector3 lookDirection;

        private Quaternion initialRotation;
        private List<(GameObject goj, Quaternion initialRotation)> transformObjectsInitial = new();

        // private ManageTransformGizmoTypeVisuals _manageTransformGizmoTypeVisuals;
        
        private void Start()
        {
            mainCam = GameObject.FindObjectOfType<Camera>();
            _selectedAxis = SelectedAxis.none;
            
            // _manageTransformGizmoTypeVisuals = FindObjectOfType<ManageTransformGizmoTypeVisuals>();
            GizmHolderUI.Instance.resetTransformButtonToggled += OnresetTransformButtonToggled;
            ARPlacementPlaneMesh.Instance.OnSpawnObject += ResetAxis;


            _placementObjectVisual = GetComponentInChildren<PlacementObjectVisual.PlacementObjectVisual>();
            
            initialRotation = transform.localRotation;
            
            axisDirection = transform.forward; // Default to forward if no axis is selected
        }

        private void OnDisable()
        {
            GizmHolderUI.Instance.resetTransformButtonToggled -= OnresetTransformButtonToggled;
            ARPlacementPlaneMesh.Instance.OnSpawnObject -= ResetAxis;
        }


        private void OnresetTransformButtonToggled()
        {
            ResetRotation();
        }
        
        private void ResetRotation()
        {
            transform.rotation = initialRotation;
        }

        private void ResetScale()
        {
            transform.localScale = Vector3.one; 
        }


        private void Update()
        {
#if UNITY_EDITOR
            if(GlobalSelectState.Instance.GetTransformstate()
               != SelectState.manipulating) return;

            if (Input.GetMouseButton(0) && isDragging)
            {
                PerformDrag(Input.mousePosition);
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                StartDrag(Input.mousePosition);
                return;
            }

            if (Input.GetMouseButtonUp(0))
            {
                ResetAxis();
            }
#endif
#if UNITY_IOS || UNITY_ANDROID
            if(GlobalSelectState.Instance.GetTransformstate()
               != SelectState.manipulating) return;

            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved && isDragging)
                { 
                    PerformDrag(Input.GetTouch(0).position);
                }

                if (Input.GetTouch(0).phase == TouchPhase.Began )
                {
                    StartDrag(Input.GetTouch(0).position);
                    return;
                }

                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    ResetAxis();
                }
            }
#endif
        }
        
        private Plane dragPlane;
        private GameObject visualPlane; 

        private void ResetAxis(TransformableObject obj)
        {
            ResetAxis();
        }

        private void ResetAxis()
        {
            _selectedAxis = SelectedAxis.none;
            axisDirection = Vector3.zero;
            isDragging = false;
            // lockRotationStartingPoint = false;

            lockDirection = false;
            
            if(showDebugPlanes)
                ResetDebugplane();
        }

        void ResetDebugplane()
        {
            if (visualPlane) // Destroy the visual plane on reset
            {
                Destroy(visualPlane);
                visualPlane = null;
            }
        }
        
        private void StartDrag(Vector2 screenPos)
        {
            Ray ray = mainCam.ScreenPointToRay(screenPos);
            RaycastHit[] hits = new RaycastHit[]{};
            hits = Physics.RaycastAll(ray, Mathf.Infinity);

            foreach (var rcHit in hits)
            {
                if (rcHit.collider.gameObject.GetComponent<GizmoObject>() || 
                    rcHit.collider.GetComponentInParent<GizmoObject>())
                {
                    SetSelectedAxis(rcHit.collider.gameObject);
                    
                    //Set the initial point of the object and the point where the click raycast hit the gizmo object
                    
                    CustomLog.Instance.InfoLog("Setting initial values:");
                    initialWorldHitPosition = rcHit.point;
                    isDragging = true;
                    lastScreenPosition = screenPos;

                    var selectedObject = transform.gameObject;
                        // TransformableObjectsSelectLogic.Instance.GetSelectedObject();

                    Vector3 inNormal;

                    if ( _selectedAxis == SelectedAxis.X || _selectedAxis == SelectedAxis.Z)
                    {
                        inNormal = -selectedObject.transform.up + new Vector3(0, 0, 0f); 
                    }
                    else
                    {
                        inNormal = mainCam.transform.forward;
                    }
                    
                    dragPlane = new Plane(
                        inPoint: rcHit.collider.transform.position,
                        inNormal: inNormal
                        );
                    
                    if (isDragging && showDebugPlanes)
                    {
                         
                         visualPlane = Instantiate(debugPlane,
                             dragPlane.ClosestPointOnPlane(selectedObject.transform.position),
                             Quaternion.LookRotation(dragPlane.normal));

                         visualPlane.transform.localScale = new Vector3(5f, 5f, 5f);
                    }
                    
                    break;
                }
            }
        }

        
        private void PerformDrag(Vector2 screenPos)
        {
            // Determine the current operation based on the transform type.
            switch (GlobalTransformState._Instance.GetTransformState)
            {
                case TransformState.Move:
                    ApplyMovement(screenPos);
                    break;
                case TransformState.Rotate:
                    ApplyRotation(screenPos);
                    break;
                case TransformState.Scale:
                    ApplyScale(screenPos);
                    break;
                case TransformState.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            
        }
        
        void ApplyMovement(Vector2 screenPos)
        {
            Ray ray = mainCam.ScreenPointToRay(screenPos);
            if (dragPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                Vector3 movementVector = hitPoint - initialWorldHitPosition;

                // Project the movement vector onto the selected axis
                switch (_selectedAxis)
                {
                    case SelectedAxis.X:
                        movementVector = Vector3.Project(movementVector, transform.right);
                        break;
                    case SelectedAxis.Y:
                        movementVector = Vector3.Project(movementVector, transform.up);
                        break;
                    case SelectedAxis.Z:
                        movementVector = Vector3.Project(movementVector, transform.forward);
                        break;
                    case SelectedAxis.none:
                        return; // Do nothing if no axis is selected
                }

                // Apply the movement to the transform position
                transform.position += movementVector;
                initialWorldHitPosition = hitPoint; // Update the initial position for continuous tracking
            }
        }
        void ApplyScale(Vector2 screenPos)
        {
            CustomLog.Instance.InfoLog("Applying Scale!");
            
            // Calculate the screen delta.
            Vector2 screenDelta = screenPos - lastScreenPosition;

            // Use vertical movement for scaling. You might adjust the sensitivity based on your needs.
            float scaleDelta = screenDelta.y * SCALESENSITIVITY;

            // Convert this to a vector, assuming uniform scaling for simplicity.
            Vector3 deltaScale = new Vector3(scaleDelta, scaleDelta, scaleDelta);

            // Apply this delta scale.
            TransformableObject selectedObject = SelectObjectsLogic.Instance.GetSelectedObject();
            selectedObject.ScaleObject(deltaScale);
            
            // Update for the next frame.
            lastScreenPosition = screenPos;
        }
        
        //Find out if the gizmo was clicked / hit behind the origin of the object, relatively to the camera
        //If yes, inverse the direction of movement. 
        bool CheckIfPointIsBehindObject()
        {
            //Get the Vector that points from the object to the camera
            Vector3 pointToCamVector3 = mainCam.transform.position - transform.position;
            pointToCamVector3.Normalize();
            
            //Get the Vector that points from the object to the hitpoint on the gizmo
            //Initial WorldHit => Position where raycaast from click hit the gizmo -> is correct
            Vector3 pointToWorldHitPosition = initialWorldHitPosition - transform.position;
            pointToWorldHitPosition.Normalize();
            
            //Find out if the point to Cam points in the same direction as the point to world hit
            float dotProduct = Vector3.Dot(pointToCamVector3, pointToWorldHitPosition);
            
            bool dotProductSmaller = dotProduct < .4; 
            CustomLog.Instance.InfoLog("Point is behind object Dot Product: " + dotProduct + " ergo: " + dotProductSmaller);

            //Check for a ~45 Degree angle towards the camera 1
            return dotProduct < .4;
        }
        
        private bool lockDirection = false;
        private bool reverseYScreenDelta; 
        private Vector2 lastScreenDelta;
        
        
        
        void ApplyRotation(Vector2 screenPos)
        {
            reverseYScreenDelta = false;
            //Distance in between current and last scene position
            Vector2 screenDelta = screenPos - lastScreenPosition;
            
            if(screenDelta.magnitude < 0.1f){ return;}
            
            float directionBasedOnDelta = 0;
            float dragAngle = 1;
            float screenMagnitude = screenPos.magnitude;

            if (_selectedAxis == SelectedAxis.Y)
            {
                directionBasedOnDelta = screenDelta.x > 0 ? -1 : 1;
            
                if (CheckIfPointIsBehindObject())
                {
                    directionBasedOnDelta *= -1;
                }
                dragAngle = screenMagnitude * directionBasedOnDelta * rotatesensitivity;
            }
                
            transform.RotateAround(transform.position, axisDirection, dragAngle);
            
            lastScreenPosition = screenPos;
        }
        
        
        void SetSelectedAxis(GameObject gizmo)
        {
            _selectedAxis = SelectedAxis.none;

            if (gizmo.transform.parent.gameObject == X || gizmo.transform.parent.parent.gameObject == X)
            {
                _selectedAxis = SelectedAxis.X;
                axisDirection = transform.right;
            } 
            else if (gizmo.transform.parent.gameObject == Y || gizmo.transform.parent.parent.gameObject == Y)
            {
                _selectedAxis = SelectedAxis.Y;
                axisDirection = transform.up;
            }
            else if (gizmo.transform.parent.gameObject == Z || gizmo.transform.parent.parent.gameObject == Z)
            {
                _selectedAxis = SelectedAxis.Z;
                axisDirection = transform.forward;
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 10); // forward should be blue

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.right * 10); // right should be red
    
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.up * 10); // up should be green
        }

        public enum SelectedAxis
        {
            X,
            Y,
            Z,
            none
        }
        
    }
}