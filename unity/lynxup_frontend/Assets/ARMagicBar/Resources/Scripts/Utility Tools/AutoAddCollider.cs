using System.Collections.Generic;
using ARMagicBar.Resources.Scripts.Debugging;
using ARMagicBar.Resources.Scripts.TransformLogic;
using Unity.VisualScripting;
using UnityEngine;

namespace ARMagicBar.Resources.Scripts.Utility_Tools
{
    [RequireComponent(typeof(SelectVisualLogic))]
    public class AutoAddCollider : MonoBehaviour
    {
        private SelectVisualLogic _selectVisualLogic;
        private List<Renderer> _transformableObjectRenderer = new();
        private BoxCollider boxCol; 
    
        void Start()
        {
            _selectVisualLogic = GetComponent<SelectVisualLogic>();
            _transformableObjectRenderer = GetRendererFromComponent();
        
            CustomLog.Instance.InfoLog("_transformab_transformableObjectRendererleObjectRenderer: "
                                       + _transformableObjectRenderer + "Count ==" + _transformableObjectRenderer.Count);
        

            if (_transformableObjectRenderer != null && _transformableObjectRenderer.Count != 0)
            {
                AddMeshColliderToObject(_transformableObjectRenderer);
            }
            else
            {
                AddBoxColliderToObject();
                // Debug.LogError("Renderer of Transformable Object has not been attached in TransformableObjectSelectVisual");
            }
        }

        List<Renderer> GetRendererFromComponent()
        {
        
            if (_selectVisualLogic)
            {
                return _selectVisualLogic.ReturnRenderer();
            }


            return null;
        }

        void AddBoxColliderToObject()
        {
            CustomLog.Instance.InfoLog("Adding Box Collider to object!");
            boxCol = transform.AddComponent<BoxCollider>();
            boxCol.size = GetComponentInChildren<PlacementObjectVisual.PlacementObjectVisual>().transform.localScale;
        }

        void AddMeshColliderToObject(List<Renderer> renderers)
        {
        
            for (int i = 0; i < renderers.Count; i++)
            {
                if (!renderers[i].GetComponent<Collider>() || !renderers[i].transform.GetComponentInChildren<Collider>())
                {
                    MeshCollider meshCol = renderers[i].AddComponent<MeshCollider>();
                    Mesh objectMesh ;

                    if (renderers[i].GetComponent<SkinnedMeshRenderer>())
                    {
                        objectMesh  = renderers[i].GetComponent<SkinnedMeshRenderer>().sharedMesh;
                    }
                    else if(renderers[i].GetComponent<MeshFilter>())
                    {
                        objectMesh  = renderers[i].GetComponent<MeshFilter>().sharedMesh;
                    }
                    else
                    {
                        objectMesh = null;
                        CustomLog.Instance.InfoLog($"PlacementObject {renderers[i].name} does not have a mesh attached.");
                    }

                    if(objectMesh != null)
                        meshCol.sharedMesh = objectMesh;
                }
            }
        }
    
    }
}