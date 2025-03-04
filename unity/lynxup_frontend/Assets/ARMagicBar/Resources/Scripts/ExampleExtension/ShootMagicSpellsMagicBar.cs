using ARMagicBar.Resources.Scripts.PlacementBar;
using ARMagicBar.Resources.Scripts.TransformLogic;
using UnityEngine;

namespace ARMagicBar.Resources.Scripts.ExampleExtension
{
    /// <summary>
    /// This is an example on how the AR magic bar could be used to cast spells
    /// </summary>
    public class ShootMagicSpellsMagicBar : MonoBehaviour
    {
        private Camera _mainCam;
        private float spawnDistance = 1.0f;
    
        void Start()
        {
            //When the player taps on the screen and an object from the UI bar is selected, this event is being fired with the 
            //transformable object that is currently selected and the screen-position that the player tapped. 
            ARPlacementPlaneMesh.Instance.OnSpawnObjectWithScreenPos += InstanceOnOnSpawnObjectWithScreenPos;

            _mainCam = FindObjectOfType<Camera>();
        }
    
        private void InstanceOnOnSpawnObjectWithScreenPos((TransformableObject objectToSpawn, Vector2 screenPos) obj)
        {
            Vector3 screenPosition = new Vector3(obj.screenPos.x, obj.screenPos.y, spawnDistance);

            Vector3 worldPosition = _mainCam.ScreenToWorldPoint(screenPosition);

        
            if (obj.objectToSpawn == null) return;
        
            TransformableObject magicObject = Instantiate(obj.objectToSpawn);
            magicObject.transform.position = worldPosition;
        
            // Make the object look away from the camera
            Vector3 lookDirection = (magicObject.transform.position - _mainCam.transform.position).normalized;
            magicObject.transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            Destroy(magicObject.gameObject, 2.5f);
        }

        private void OnDestroy()
        {
            ARPlacementPlaneMesh.Instance.OnSpawnObjectWithScreenPos -= InstanceOnOnSpawnObjectWithScreenPos;
        }
    }
}
