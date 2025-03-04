using UnityEngine;

namespace ARMagicBar.Resources.Scripts.Utility_Tools
{
    public class BlockRayAtLookAtCamera : MonoBehaviour
    {
        private Camera mainCam;


        private void Start()
        {
            mainCam = FindObjectOfType<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 directionToCamera = mainCam.transform.position - transform.position;
            directionToCamera.y = 0; // This removes any vertical component of the vector

            Quaternion rotation = Quaternion.LookRotation(directionToCamera, Vector3.up);
            transform.rotation = rotation * Quaternion.Euler(90, 0, 0); // Adjust these values as needed
        }
    }
}
