using UnityEngine;

namespace ARMagicBar.Resources.Scripts.PlacementBar
{
    public class PlacementObject : MonoBehaviour
    {
        private Transform originalTransform; 
        
        public Transform GetTransform()
        {
            return transform;
        }

        private void OnEnable()
        {
            originalTransform = transform;
        }

        public Transform GetOriginalTransform()
        {
            return originalTransform;
        }
    }
}