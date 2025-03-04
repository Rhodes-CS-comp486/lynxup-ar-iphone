using UnityEngine;

namespace ARMagicBar.Resources.Scripts
{
    public class GlobalSelectAxis : MonoBehaviour
    {
        private GlobalAxis _globalAxis;

        public static GlobalSelectAxis Instance;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public GlobalAxis GetGlobaAxis()
        {
            return _globalAxis;
        }


        public void SetGlobalAxisX()
        {
            _globalAxis = GlobalAxis.X;
        }
        
        public void SetGlobalAxisY()
        {
            _globalAxis = GlobalAxis.Y;
        }
        
        public void SetGlobalAxisZ()
        {
            _globalAxis = GlobalAxis.Z;
        }
    }

    public enum GlobalAxis
    {
        X,
        Y,
        Z
    }
}