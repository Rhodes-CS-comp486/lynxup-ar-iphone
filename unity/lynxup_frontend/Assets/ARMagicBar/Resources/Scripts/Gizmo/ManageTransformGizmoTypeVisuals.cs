using System;
using System.Collections.Generic;
using ARMagicBar.Resources.Scripts.Debugging;
using ARMagicBar.Resources.Scripts.Gizmo.Visuals;
using ARMagicBar.Resources.Scripts.GizmoUI;
using UnityEngine;

namespace ARMagicBar.Resources.Scripts.Gizmo
{
    public class ManageTransformGizmoTypeVisuals : MonoBehaviour
    {
        [SerializeField] private GizmHolderUI _gizmoHolderUI;
        [SerializeField] private List<MoveGizmoVisual> _moveGizmoVisuals;
        [SerializeField] private List<ScaleGizmoVisual> _scaleGizmoVisuals;
        [SerializeField] private List<RotateGizmoVisual> _rotateGizmoVisuals;
        
        //empty Gameobject that adjusts to the local position of an object
        [SerializeField] private Transform localGizmosTransform;
        
        
        private void Start()    
        {
            _gizmoHolderUI.moveButtonToggled += ToggleMoveGizmo;
            _gizmoHolderUI.scaleButtonToggled += ToggleScaleGizmo;
            _gizmoHolderUI.rotateButtonToggled += ToggleRotateGizmo;
            GizmHolderUI.OnBackToUIGizmosToggled += HideAll;

            HideAll();
            // gizmoTransformType = GizmoTransformType.none;

        }
        private void OnDestroy()
        {
            _gizmoHolderUI.moveButtonToggled -= ToggleMoveGizmo;
            _gizmoHolderUI.scaleButtonToggled -= ToggleScaleGizmo;
            _gizmoHolderUI.rotateButtonToggled -= ToggleRotateGizmo;
            GizmHolderUI.OnBackToUIGizmosToggled -= HideAll;
        }

        void ToggleOneGizmoActive()
        {
            
            switch (GlobalTransformState._Instance.GetTransformState)
            {
                case TransformState.Move:
                    SetMoveActive();
                    break;
                case TransformState.Rotate:
                    SetRotateActive();
                    break;
                case TransformState.Scale:
                    SetScaleActive();
                    break;
                case TransformState.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            // switch (gizmotransformType)
            // {
            //     case GizmoTransformType.move:
            //         SetMoveActive();
            //         break;
            //     case GizmoTransformType.scale:
            //         SetScaleActive();
            //         break;
            //     case GizmoTransformType.rotate:
            //         SetRotateActive();
            //         break;
            //     case GizmoTransformType.none:
            //         break;
            //     default:
            //         throw new ArgumentOutOfRangeException();
            // }
        }

        void SetMoveActive()
        {
            foreach (var moveGizmo in _moveGizmoVisuals)
            {
                moveGizmo.gameObject.SetActive(true);
            }

            foreach (var scaleGizmo in _scaleGizmoVisuals)
            {
                scaleGizmo.gameObject.SetActive(false);
            }

            foreach (var rotateGizmo in _rotateGizmoVisuals)
            {
                rotateGizmo.gameObject.SetActive(false);
            }
        }
        
        void SetRotateActive()
        {
            foreach (var moveGizmo in _moveGizmoVisuals)
            {
                moveGizmo.gameObject.SetActive(false);
            }

            foreach (var scaleGizmo in _scaleGizmoVisuals)
            {
                scaleGizmo.gameObject.SetActive(false);
            }

            foreach (var rotateGizmo in _rotateGizmoVisuals)
            {
                rotateGizmo.gameObject.SetActive(true);
            }
        }
        
        void SetScaleActive()
        {
            foreach (var moveGizmo in _moveGizmoVisuals)
            {
                moveGizmo.gameObject.SetActive(false);
            }

            foreach (var scaleGizmo in _scaleGizmoVisuals)
            {
                scaleGizmo.gameObject.SetActive(true);
            }

            foreach (var rotateGizmo in _rotateGizmoVisuals)
            {
                rotateGizmo.gameObject.SetActive(false);
            }
        }

        void HideAll()
        {
            foreach (var moveGizmo in _moveGizmoVisuals)
            {
                moveGizmo.gameObject.SetActive(false);
            }

            foreach (var scaleGizmo in _scaleGizmoVisuals)
            {
                scaleGizmo.gameObject.SetActive(false);
            }

            foreach (var rotateGizmo in _rotateGizmoVisuals)
            {
                rotateGizmo.gameObject.SetActive(false);
            }
            
            GlobalTransformState._Instance.SetToNone();
        }
        

        void ToggleMoveGizmo()
        {
            CustomLog.Instance.InfoLog("Toggle Move Gizmo");
            // gizmoTransformType = GizmoTransformType.move;
            GlobalTransformState._Instance.SetToMove();
            ToggleOneGizmoActive();
        }

        void ToggleScaleGizmo()
        {
            CustomLog.Instance.InfoLog("Toggle Scale Gizmo");
            // gizmoTransformType = GizmoTransformType.scale;
            GlobalTransformState._Instance.SetToScale();
            ToggleOneGizmoActive();
        }

        void ToggleRotateGizmo()
        {
            CustomLog.Instance.InfoLog("Toggle Rotate Gizmo");
            // gizmoTransformType = GizmoTransformType.rotate;
            GlobalTransformState._Instance.SetToRotate();
            ToggleOneGizmoActive();
        }
        
        public enum GizmoTransformType
        {
            move,
            scale,
            rotate,
            none
        }


    }
}