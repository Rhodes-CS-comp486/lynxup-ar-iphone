using System.Collections.Generic;
using ARMagicBar.Resources.Scripts.Debugging;
using ARMagicBar.Resources.Scripts.PlacementBarUI;
using ARMagicBar.Resources.Scripts.TransformLogic;
using UnityEditor;
using UnityEngine;

namespace ARMagicBar.Resources.Scripts.PlacementBar
{
    public class PlacementBarLogic : MonoBehaviour
    {
        [SerializeField] private PlaceableObjectSODatabase database;
        [SerializeField] private List<PlacementObjectSO> placementObjects = new List<PlacementObjectSO>();
    
        private TransformableObject objectToPlace;
        private PlacementObjectSO placementObjectSoToPlace;

        public static PlacementBarLogic Instance; 
    
        void Awake()
        {
            Instance = this;
            objectToPlace = null;
            SetPlacementObjects();
        }

        void SetPlacementObjects()
        {
            placementObjects = new List<PlacementObjectSO>(database.PlacementObjectSos);
        }
        
        public void ClearObjectToInstantiate()
        {
            objectToPlace = null;
        }


#if  UNITY_EDITOR
    
        public void LoadAllPlacementObjects()
        {  
            placementObjects.Clear();
        
            // Find all ScriptableObject assets of type PlacementObjectSO in the specific folder
            string[] guids = AssetDatabase.FindAssets("t:PlacementObjectSO", new[] { "Assets/PlaceAndManipulateObjects/Resources/PlaceableObjects" });
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                PlacementObjectSO obj = AssetDatabase.LoadAssetAtPath<PlacementObjectSO>(assetPath);
                if (obj != null)
                {
                    placementObjects.Add(obj);
                }
            }
        }

#endif


        private void Start()
        {
            PlacementBarUIElements.Instance.OnUiElementSelected += SetObjectToInstantiate;
        }

        private void OnDestroy()
        {
            PlacementBarUIElements.Instance.OnUiElementSelected -= SetObjectToInstantiate;
        }

        private void SetObjectToInstantiate(TransformableObject obj)
        {
            if (obj != null)
            {
                CustomLog.Instance.InfoLog("Should set object to place to " + obj.name);
            }
            else
            {
                CustomLog.Instance.InfoLog("Should set object to null");
            }
            objectToPlace = obj;
        }

        public TransformableObject GetPlacementObject()
        {
            return objectToPlace;
        }

        public PlacementObjectSO GetPlacementObjectSo()
        {
            return placementObjectSoToPlace;
        }
    
        public PlacementObjectSO[] GetAllObjects()
        {
            if (placementObjects == null) return default;
        
            return placementObjects.ToArray();
        }

        public bool GetIsSelectedFromUI(PlacementObjectUiItem uiItemObject)
        {
            foreach (var placementObjectSo in placementObjects)
            {
                if (placementObjectSo.uiSprite == uiItemObject)
                {
                    return default;
                }
            }
            return default;
        }
    
    }
}
