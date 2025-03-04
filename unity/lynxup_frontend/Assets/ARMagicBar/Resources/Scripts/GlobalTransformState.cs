using UnityEngine;

namespace ARMagicBar.Resources.Scripts
{
    public class GlobalTransformState : MonoBehaviour
    {
        private TransformState _transformState;

        public static GlobalTransformState _Instance;

        public TransformState GetTransformState
        {
             get => _transformState;
             private set => _transformState = value;
        }
        
        private void Start()
        {
            _Instance = this; 
        }

        public void SetToMove()
        {
            _transformState = TransformState.Move;
        }
        
        public void SetToRotate()
        {
            _transformState = TransformState.Rotate;
        }

        public void SetToScale()
        {
            _transformState = TransformState.Scale;
        }

        public void SetToNone()
        {
            _transformState = TransformState.None;
        }
    }
    
    public enum TransformState 
    {
        Move,
        Rotate,
        Scale,
        None
    }
}