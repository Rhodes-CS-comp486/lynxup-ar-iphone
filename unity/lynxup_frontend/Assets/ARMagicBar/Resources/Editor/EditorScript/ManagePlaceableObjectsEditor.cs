using System.Collections.Generic;
using System.Linq;
using ARMagicBar.Resources.Scripts.Debugging;
using ARMagicBar.Resources.Scripts.PlacementBar;
using ARMagicBar.Resources.Scripts.PlacementObjects;
using ARMagicBar.Resources.Scripts.PlacementObjectVisual;
using ARMagicBar.Resources.Scripts.TransformLogic;
using UnityEditor;
using UnityEngine;


namespace ARMagicBar.Resources.Editor.EditorScript
{
    public class ManagePlaceableObjectsEditor : EditorWindow
    {
        [SerializeField] private GameObject prefab;
    
        private PlaceableObjectSODatabase _placeableObjectSoDatabase;
    
        private List<GameObject> prefabs = new List<GameObject>();
        private List<Sprite> images = new List<Sprite>();

        private List<GameObject> loadedPrefabsRef = new List<GameObject>();
        private List<Sprite> loadedImageRefs = new List<Sprite>();
    
        private const int maxObjectCount = 9;
        private Vector2 scrollPosition;

 
        private const string maxPrefabMessage =
            "Max placement objects reached for this database. You can create another one or delete unused objects.";
    
        private TransformableObject placeableObjectTemplate;
    
    

        [MenuItem("Window/AR Magic Bar/Prefab and Image Editor")]
        public static void ShowWindow()
        {
            GetWindow<ManagePlaceableObjectsEditor>("Prefab and Image Editor");
        }
    
        void AddToDatabase(PlacementObjectSO placementObjectSo)
        {
            CustomLog.EnsureInstance();
        
            _placeableObjectSoDatabase.PlacementObjectSos.Add(placementObjectSo);
            EditorUtility.SetDirty(_placeableObjectSoDatabase); // Mark it dirty to ensure it saves
            AssetDatabase.SaveAssets();
        }

        private void OnInspectorUpdate()
        {
            this.Repaint();
            CheckForNulls();
        }

        private void Awake()
        {
            CustomLog.EnsureInstance();

            if(_placeableObjectSoDatabase == null)
                FindOrCreateDatabase();
        
        
            LoadPlaceableObjectTemplate();
            RefreshExistingPlaceableObjects();
        }


        void FindOrCreateDatabase() {
        
            CustomLog.EnsureInstance();

        
            string searchFilter = "t:PlaceableObjectSODatabase";
            string[] results = AssetDatabase.FindAssets(searchFilter);
            if (results.Length > 0) {
                string path = AssetDatabase.GUIDToAssetPath(results[0]);
                _placeableObjectSoDatabase = AssetDatabase.LoadAssetAtPath<PlaceableObjectSODatabase>(path);
            } else {
                // No database found, create a new one
                CreateNewDatabase();
            }
        }

        void CreateNewDatabase() {
            CustomLog.EnsureInstance();
        
            _placeableObjectSoDatabase = ScriptableObject.CreateInstance<PlaceableObjectSODatabase>();
            string assetPath = "Assets/ARMagicBar/PlaceableObjectsDatabase/PlaceableObjectDatabase.asset";
            AssetDatabase.CreateAsset(_placeableObjectSoDatabase, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            CustomLog.Instance.InfoLog(("Created new PlaceableObjectSODatabase at: " + assetPath));
        }

        private int selectedIndex = -1;


        void LoadPlaceableObjectTemplate()
        {
            CustomLog.EnsureInstance();

            if (placeableObjectTemplate == null)
            {
                GameObject prefab = UnityEngine.Resources.Load<GameObject>("Prefabs/PlaceableObject/PlaceableObjectTemplate");
                if (prefab != null)
                {
                    placeableObjectTemplate = prefab.GetComponent<TransformableObject>();
                }
                else
                {
                    Debug.LogError("Could not load the PlaceableObject prefab. Please check the path.");
                }
            }
        }


        void OnGUI()
        {        
            CustomLog.EnsureInstance();

            EditorGUILayout.BeginVertical();
            GUILayout.Label("Manage Placeable Objects", EditorStyles.boldLabel);

            // Allow the user to assign the database via drag-and-drop or by selecting with the Object Picker
            _placeableObjectSoDatabase = EditorGUILayout.ObjectField("Placeable Object Database", _placeableObjectSoDatabase, typeof(PlaceableObjectSODatabase), false) as PlaceableObjectSODatabase;

            if (_placeableObjectSoDatabase == null)
            {
                EditorGUILayout.HelpBox("Please assign a PlaceableObjectSODatabase.", MessageType.Warning);
            }
            else
            {
                if (GUILayout.Button("Refresh Database"))
                {
                    RefreshExistingPlaceableObjects();
                }
            
                InitializePlaceableObjectTemplate();

                GUILayout.Label("Prefab and Image Editor", EditorStyles.boldLabel);

                scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            
                if (prefabs.Count <= 30)
                {
                    if (GUILayout.Button("Add Pair"))
                    {
                        CustomLog.Instance.InfoLog("Adding prefab, count is" + prefabs.Count);
                        CustomLog.Instance.InfoLog($"Creating placeable object for {prefab.name} with image {images}");
                        prefabs.Add(null);
                        images.Add(null);
                    }
                }

                for (int i = 0; i < prefabs.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                
                    prefabs[i] = EditorGUILayout.ObjectField("Prefab", prefabs[i], typeof(GameObject), false) as GameObject;
                    images[i] = EditorGUILayout.ObjectField("Image", images[i], typeof(Sprite), false) as Sprite;
                    if (GUILayout.Button("Delete", GUILayout.Width(60)))
                    {
                        //If the user added an empty field with no gameobject or image attached yet
                        if (prefabs[i] == null || images[i] == null)
                        {
                            prefabs.RemoveAt(i);
                            images.RemoveAt(i);
                            GUILayout.EndHorizontal();
                            continue;
                        }
                        else
                        {
                            //Delete the object and scriptable object in it's folder 
                            PlacementObjectSO toDelete = _placeableObjectSoDatabase.PlacementObjectSos.FirstOrDefault(so => so.placementObject.gameObject == prefabs[i]);
                    
                            if (toDelete != null)
                            {
                                // Construct the asset path for both the ScriptableObject and its prefab
                                string soPath = AssetDatabase.GetAssetPath(toDelete);
                                string prefabPath = AssetDatabase.GetAssetPath(toDelete.placementObject.gameObject);

                                // Remove from the database list
                                _placeableObjectSoDatabase.PlacementObjectSos.Remove(toDelete);

                                // Delete the assets
                                AssetDatabase.DeleteAsset(prefabPath);
                                AssetDatabase.DeleteAsset(soPath);
                        
                                prefabs.RemoveAt(i);
                                images.RemoveAt(i);
                            }

                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();

            

            
                if (GUILayout.Button("Create Placeable Objects"))
                {
                    HashSet<GameObject> uniquePrefabs = new HashSet<GameObject>();
                    HashSet<Sprite> uniquImages = new HashSet<Sprite>();
                
                    CustomLog.Instance.InfoLog("Before Distinct");
                    DebugList(prefabs);

                    loadedPrefabsRef = prefabs.Distinct().ToList();
                    loadedImageRefs = images.Distinct().ToList();
                
                    CustomLog.Instance.InfoLog("After Distinct");
                    DebugList(loadedPrefabsRef);
                
                    for (int i = 0; i < loadedPrefabsRef.Count; i++)
                    {
                        CustomLog.Instance.InfoLog($"Create Obj, prefabs  => {i} name = {prefabs[i].name} ");
                        if (loadedPrefabsRef[i] != null && !uniquePrefabs.Contains(prefabs[i]) && loadedImageRefs[i] != null)
                        {
                            uniquePrefabs.Add(loadedPrefabsRef[i]); // Add to set to ensure uniqueness
                            // uniquePrefabs.Add(prefabs[i]); // Add to set to ensure uniqueness
                            CustomLog.Instance.InfoLog("Creating Place Object for: " + loadedPrefabsRef[i].name + " Image: " + loadedImageRefs[i].texture);
                            CreatePlaceableObject(loadedPrefabsRef[i], loadedImageRefs[i].texture);
                        }
                    }

                    RefreshExistingPlaceableObjects(); // Refresh list to reflect the new state
                }
        
            }
            EditorGUILayout.EndVertical();
        }
    
        void CheckForNulls()
        {
            CustomLog.EnsureInstance();

            if (prefabs.Count <= 0 && images.Count <= 0) return;

        
            for (int i = 0; i < images.Count; i++)
            {
                if (images[i] == null)
                {
                    // Make sure there is a corresponding element in the database list
                    if (i < _placeableObjectSoDatabase.PlacementObjectSos.Count)
                    {
                        if(_placeableObjectSoDatabase.PlacementObjectSos[i] == null) return;
                    
                        images[i] = TextureToSprite(_placeableObjectSoDatabase.PlacementObjectSos[i].uiSprite);
                    }
                }
            }
        }

        void RefreshExistingPlaceableObjects() {
            CustomLog.EnsureInstance();

            // Clear current lists to avoid duplicates if this method is called multiple times
        
            prefabs.Clear();
            images.Clear();
        
        
            // Iterate over the PlaceableObjectSODatabase and populate the lists
            if (_placeableObjectSoDatabase != null) {
                foreach (var placeableObjectSO in _placeableObjectSoDatabase.PlacementObjectSos) {
                    if (placeableObjectSO.placementObject != null && placeableObjectSO.uiSprite != null) {
                    
                        // Assuming the prefab is the parent of the TransformableObject component
                        GameObject prefab = placeableObjectSO.placementObject.gameObject;
                        if (prefab != null) {
                            // Add the prefab to the list
                            prefabs.Add(prefab);
                            // Add the associated sprite to the list
                            images.Add(TextureToSprite(placeableObjectSO.uiSprite));
                        
                            CustomLog.Instance.InfoLog("Loading prefab " + prefab.name);
                        }
                    }
                }
            }

        }
    
        void InitializePlaceableObjectTemplate()
        {
            CustomLog.EnsureInstance();

            if (placeableObjectTemplate == null)
            {
                GameObject prefab = UnityEngine.Resources.Load<GameObject>("Prefabs/PlaceableObject/PlaceableObjectTemplate");
                if (prefab != null)
                {
                    placeableObjectTemplate = prefab.GetComponent<TransformableObject>();
                }
                else
                {
                    Debug.LogError("Could not load the PlaceableObject prefab. Please check the path.");
                }
            }
        }

    
        /// <summary>
        /// </summary>
        /// <param name="texture2D">Takes Texture2D</param>
        /// <returns>Sprite</returns>
        Sprite TextureToSprite(Texture2D texture2D)
        {
            CustomLog.EnsureInstance();

            Texture2D texture = texture2D; // Assuming this is a Texture2D
            Sprite newSprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return newSprite;
        }
    
        void CleanupDatabase()
        {
            CustomLog.EnsureInstance();

        
            if (_placeableObjectSoDatabase == null)
            {
                Debug.LogError("PlaceableObjectSODatabase is not assigned.");
                return;
            }

            // Check for null or missing objects and mark them for removal
            List<PlacementObjectSO> invalidEntries = new List<PlacementObjectSO>();

            foreach (var entry in _placeableObjectSoDatabase.PlacementObjectSos)
            {
                if (entry == null || entry.placementObject == null || entry.placementObject.gameObject == null)
                {
                    invalidEntries.Add(entry);
                }
            }

            // Remove all invalid entries from the database
            foreach (var invalidEntry in invalidEntries)
            {
                _placeableObjectSoDatabase.PlacementObjectSos.Remove(invalidEntry);

                // Additionally, if the ScriptableObject itself is not null, you might want to delete it from the asset database
                if (invalidEntry != null)
                {
                    string assetPath = AssetDatabase.GetAssetPath(invalidEntry);
                    AssetDatabase.DeleteAsset(assetPath);
                }
            }

            // Save changes to the asset database
            EditorUtility.SetDirty(_placeableObjectSoDatabase);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            CustomLog.Instance.InfoLog("Cleaned up PlaceableObjectSODatabase. Removed " + invalidEntries.Count + " invalid entries.");
        }


        private void CreatePlaceableObject(GameObject prefab, Texture2D image)
        {
            CustomLog.EnsureInstance();

            if (placeableObjectTemplate == null || prefab == null || image == null)
            {
                Debug.Log("Missing template, prefab, or image. Skipping creation.");
                return;
            }
        
        
            if(_placeableObjectSoDatabase == null || _placeableObjectSoDatabase.PlacementObjectSos == null)
            {
                Debug.LogError("PlaceableObjectSODatabase or its list is null. Cannot create placeable object.");
                return;
            }
        
        
            // Check for existing objects
            var existingObject = _placeableObjectSoDatabase.PlacementObjectSos
                .FirstOrDefault(po => po.placementObject.gameObject.name == prefab.name);
            if (existingObject != null)
            {
                CustomLog.Instance.InfoLog($"Skipping creation: An object for {prefab.name} already exists.");
                return;
            }
        
        
        
            //Instantiate the prefab template
            GameObject newPrefab = Instantiate(placeableObjectTemplate.gameObject);
            EditorUtility.SetDirty(newPrefab);
        
        
            //Attach the ReferenceToSO Script
            CustomLog.Instance.InfoLog("Is new prefab active? " + newPrefab.activeSelf);

        
            GameObject childPrefab = Instantiate(prefab, newPrefab.transform);
            childPrefab.transform.localPosition = Vector3.zero;
        
            // childPrefab.transform.localRotation = Quaternion.identity;
            // childPrefab.transform.localPosition += new Vector3(0, childPrefab.transform.localScale.y / 2, 0);
        
            childPrefab.AddComponent<PlacementObjectVisual>();

            // Save the new prefab
            //Assets/PlaceAndManipulateObjects/Resources/PlaceableObjects
            string prefabPath = $"Assets/ARMagicBar/Resources/PlaceableObjects/{prefab.name}_Placeable.prefab";
            GameObject savedPrefabAsset = PrefabUtility.SaveAsPrefabAsset(newPrefab, prefabPath);
        
            // Load the saved prefab asset to get the TransformableObject component
            GameObject loadedPrefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            TransformableObject transformableObjectComponent = loadedPrefabAsset.GetComponentInChildren<TransformableObject>();

            if (transformableObjectComponent == null)
            {
                Debug.LogError($"Failed to find TransformableObject component in prefab at path: {prefabPath}");
                return;
            }
        
            // Create and set up the ScriptableObject
            PlacementObjectSO placeableObject = ScriptableObject.CreateInstance<PlacementObjectSO>();
        

            placeableObject.placementObject = transformableObjectComponent;
            placeableObject.uiSprite = image;
            placeableObject.nameOfObject = childPrefab.name;
            placeableObject.name = $"{prefab.name}_PlaceableObject";
        
            // Save the ScriptableObject
            string assetPath = $"Assets/ARMagicBar/Resources/PlaceableObjects/{prefab.name}_PlaceableObject.asset";
        
        
            AssetDatabase.CreateAsset(placeableObject, assetPath);
            AssetDatabase.SaveAssets(); 
            AssetDatabase.Refresh(); 
        
            CustomLog.Instance.InfoLog($"Created placeable object for {prefab.name}");
            AddToDatabase(placeableObject);
        
            ReferenceToSO referenceToSo = loadedPrefabAsset.GetComponent<ReferenceToSO>() ?? newPrefab.AddComponent<ReferenceToSO>();

            if (referenceToSo == null) {
                Debug.LogError("ReferenceToSO component is null.");
            } else if (placeableObject == null) {
                Debug.LogError("placeableObject is null.");
            } else
            {
                CustomLog.Instance.InfoLog("Should set correspondingObj to => " + placeableObject);
                CustomLog.Instance.InfoLog("Reference Object ToSo => " + referenceToSo);
            
                referenceToSo.SetPlacementObjectSO(placeableObject);
                EditorUtility.SetDirty(referenceToSo);
                EditorUtility.SetDirty(referenceToSo.gameObject);
            }
        
            CustomLog.Instance.InfoLog("referencetoSO GetPlacementObejctSO =>" + referenceToSo.GetPlacementObejctSO());
        
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            DestroyImmediate(newPrefab); // Destroy the temporary object
        
        }
    
    
        void DebugList(List<GameObject> list)
        {
            CustomLog.EnsureInstance();
        
            foreach (var goj in list)
            {
                CustomLog.Instance.InfoLog("LoadedPrefRefs=> " + goj.name);
            }
        
            CustomLog.Instance.InfoLog("Debug End");
        }
    }
}