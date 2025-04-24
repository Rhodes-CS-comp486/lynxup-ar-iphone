using System.Collections.Generic;
using UnityEngine;


public class PrefabRegistry : MonoBehaviour
{
    [System.Serializable]
    public class PrefabMapping
    {
        public string databaseName;
        public GameObject prefab;
    }
    public static PrefabRegistry Instance { get; private set; }

    public List<PrefabMapping> mappings;
    public Dictionary<string, GameObject> prefabLookup = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        foreach (var mapping in mappings)
        {
            prefabLookup[mapping.databaseName] = mapping.prefab;
        }

        DontDestroyOnLoad(gameObject); // Optional: persist across scenes
    }

    public GameObject GetPrefab(string name)
    {
        prefabLookup.TryGetValue(name, out var prefab);
        return prefab;
    }
}