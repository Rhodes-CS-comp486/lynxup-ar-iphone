using System;
using System.Collections.Generic;
using ARMagicBar.Resources.Scripts.Debugging;
using ARMagicBar.Resources.Scripts.TransformLogic;
using UnityEngine;
using UnityEngine.UI;

namespace ARMagicBar.Resources.Scripts.GizmoUI
{
    public class GizmHolderUI : MonoBehaviour
    {
        [SerializeField] public GameObject  gizmoHolderCanvas;
        [SerializeField] public MoveGizmoUI moveGizmoUI;
        [SerializeField] public RotateGizmoUI rotateGizmoUI;
        [SerializeField] public ScaleGizmoUI scaleGizmoUI;
        [SerializeField] public BackToGizmoUI backToGizmoUI;
        [SerializeField] public ResetTransformGizmoUI resetTransformGizmoUI;
        public List<IGizmos> iGizmos; 
        
        //Reference to the buttons
        [SerializeField] public Button moveButtonReference;
        [SerializeField] public Button rotateButtonReference;
        [SerializeField] public Button scaleButtonReference;
        [SerializeField] public Button deleteButtonReference;
        [SerializeField] public Button backButtonReference;
        [SerializeField] public Button resetTransformButtonReference; 

        public event Action moveButtonToggled;
        public event Action rotateButtonToggled;
        public event Action scaleButtonToggled;
        public event Action deleteButtonToggled;

        public event Action resetTransformButtonToggled; 

        public static event Action OnAnyGizmoUIButtonToggled;
        public static event Action OnBackToUIGizmosToggled; 
        

        private TransformableObject _transformableObject;
        private Camera mainCamera;


        public static GizmHolderUI Instance;  
        
        void AddGizmos()
        {
            iGizmos = new List<IGizmos>() { moveGizmoUI, rotateGizmoUI, scaleGizmoUI, resetTransformGizmoUI};
        }


        void SetCanvasCamera()
        {
            Canvas objectCanvas = GetComponent<Canvas>();
            objectCanvas.worldCamera = mainCamera;
        }

        private void Awake()
        {
            Instance = this;
            _transformableObject = GetComponentInParent<TransformableObject>();
            
            if (mainCamera == default)
            {
                mainCamera = FindObjectOfType<Camera>();
                SetCanvasCamera();
            }

        }

        private void OnDestroy()
        {
            _transformableObject.OnWasSelected -= TransformableObjectOnOnWasSelected; 
            SelectObjectsLogic.Instance.OnDeselectAll -= TransformableObjectOnDeselectAll;
        }

        private void Start()
        {
            AddGizmos();
            _transformableObject.OnWasSelected += TransformableObjectOnOnWasSelected; 
            SelectObjectsLogic.Instance.OnDeselectAll += TransformableObjectOnDeselectAll;
            
            resetTransformButtonReference.onClick.AddListener(() =>
            {
                CustomLog.Instance.InfoLog("ResetButtonHit");
                resetTransformButtonToggled?.Invoke();
            });
            
            backButtonReference.onClick.AddListener(() =>
            {
                CustomLog.Instance.InfoLog("Back Button Hit");
                ShowTransformElements();
                HideBackToGizmoUI();
                OnBackToUIGizmosToggled?.Invoke();
            });
            
            moveButtonReference.onClick.AddListener(() =>
            {
                CustomLog.Instance.InfoLog("Move UI Button Clicked");
                moveButtonToggled?.Invoke();
                OnAnyGizmoUIButtonToggled?.Invoke();
                
                HideTransformElements();
                ShowBackToGizmoUI();
                // ShowSingleGizmo(moveGizmoUI);
            });
            rotateButtonReference.onClick.AddListener(() =>
            {
                CustomLog.Instance.InfoLog("Rotate UI Button clicked");
                rotateButtonToggled?.Invoke();
                OnAnyGizmoUIButtonToggled?.Invoke();
                
                HideTransformElements();
                ShowBackToGizmoUI();
                // ShowSingleGizmo(rotateGizmoUI);

            });
            scaleButtonReference.onClick.AddListener(() =>
            {
                CustomLog.Instance.InfoLog("Scale UI Button clicked");
                scaleButtonToggled?.Invoke();
                OnAnyGizmoUIButtonToggled?.Invoke();
                
                HideTransformElements();
                ShowBackToGizmoUI();

                // ShowSingleGizmo(scaleGizmoUI);

            });
            deleteButtonReference.onClick.AddListener(() =>
            {
                deleteButtonToggled?.Invoke();
            });
            
            mainCamera = FindObjectOfType<Camera>();


            HideBackToGizmoUI();
            HideTransformElements();
        }
        
        private void Update()
        {
            if (isActiveAndEnabled && mainCamera != null)
            {
                transform.LookAt(mainCamera.transform);
            }
        }
        
        private void TransformableObjectOnDeselectAll()
        {
            HideTransformElements();
        }

        private void TransformableObjectOnOnWasSelected(bool obj)
        {
            ShowTransformElements();
        }


        void ShowBackToGizmoUI()
        {
            backToGizmoUI.gameObject.SetActive(true);
        }
        
        void HideBackToGizmoUI()
        {
            backToGizmoUI.gameObject.SetActive(false);

        }
        
        public void ShowTransformElements()
        {
           gizmoHolderCanvas.SetActive(true); 
        }

        public void HideTransformElements()
        {
            gizmoHolderCanvas.SetActive(false); 
        }
        
    }
}