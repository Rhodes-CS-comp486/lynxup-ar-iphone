using ARMagicBar.Resources.Scripts.Debugging;
using UnityEngine;

namespace ARMagicBar.Resources.Scripts.UITransformBar
{
    public class UITransformBarCanvasSortingOrder : MonoBehaviour
    { 
        void Start()
        {
            Canvas canvas = GetComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = int.MaxValue;
            CustomLog.Instance.InfoLog("Setting " + canvas.name + " to overwrite Layer");

        }
    }
}

