using ARMagicBar.Resources.Scripts.Debugging;
using ARMagicBar.Resources.Scripts.TransformLogic;
using UnityEngine;

namespace ARMagicBar.Resources.Scripts.Other
{
    public class TogglePlacementBarSettings : MonoBehaviour
    {
        [Header("Disable the selection and transformation for all objects")]
        [SerializeField] private bool disableTransform;

        

        private void Start()
        {
            CustomLog.Instance.InfoLog("Setting DisableTransfomr to => " +  disableTransform);
            TransformableObjectsSelectLogic.Instance.DisableTransformOptions = disableTransform;
            SelectObjectsLogic.Instance.DisableTransformOptions = disableTransform;

        }
    }
}