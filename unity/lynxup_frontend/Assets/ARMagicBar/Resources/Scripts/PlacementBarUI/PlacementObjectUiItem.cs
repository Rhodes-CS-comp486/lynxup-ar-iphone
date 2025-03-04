using ARMagicBar.Resources.Scripts.Debugging;
using ARMagicBar.Resources.Scripts.PlacementBar;
using ARMagicBar.Resources.Scripts.TransformLogic;
using UnityEngine;
using UnityEngine.UI;

namespace ARMagicBar.Resources.Scripts.PlacementBarUI
{
    public class PlacementObjectUiItem : MonoBehaviour
    {
        [SerializeField] private GameObject SelectedState;
        [SerializeField] private GameObject DisabledState; 
        [SerializeField] private RawImage normalIMG;
        private TransformableObject correspondingObject;
        private PlacementObjectSO correspondingPlacementObjectSO;

        private void Awake()
        {
            HideSelectedState();
            HideDisabledState();
        }

        public bool IsDisabled()
        {
            return DisabledState.activeSelf;
        }

        public void SetCorrespondingObject(TransformableObject placementObject)
        {
            correspondingObject = placementObject; 
        }

        public PlacementObjectSO CorrespondingPlacementObjectSO
        {
            get => correspondingPlacementObjectSO;
            set => correspondingPlacementObjectSO = value;
        }
        
        public TransformableObject GetCorrespondingObject()
        {
            return correspondingObject;
        }

        public void SetTexture(Texture2D tex2d)
        {
            normalIMG.texture = tex2d;
        }
        public void ShowSelectedState()
        {
            SelectedState.SetActive(true);
        }

        public void HideSelectedState()
        {
            SelectedState.SetActive(false);
        }

        public void HideDisabledState()
        {
            DisabledState.SetActive(false);
        }

        public void ShowDisabledState()
        {
            CustomLog.Instance.InfoLog("Show disabled state " + gameObject.name);
            DisabledState.SetActive(true);
        }

        public bool IsActive()
        {
            return SelectedState.gameObject.activeSelf;
        }

    }
}