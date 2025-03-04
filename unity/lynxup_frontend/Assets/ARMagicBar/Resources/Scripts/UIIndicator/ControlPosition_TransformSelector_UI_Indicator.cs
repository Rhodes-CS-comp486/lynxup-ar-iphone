using System;
using System.Collections.Generic;
using ARMagicBar.Resources.Scripts.TransformLogic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ARMagicBar.Resources.Scripts.UIIndicator
{
    public class ControlPosition_TransformSelector_UI_Indicator : MonoBehaviour
    {
        private Camera mainCam;
    
        [SerializeField] private GameObject UI_TransformElements;
        private Bounds TransformableObjectBounds;
        private Bounds InitialBounds; 

        [FormerlySerializedAs("_transformableObjectSelectVisual")] [SerializeField]
        private SelectVisualLogic selectVisualLogic;

        private PlacementObjectVisual.PlacementObjectVisual _placementObjectVisual;

        private const float DISTANCE_FROM_OBJECT = 1f; // Adjustable distance from the object's surface
        private const float DISTANCE_FROM_BOUNDS = 1f;
        private float circleRadius;
        private const float additionalOffset = .25f;

        private Vector3 closestFromBoundsDebug;
        private Vector3 positionOnRadiusDebug;
        private Vector3 vectorToCamDebug;
        private float circleRadiusDebug;

        private bool objectHasNoRenderer = false;
    
        private void Start()
        {
            mainCam = GameObject.FindObjectOfType<Camera>();
            if (selectVisualLogic == null)
            {
                selectVisualLogic = FindObjectOfType<SelectVisualLogic>();
            }
        
            // Make sure the _transformableObjectSelectVisual is assigned
            TransformableObjectBounds = ReturnHighestBounds(selectVisualLogic.ReturnRenderer());;

            if (TransformableObjectBounds == default)
            {
                objectHasNoRenderer = true;
                return;
            }
        
            InitialBounds = TransformableObjectBounds;
            UI_TransformElements.transform.position =  new Vector3(InitialBounds.center.x,InitialBounds.max.y, InitialBounds.center.z);
            _placementObjectVisual = selectVisualLogic.GetComponentInChildren<PlacementObjectVisual.PlacementObjectVisual>();
        }

        private void Update()
        {
            if(mainCam == null || objectHasNoRenderer) return;
            UI_TransformElements.transform.position = CalculateUIPosition();
            CalculateCircleRadius();

            // new Vector3(currentBounds.center.x,
            // currentBounds.max.y, 
            // currentBounds.center.z); 

            // UI_TransformElements.transform.position = CalculateUIPosition();
            // UI_TransformElements.transform.rotation = Quaternion.LookRotation(UI_TransformElements.transform.position - mainCam.transform.position);
        }


        float CalculateCircleRadius()
        {
            float radius = Math.Max(_placementObjectVisual.gameObject.transform.localScale.x,
                _placementObjectVisual.transform.localScale.z);


            return circleRadiusDebug = radius;
        }

        Bounds ReturnHighestBounds(List<Renderer> renderers)
        {
            if (renderers.Count == 0) return default;
        
            Bounds highestBounds = new Bounds();
            float highestY = Mathf.NegativeInfinity; 
        
            foreach (var renderer in renderers)
            {
                if (renderer.bounds == default) continue;
            
                float boundsMaxY = renderer.bounds.max.y;
                if (boundsMaxY > highestY)
                {
                    highestY = boundsMaxY;
                    highestBounds = renderer.bounds;
                }
            }

            return highestBounds;
        }

        Vector3 CalculateUIPosition()
        {
            Bounds currentBounds = ReturnHighestBounds(selectVisualLogic.ReturnRenderer());

            Vector3 vectorToCamera = (mainCam.transform.position - currentBounds.center).normalized;
            vectorToCamDebug = vectorToCamera;

            Vector3 positionOnRadius = currentBounds.center + vectorToCamera * (currentBounds.extents.magnitude + circleRadius + additionalOffset);
            positionOnRadiusDebug = positionOnRadius;
        
            return positionOnRadius;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
        
            if (closestFromBoundsDebug != default)
            {
                Gizmos.DrawWireSphere(closestFromBoundsDebug, .25f);
            }
        
            Gizmos.color = Color.red;
        
            if (positionOnRadiusDebug != default)
            {
                Gizmos.DrawWireSphere(positionOnRadiusDebug, .25f);
            }
        
            Gizmos.color = Color.green;

            if (vectorToCamDebug != default && closestFromBoundsDebug != default)
            {
                Gizmos.DrawRay(closestFromBoundsDebug, vectorToCamDebug);
            }

            if (circleRadiusDebug != default)
            {
                Gizmos.DrawWireSphere(TransformableObjectBounds.center,
                    circleRadiusDebug);
            }
        }
    }
}
