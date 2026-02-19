using UnityEngine;
using UnityEngine.UI; // Include if you're planning to use UI
using TMPro;
public class InventoryVisualizer : MonoBehaviour
{
    [SerializeField]
    private InventoryManager inventoryManager; // Reference to the InventoryManager
    [SerializeField]
    private Transform inventoryPanel; // A parent UI element to display inventory

    [Header("UI Elements")]
    [SerializeField]
    private GameObject itemUIPrefab; // A prefab to represent each inventory item in the UI (optional)

    private void OnEnable()
    {
        UpdateInventory();
    }



    // Call this method whenever you need to update the inventory visual representation
    public void UpdateInventory()
    {
        ClearInventoryUI();

        // Loop through each inventory item and create a visual element for it
        foreach (var itemEntry in inventoryManager.inventoryItems)
        {
            // Create an item UI (you can replace this with your own UI prefab or logic)
            GameObject itemUI = Instantiate(itemUIPrefab, inventoryPanel);
            ItemEntryVisual itemVisual = itemUI.GetComponent<ItemEntryVisual>();

            if (itemVisual.mainImg != null)
            {
                itemVisual.mainImg.sprite = itemEntry.Value.itemEntry.icon;
                itemVisual.mainImg.preserveAspect = true;
            }

            if (itemVisual.mainImgShadow != null)
            {
                itemVisual.mainImgShadow.sprite = itemEntry.Value.itemEntry.icon;
                itemVisual.mainImgShadow.preserveAspect = true;
            }

            if (itemVisual.invText != null)
            {
                itemVisual.invText.text = $"x{itemEntry.Value.TotalCount}";
            }
        }
    }

    // Clear out the old inventory UI
    private void ClearInventoryUI()
    {
        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }
    }
}
