using System;
using System.Collections.Generic;
using ARMagicBar.Resources.Scripts.Debugging;
using ARMagicBar.Resources.Scripts.PlacementBar;
using ARMagicBar.Resources.Scripts.TransformLogic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace ARMagicBar.Resources.Scripts.PlacementBarUI
{
    public class PlacementBarUIElements : MonoBehaviour
    {
        [Header("Ideally a empty GameObject in a canvas with vertical or horizontal stack")]
        [SerializeField] private Transform uiObjectParent;
        [FormerlySerializedAs("UIPrefab")] [SerializeField] private PlacementObjectUiItem uiItemPrefab;
    
        [Header("Change the texture of the page and hide icon")]
        [SerializeField] private Texture2D hideTexture;
        [SerializeField] private Texture2D paginationTexture; 
    

        private PlacementObjectUiItem _selectedUiItemElement; 
        List<PlacementObjectUiItem> _uiObjectGameObjects;
        private List<(int, PlacementObjectUiItem)> pageToUIObjects = new();

        [Header("Determine if there should be a hide / page icon")]
        public bool enableHide = true;
        public bool enablePage = true;

        public int testDisableUIElement = -1; 
    
        // Set object to instantiate 
        public event Action<TransformableObject> OnUiElementSelected;
    
        //For Debug purposes
        // public event Action<PlacementObjectSO> OnUiElementSelectedSO;
        public event Action OnAllUiElementsAdded;
    
        public static PlacementBarUIElements Instance;
    
        private bool barIsHidden = false;
        private const string HIDEOBJECT_NAME = "Hide Obejct";

        private int currentPage = 1;
    
        [Header("Determine the amount of items per page")]
        public int MAX_ITEMS_PER_PAGE = 3; 
        private int maxPage = 1;
        private const string PAGINATIONOBJECT_NAME = "Pagination Object";
    
        // Start is called before the first frame update
        private void Awake()
        {
            if(Instance == null)
                Instance = this; 
        }

        public void DisableElement(int index)
        {
            CustomLog.Instance.InfoLog("Should disable Element at " + index);
            CustomLog.Instance.InfoLog("Amount of UIElements is" + _uiObjectGameObjects.Count);
        
            if(_uiObjectGameObjects[index])
                _uiObjectGameObjects[index].ShowDisabledState();
        }
    
        public void EnableElement(int index)
        {
            if(_uiObjectGameObjects[index])
                _uiObjectGameObjects[index].HideDisabledState();
        }

        void Start()
        {
            _uiObjectGameObjects = new List<PlacementObjectUiItem>();
            SetUIElements();

            if (testDisableUIElement >= 0)
            {
                DisableElement(testDisableUIElement);
            }
        
            ARPlacementPlaneMesh.Instance.OnSpawnObject += InstanceOnOnSpawnObject;
        }

        private void OnDestroy()
        {
            ARPlacementPlaneMesh.Instance.OnSpawnObject -= InstanceOnOnSpawnObject;

        }

        private void InstanceOnOnSpawnObject(TransformableObject obj)
        {
            DeselectOnPlacement();
        }

        private void Update()
        {
            if(GlobalSelectState.Instance.GetTransformstate() != SelectState.unselected) return;
        
        
            CheckForSelectUIElement();
        }


        //Create a UI Element for each Placement-object and add the UI texture onto it 
        void SetUIElements()
        {
            //Paginate item
            if (enablePage)
            {
                PlacementObjectUiItem paginationUiItem = Instantiate(uiItemPrefab, parent: uiObjectParent);
                paginationUiItem.AddComponent<PaginationUIElement>();
                paginationUiItem.gameObject.name = PAGINATIONOBJECT_NAME;
                paginationUiItem.SetTexture(paginationTexture);
            }

        
            //Get all Elements (SO's) from the placementBarLogic 
            PlacementObjectSO[] allPlacement = PlacementBarLogic.Instance.GetAllObjects();
       
            maxPage = (int) Math.Ceiling((double) allPlacement.Length / MAX_ITEMS_PER_PAGE);
       
            foreach (var placementObjectSo in allPlacement)
            {
           
                //Add UI Object for each element and add the specific texture as a icon
                PlacementObjectUiItem instantiatedUiItemElement = Instantiate(uiItemPrefab, parent: uiObjectParent);
                instantiatedUiItemElement.SetTexture(placementObjectSo.uiSprite);
                instantiatedUiItemElement.SetCorrespondingObject(placementObjectSo.placementObject);
                instantiatedUiItemElement.CorrespondingPlacementObjectSO = placementObjectSo;
                instantiatedUiItemElement.gameObject.name = placementObjectSo.placementObject.name;
           
                _uiObjectGameObjects.Add(instantiatedUiItemElement);



                int index = Array.IndexOf(allPlacement, placementObjectSo);

                (int, PlacementObjectUiItem) indexToObject =
                    new(CalculatePage(index + 1), instantiatedUiItemElement);
           
                pageToUIObjects.Add(indexToObject);
            }
       
            //Hide item
            if (enableHide)
            {
                PlacementObjectUiItem hideUiItemObject = Instantiate(uiItemPrefab, parent: uiObjectParent);
                hideUiItemObject.AddComponent<HideUIElement>();
                hideUiItemObject.gameObject.name = HIDEOBJECT_NAME;
                hideUiItemObject.SetTexture(hideTexture);
            }
       
            ShowItemsOnCurrentPage();
            DebugPrintDictionary();
            OnAllUiElementsAdded?.Invoke();

        }


        void ShowItemsOnCurrentPage()
        {

            foreach (var kvp in pageToUIObjects)
            {
                if (kvp.Item1 == currentPage)
                {
                    kvp.Item2.gameObject.SetActive(true);
                }
                else
                {
                    kvp.Item2.gameObject.SetActive(false);
                }     
            }
        }


        void DebugPrintDictionary()
        {
            foreach (var kvp in pageToUIObjects)
            {
                CustomLog.Instance.InfoLog("Page: " + kvp.Item1 + " Contains: " + kvp.Item2.name);
            }
        }

        int CalculatePage(int itemIndex)
        {
            return (int)Math.Ceiling((double)itemIndex / MAX_ITEMS_PER_PAGE);
        }

        public void PageUp()
        {
            currentPage += 1;
            currentPage %= maxPage+1;
        

            if (currentPage == 0)
                currentPage = 1;
        }


        public void HideUIElements()
        {
        
            foreach (var placementObjectUI in _uiObjectGameObjects)
            {
                if(placementObjectUI.GetComponent<HideUIElement>()) return;
            
                placementObjectUI.transform.gameObject.SetActive(false);
            }
        
            DeselectAllElements();

            barIsHidden = true;
        }

        public void ShowUIElements()
        {
            foreach (var placementObjectUI in _uiObjectGameObjects)
            {
                if(placementObjectUI.GetComponent<HideUIElement>()) return;

                placementObjectUI.transform.gameObject.SetActive(true);

            }
        
            barIsHidden = false;
        }

        public void DeselectAllElements()
        {
            foreach (var placementObjectUI in _uiObjectGameObjects)
            { 
                if(placementObjectUI.GetComponent<HideUIElement>()) continue;
                placementObjectUI.HideSelectedState();
            }
        
            OnUiElementSelected?.Invoke(null);
        }

        void DeselectOnPlacement()
        {
            OnUiElementSelected?.Invoke(null);
            DeselectAllElements();
        }
    
        void CheckForSelectUIElement()
        {
            if(EventSystem.current == null) return;
        
            Vector2 inputPos = Vector2.zero;
            bool isInputDetected = false;

            // Check for touch input
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                inputPos = Input.GetTouch(0).position;
                isInputDetected = true;
            }

#if UNITY_EDITOR
        
            // Check for mouse input
            else if (Input.GetMouseButtonDown(0))
            {
                inputPos = Input.mousePosition;
                isInputDetected = true;
            }
#endif
        
        
            // Proceed only if an input is detected
            if (isInputDetected)
            {
                //Is over UI Object ?
                if (EventSystem.current.IsPointerOverGameObject() || // For mouse
                    (Input.touchCount > 0 &&
                     EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))) // For touch
                {
                    PointerEventData pointerEventData = new PointerEventData(EventSystem.current) { position = inputPos };
                    List<RaycastResult> results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointerEventData, results);
                
                    foreach (var raycastResult in results)
                    {
                        PlacementObjectUiItem selectedUiItem = raycastResult.gameObject.GetComponentInParent<PlacementObjectUiItem>();
                    
                        if(selectedUiItem == null) continue;

                    
                    
                        if (selectedUiItem.TryGetComponent(out HideUIElement hidePlacementBarUI))
                        {
                            if (barIsHidden)
                            {
                                CustomLog.Instance.InfoLog("Show Elements Outter");
                                ShowUIElements();
                            }
                            else
                            {
                                CustomLog.Instance.InfoLog("Hide Elements Outter");
                                HideUIElements();
                            }
                        }

                        if (selectedUiItem.TryGetComponent(out PaginationUIElement paginationUIElement))
                        {
                            PageUp();
                            ShowItemsOnCurrentPage();
                        }
                    

                        if (selectedUiItem != null)
                        {
                            if(selectedUiItem.IsDisabled())
                                return;
                        
                            if (_uiObjectGameObjects.Contains(selectedUiItem))
                            {
                            
                                if (selectedUiItem.IsActive())
                                {
                                
                                    OnUiElementSelected?.Invoke(null);
                                    // OnUiElementSelectedSO?.Invoke(null);
                                
                                    selectedUiItem.HideSelectedState();
                                }
                                else
                                {
                                    selectedUiItem.ShowSelectedState();
                                    OnUiElementSelected?.Invoke(selectedUiItem.GetCorrespondingObject());
                                    // OnUiElementSelectedSO?.Invoke(selectedUiItem.CorrespondingPlacementObjectSO);
                                
                                
                                    foreach (var objects in _uiObjectGameObjects)
                                    {
                                        if (objects != selectedUiItem)
                                        {
                                            objects.HideSelectedState();
                                        }
                                    }
                                } 
                                break;
                            }
                        }
                    }
                }
                else
                {
                    //When Clicking anywhere not on the UI
                    // DeselectAllElements();
                }
            }
        }
    }
}
