using ARMagicBar.Resources.Scripts.GizmoUI;
using ARMagicBar.Resources.Scripts.TransformLogic;
using UnityEngine;

namespace ARMagicBar.Resources.Scripts
{
    public class GlobalSelectState : MonoBehaviour
    {
         SelectState _selectState;
         
         public static GlobalSelectState Instance;

         private void Awake()
         {
             Instance = this;
             _selectState = SelectState.unselected;
         }

         private void Start()
        {
            if (SelectObjectsLogic.Instance != null)
            {
                SelectObjectsLogic.Instance.OnSelectObject += SetGlobalStateToSelected;
                SelectObjectsLogic.Instance.OnDeselectAll += SetGlobalStateToUnselected;
                SelectObjectsLogic.Instance.OnGizmoSelected += TransformableObjectSelectLogicOnOnGizmoSelected;
            }
            else
            {
                Debug.LogError("SelectObjectsLogic instance is not initialized.");
            }
            
            TransformableObject.OnBeingDeleted += SetGlobalStateToUnselectedGameObject;
            GizmHolderUI.OnAnyGizmoUIButtonToggled += SetGlobalStateToManipulate;
            GizmHolderUI.OnBackToUIGizmosToggled += SetGlobalStateToSelected; 
            
            
        }

        private void OnDestroy()
        {
            if (SelectObjectsLogic.Instance != null)
            {
                SelectObjectsLogic.Instance.OnSelectObject -= SetGlobalStateToSelected;
                SelectObjectsLogic.Instance.OnDeselectAll -= SetGlobalStateToUnselected;
                SelectObjectsLogic.Instance.OnGizmoSelected -= TransformableObjectSelectLogicOnOnGizmoSelected;
            }
            else
            {
                Debug.LogError("SelectObjectsLogic instance is not initialized.");
            }
            
            TransformableObject.OnBeingDeleted -= SetGlobalStateToUnselectedGameObject;
            GizmHolderUI.OnAnyGizmoUIButtonToggled -= SetGlobalStateToManipulate;
            GizmHolderUI.OnBackToUIGizmosToggled -= SetGlobalStateToSelected; 

        }

        private void TransformableObjectSelectLogicOnOnGizmoSelected(GameObject obj)
        {
            SetGlobalStateToDragging();
        }

        public SelectState GetTransformstate()
        {
            return _selectState; 
        }

        void SetGlobalStateToDragging()
        {
            _selectState = SelectState.dragging;
        }
        
        void SetGlobalStateToManipulate()
        {
            _selectState = SelectState.manipulating;
        }

        void SetGlobalStateToSelected()
        {
            _selectState = SelectState.selected;
        }

        void SetGlobalStateToUnselectedGameObject(GameObject gameObject)
        {
            SetGlobalStateToUnselected();
        } 

        void SetGlobalStateToUnselected()
        {
            _selectState = SelectState.unselected;
        }
        
    }
    
    public enum SelectState
    {
        unselected, 
        selected,
        manipulating,
        dragging
    }
}