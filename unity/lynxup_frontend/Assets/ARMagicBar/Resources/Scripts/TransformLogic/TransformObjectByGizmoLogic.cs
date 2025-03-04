using System;
using ARMagicBar.Resources.Scripts.Debugging;
using ARMagicBar.Resources.Scripts.Gizmo;
using ARMagicBar.Resources.Scripts.GizmoUI;
using ARMagicBar.Resources.Scripts.PlacementBar;
using UnityEngine;

namespace ARMagicBar.Resources.Scripts.TransformLogic
{
    public class TransformObjectByGizmoLogic : MonoBehaviour
    {
        private Camera mainCam;

        [SerializeField] private GameObject X;
        [SerializeField] private GameObject Y;
        [SerializeField] private GameObject Z;
        [SerializeField] private GameObject debugPlane; 

       private const float SCALESENSITIVITY = .0085f;
       private const float ROTATESENSITIVITY = 1f;
       private const float MOVESENSITIVITY = .5f;
        
        private bool isDragging; 

        private SelectedAxis _selectedAxis;
        private Vector3 axisDirection;
        
        private Vector3 initialScreenPosition;
        private Vector3 initialWorldHitPosition; 
        private Vector2 lastScreenPosition;

        // private ManageTransformGizmoTypeVisuals _manageTransformGizmoTypeVisuals;
        
        private void Start()
        {
            mainCam = GameObject.FindObjectOfType<Camera>();
            _selectedAxis = SelectedAxis.none;
            // _manageTransformGizmoTypeVisuals = FindObjectOfType<ManageTransformGizmoTypeVisuals>();
            GizmHolderUI.Instance.resetTransformButtonToggled += OnresetTransformButtonToggled;
            ARPlacementPlaneMesh.Instance.OnSpawnObject += ResetAxis;
        }

        private void OnDisable()
        {
            GizmHolderUI.Instance.resetTransformButtonToggled -= OnresetTransformButtonToggled;
            ARPlacementPlaneMesh.Instance.OnSpawnObject -= ResetAxis;
        }

        private void OnresetTransformButtonToggled()
        {
            SelectObjectsLogic.Instance.GetSelectedObject().ResetObject();
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
            CustomLog.Instance.InfoLog("Performing Reset");
            _selectedAxis = SelectedAxis.none;
            axisDirection = Vector3.zero;
            isDragging = false;
            ResetDebugplane();
        }

        void ResetDebugplane()
        {
            CustomLog.Instance.InfoLog("ResetDebugplane");
            if (visualPlane) // Destroy the visual plane on reset
            {
                CustomLog.Instance.InfoLog("Destroy Visual Plane");
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
                    initialWorldHitPosition = rcHit.point;
                    
                    CustomLog.Instance.InfoLog("Hitting GizmoObject: " + rcHit.collider.gameObject.name);
                    SetSelectedAxis(rcHit.collider.gameObject);
                    initialScreenPosition = rcHit.point;
                    isDragging = true;
                    lastScreenPosition = screenPos;

                    var selectedObject = SelectObjectsLogic.Instance.GetSelectedObject();


                    Vector3 inNormal;

                    switch (_selectedAxis)
                    {
                        case SelectedAxis.X:
                            inNormal = Vector3.up; // Plane perpendicular to Y-axis
                            break;
                        case SelectedAxis.Y:
                            inNormal = mainCam.transform.forward; // Plane perpendicular to camera forward
                            break;
                        case SelectedAxis.Z:
                            inNormal = Vector3.up; // Plane perpendicular to Y-axis
                            break;
                        default:
                            inNormal = mainCam.transform.forward;
                            break;
                    }

                    dragPlane = new Plane(
                        inPoint: rcHit.collider.transform.position,
                        inNormal: inNormal
                    );
                    
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
            // Calculate ray from camera to current screen position.
            Ray ray = mainCam.ScreenPointToRay(screenPos);
            if (dragPlane.Raycast(ray, out float enter))
            {
                // Find the point that intersects with the plane.
                Vector3 hitPoint = ray.GetPoint(enter);

                // Calculate the movement vector based on the last hit point and the current hit point.
                Vector3 movementVector = hitPoint - initialScreenPosition;

                // Project the movement vector onto the selected axis direction.
                movementVector = Vector3.Project(movementVector, axisDirection);

                // Apply the movement vector to the selected object.
                SelectObjectsLogic.Instance.GetSelectedObject().MoveVisual(movementVector);

                // Update initialScreenPosition for the next frame.
                initialScreenPosition = hitPoint;
            }
        }
        void ApplyScale(Vector2 screenPos)
        {
            // Calculate the screen delta.
            Vector2 screenDelta = screenPos - lastScreenPosition;

            // Use vertical movement for scaling. You might adjust the sensitivity based on your needs.
            float scaleDelta = screenDelta.y * SCALESENSITIVITY;

            // Convert this to a vector, assuming uniform scaling for simplicity.
            Vector3 deltaScale = new Vector3(scaleDelta, scaleDelta, scaleDelta);
            
            CustomLog.Instance.InfoLog("ApplyScale " + scaleDelta);

            // Apply this delta scale.
            TransformableObject selectedObject = SelectObjectsLogic.Instance.GetSelectedObject();
            selectedObject.ScaleObject(deltaScale);

            // Update for the next frame.
            lastScreenPosition = screenPos;
        }

        void ApplyRotation(Vector2 screenPos)
        {
            // First, calculate the delta between the current screen position and the last screen position.
            Vector2 screenDelta = screenPos - lastScreenPosition;

            // Depending on the axis, map the delta to a rotation amount, invert direction with -1.
            float rotationAmount = 0f;
            switch (_selectedAxis)
            {
                case SelectedAxis.X:
                    // For X rotation, let's use vertical movement (y delta) and invert direction.
                    rotationAmount = screenDelta.y * ROTATESENSITIVITY;
                    break;
                case SelectedAxis.Y:
                    // For Y rotation, horizontal movement (x delta) and invert direction.
                    rotationAmount = -screenDelta.x * ROTATESENSITIVITY;
                    break;
                case SelectedAxis.Z:
                    // For Z rotation, choose a delta that makes sense for your application and invert direction.
                    rotationAmount = -screenDelta.x * ROTATESENSITIVITY; // This is arbitrary.
                    break;
            }

            if (CheckIfPointIsBehindObject())
            {
                rotationAmount *= -1;
            }

            // Convert the rotation amount to a Quaternion around the correct axis.
            Quaternion rotation = Quaternion.Euler(
                _selectedAxis == SelectedAxis.X ? rotationAmount : 0,
                _selectedAxis == SelectedAxis.Y ? rotationAmount : 0,
                _selectedAxis == SelectedAxis.Z ? rotationAmount : 0
            );

            // Apply the rotation to the selected object.
            TransformableObject selectedObject = SelectObjectsLogic.Instance.GetSelectedObject();
            selectedObject.RotateObject(rotation);
            
            CustomLog.Instance.InfoLog("ApplyRotation " + rotation);
    
            // Update the last screen position for the next frame.
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
        
        void SetSelectedAxis(GameObject gizmo)
        {
            CustomLog.Instance.InfoLog("Selected gizmo => " + gizmo.name);
            if (gizmo.transform.parent.gameObject == X || 
                gizmo.transform.parent.parent.gameObject == X)
            {
                CustomLog.Instance.InfoLog("X Axis");
                _selectedAxis = SelectedAxis.X;
                axisDirection = Vector3.right;
            } 
            else if (gizmo.transform.parent.gameObject == Y ||
                     gizmo.transform.parent.parent.gameObject == Y)
            {
                _selectedAxis = SelectedAxis.Y;
                axisDirection = Vector3.up;
                CustomLog.Instance.InfoLog("Y Axis");
            }
            else if (gizmo.transform.parent.gameObject == Z ||
                     gizmo.transform.parent.parent.gameObject == Z)
            {
                _selectedAxis = SelectedAxis.Z;
                axisDirection = Vector3.forward;
                CustomLog.Instance.InfoLog("Z Axis");
            } 
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