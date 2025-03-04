using ARMagicBar.Resources.Scripts.PlacementBarUI;
using UnityEngine;
using UnityEngine.UI;

namespace ARMagicBar.Resources.Scripts.ExampleExtension
{
    public class DisableElements : MonoBehaviour
    {
        [SerializeField] private Button debugReEnable; 
        //Disables the elements with index 0 , 1 in the PlacementBarUI
        private void Start()
        {
            debugReEnable.onClick.AddListener(ReEnableElements);
        
            for (int i = 0; i < 2; i++)
            {
                PlacementBarUIElements.Instance.DisableElement(i);
            }
        }

    
        //Re-Enable element with index 0 
        public void ReEnableElements()
        {
            PlacementBarUIElements.Instance.EnableElement(0);
        }
    }
}


