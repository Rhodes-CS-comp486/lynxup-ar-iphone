using ARMagicBar.Resources.Scripts.TransformLogic;
using UnityEngine;

namespace ARMagicBar.Resources.Scripts.PlacementBar
{
    [CreateAssetMenu]
    public class PlacementObjectSO : ScriptableObject
    {
        [SerializeField] public string nameOfObject; 
        [SerializeField] public Texture2D uiSprite;
        [SerializeField] public TransformableObject placementObject;
        
        [Header("Optional parameters")]
        [SerializeField] public int placementCosts; 
    }
}