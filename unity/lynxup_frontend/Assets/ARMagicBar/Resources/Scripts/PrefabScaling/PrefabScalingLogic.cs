using UnityEngine;

namespace ARMagicBar.Resources.Scripts.PrefabScaling
{
    [ExecuteInEditMode]
    public class PrefabScalingLogic : MonoBehaviour
    {
        private GameObject objectToScale;
        [SerializeField] private GameObject fullPrefab; 
    
    
        [Range(0,10)]
        [SerializeField] private float adjustOnlyObjectSize = 1.0f; // Default size
    
        [Range(0,10)]
        [SerializeField] private float adjustFullPrefabSize = 1.0f;

        private void Start()
        {
            if (objectToScale == null)
            {
                objectToScale = GetComponent<PlacementObjectVisual.PlacementObjectVisual>().gameObject;
            }
        }

        private void Update()
        { 
            if (objectToScale != null)
            {
                objectToScale.transform.localScale = new Vector3(adjustOnlyObjectSize, adjustOnlyObjectSize, adjustOnlyObjectSize);
            }

            if (fullPrefab != null)
            {
                fullPrefab.transform.localScale = new Vector3(adjustFullPrefabSize, adjustFullPrefabSize, adjustFullPrefabSize);
            }
        }
    }
}
