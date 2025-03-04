using System.Collections.Generic;
using UnityEngine;

namespace ARMagicBar.Resources.Scripts.PlacementBar
{
    [CreateAssetMenu(fileName = "PlaceableObjectDatabase", 
        menuName = "ARMagicBar/PlaceableObjectDatabase", order = 1)]
    public class PlaceableObjectSODatabase : ScriptableObject
    {
        public List<PlacementObjectSO> PlacementObjectSos = new List<PlacementObjectSO>();
    }
}