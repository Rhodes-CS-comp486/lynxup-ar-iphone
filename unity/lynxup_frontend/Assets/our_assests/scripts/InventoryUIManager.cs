using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventoryPanel; // Reference to Inventory Panel
    public Button inventoryButton; // Reference to Inventory Button
    public Transform contentArea; // Reference to Scroll View Content
    public GameObject itemPrefab; // Prefab for Inventory Items

    private List<string> inventoryItems = new List<string>(); // Simulated Inventory

    void Start()
    {
        inventoryPanel.SetActive(false); // Hide inventory at start
        inventoryButton.onClick.AddListener(ToggleInventory);
    }

    void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        if (inventoryPanel.activeSelf)
        {
            LoadInventory();
        }
    }

    void LoadInventory()
    {
        // Clear previous items
        foreach (Transform child in contentArea)
        {
            Destroy(child.gameObject);
        }

        // Fetch inventory data from Firestore or local storage
        inventoryItems = GetInventoryFromBackend(); 

        // Display items
        foreach (string item in inventoryItems)
        {
            GameObject newItem = Instantiate(itemPrefab, contentArea);
            newItem.GetComponentInChildren<TMP_Text>().text = item;
        }
    }

    // Simulated function to get inventory (Replace with Firestore fetch)
    List<string> GetInventoryFromBackend()
    {
        return new List<string> { "Potion", "Magic Scroll", "Sword", "Shield" };
    }
}