using ARMagicBar.Resources.Scripts.PlacementBar;
using UnityEngine;

namespace ARMagicBar.Resources.Scripts.PlacementObjects
{
    public class ReferenceToSO : MonoBehaviour
    {
        public PlacementObjectSO correspondingObject;
    

        public void SetPlacementObjectSO(PlacementObjectSO objectSO)
        {
            correspondingObject = objectSO;
        } 
    
        public PlacementObjectSO GetPlacementObejctSO()
        {
            return correspondingObject;
        } 
    
        public PlacementObjectSO CorrespondingObject
        {
            get;
            set;
        }

    }
}
