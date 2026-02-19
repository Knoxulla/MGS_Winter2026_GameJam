using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField]
    private ItemCatalogue itemCatalogue;

    // Serialized dictionary for the inspector
    [SerializeField]
    public Dictionary<int, InventoryItemEntry> inventoryItems = new Dictionary<int, InventoryItemEntry>();

    public InventoryVisualizer visuals;

    private void Start()
    {
        visuals = GetComponent<InventoryVisualizer>();
    }

    // Add an item to the inventory
    public void AddItem(GameObject itemObject, int itemID, int amt)
    {
        if (!inventoryItems.ContainsKey(itemID))
        {
            inventoryItems[itemID] = new InventoryItemEntry(itemID, itemCatalogue.GetItem(itemID));
        }

        if (itemCatalogue.GetItem(itemID).maxHoldAmt <= inventoryItems[itemID].TotalCount) return;

        for (int i = 0; i < amt; i++)
        {
            inventoryItems[itemID].AddInstance(itemObject);
        }

        //if (!inventoryItems[itemID].itemEntry.PickupSound.IsNull && AudioManager.Instance != null)
        //{
        //    AudioManager.Instance.PlayOneShotUI(inventoryItems[itemID].itemEntry.PickupSound);
        //}

        visuals.UpdateInventory();
    }

    public void AddItem(int itemID, int amt)
    {
       

        if (!inventoryItems.ContainsKey(itemID))
        {
            inventoryItems[itemID] = new InventoryItemEntry(itemID, itemCatalogue.GetItem(itemID));

        }

        if (itemCatalogue.GetItem(itemID).maxHoldAmt <= inventoryItems[itemID].TotalCount) return;

            for (int i = 0; i < amt; i++)
        {
            inventoryItems[itemID].AddInstance(itemCatalogue.GetItem(itemID).itemPrefab);
        }

        //if (!inventoryItems[itemID].itemEntry.PickupSound.IsNull && AudioManager.Instance != null)
        //{
        //    AudioManager.Instance.PlayOneShotUI(inventoryItems[itemID].itemEntry.PickupSound);
        //}

        visuals.UpdateInventory();
    }

    // Check if the inventory has enough of a specific item
    public bool HasEnoughItems(int itemID, int amount)
    {
        return inventoryItems.ContainsKey(itemID) && inventoryItems[itemID].TotalCount >= amount;
    }

    // Remove a specific amount of an item from the inventory
    public void RemoveItem(int itemID, int amount)
    {
        if (inventoryItems.ContainsKey(itemID))
        {
            inventoryItems[itemID].RemoveInstances(amount);
            if (inventoryItems[itemID].TotalCount == 0)
                inventoryItems.Remove(itemID);

            visuals.UpdateInventory();
        }
    }

    public void RemoveItem(int itemID, int amount, Vector3 pos)
    {
        if (inventoryItems.ContainsKey(itemID))
        {
            
            //if (!inventoryItems[itemID].itemEntry.UseSound.IsNull && AudioManager.Instance != null)
            //{
            //    AudioManager.Instance.PlayOneShot(inventoryItems[itemID].itemEntry.UseSound, pos);
            //}

            inventoryItems[itemID].RemoveInstances(amount);
            if (inventoryItems[itemID].TotalCount == 0)
                inventoryItems.Remove(itemID);


            visuals.UpdateInventory();
        }
    }


    /// <summary>
    /// Checks to see if the item is at its max amount, returns false if not and returns true if it is.
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public bool CheckItemMaxHold(int itemID)
    {
        return inventoryItems.ContainsKey(itemID) && inventoryItems[itemID].TotalCount >= inventoryItems[itemID].GetMaxHoldAmount();
    }

    // Remove an item from the inventory using its GameObject
    public void RemoveItemByGameObject(GameObject itemObject)
    {
        foreach (var entry in inventoryItems.Values)
        {
            // Check if this entry contains the GameObject
            if (entry.itemInstances.Contains(itemObject))
            {
                // Remove the GameObject from the itemInstances list
                entry.itemInstances.Remove(itemObject);

                // If the itemInstances list is empty after removal, remove the entry from the inventory
                if (entry.TotalCount == 0)
                {
                    inventoryItems.Remove(entry.ItemID);
                }

                visuals.UpdateInventory(); // Update the inventory UI or visuals after removal
                return; // Exit once the item is found and removed
            }
        }

        Debug.LogWarning("Item not found in inventory!");
    }

    public List<GameObject> GetItemInstances(int itemID)
    {
        if (inventoryItems.ContainsKey(itemID))
        {
            return inventoryItems[itemID].itemInstances; // Return the list of GameObjects for this item
        }
        return new List<GameObject>(); // Return an empty list if the itemID doesn't exist in the inventory
    }
}

[System.Serializable]
public class InventoryItemEntry
{
    public int ItemID { get; private set; }
    public List<GameObject> itemInstances;
    public ItemCatalogue.ItemEntry itemEntry;

    public int TotalCount;

    public InventoryItemEntry(int itemID, ItemCatalogue.ItemEntry item)
    {
        ItemID = itemID;
        itemEntry = item;
        itemInstances = new List<GameObject>(); // Initialize the list
        TotalCount = 0;
    }

    // Add an instance of the item
    public void AddInstance(GameObject itemObject)
    {
        TotalCount += 1;

        if (itemObject == null) return;
        itemInstances.Add(itemObject);
    }

    // Remove a certain amount of instances
    public void RemoveInstances(int amount)
    {
        int removeCount = Mathf.Min(amount, itemInstances.Count);

        TotalCount -= removeCount;
        
        if (itemInstances == null) return;
        itemInstances.RemoveRange(0, removeCount);
    }

    // Return the max hold amount for this item
    public int GetMaxHoldAmount()
    {
        return itemEntry.maxHoldAmt;
    }
}
