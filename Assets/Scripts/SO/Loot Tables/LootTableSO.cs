using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(fileName = "LootTable_", menuName = "Mechanics/Drops/Create LootTable...")]
public class LootTableSO : ScriptableObject
{
    public List<LootEntry> lootEntries;

    [System.Serializable]
    public class LootEntry
    {
        [Header("Item Reference")]
        public ItemCatalogue.ItemEntry item;  // Reference to the ItemEntry from the ItemCatalogue
        [Range(0, 100), Tooltip("Chance for this item to drop (percentage)")]
        public float dropChance;  // Drop chance in percentage
        [Tooltip("How many of this item are dropped")]
        public int quantity;  // Number of items dropped
    }

    // Method to spawn loot based on how many items should drop at once
    public void SpawnRandomLoot(Vector3 spawnLocation, int itemsToDrop)
    {
        // Ensure the number of items to drop is positive
        if (itemsToDrop <= 0) return;

        // Loop through each loot entry and determine if it should drop
        int itemsDropped = 0;

        while (itemsDropped < itemsToDrop)
        {
            // Pick a random loot entry
            LootEntry lootEntry = lootEntries[Random.Range(0, lootEntries.Count)];

            // Check if this item should drop based on its drop chance
            float roll = Random.Range(0f, 100f);
            if (roll <= lootEntry.dropChance)
            {
                // If the item can be spawned as an entity, spawn it
                if (lootEntry.item.canBeEntity && lootEntry.item.itemPrefab != null)
                {
                    // Spawn the specified quantity of the item
                    for (int i = 0; i < lootEntry.quantity; i++)
                    {
                        Instantiate(lootEntry.item.itemPrefab, spawnLocation, Quaternion.identity);
                        itemsDropped++;
                        if (itemsDropped >= itemsToDrop) break;  // Stop once we've dropped the required number of items
                    }
                }

                //if (lootEntry.item.canBePickup && lootEntry.item.Pickup != null)
                //{
                //    // Spawn the specified quantity of the item
                //    for (int i = 0; i < lootEntry.quantity; i++)
                //    {
                //        Instantiate(lootEntry.item.itemPrefab, spawnLocation, Quaternion.identity);
                //        itemsDropped++;
                //        if (itemsDropped >= itemsToDrop) break;  // Stop once we've dropped the required number of items
                //    }
                //}

            }

            // Exit the loop early if we've already spawned enough items
            if (itemsDropped >= itemsToDrop) break;
        }
    }

    // Tries to spawn all loot at least once
    public void SpawnGuaranteedLoot(Transform SpawnPoint = null)
    {
        // If the item can be spawned as a pickup, spawn it
        if (Pickup_Pool_Master.instance == null) return;

        foreach (LootEntry entry in lootEntries)
        {
            float roll = Random.Range(0f, 100f);

            if (roll <= entry.dropChance)
            {
                if (entry.item.canBePickup)
                {
                    int setMinCount, setMaxCount;

                    setMinCount = entry.item.MinPickupCount;
                    setMaxCount = entry.item.MaxPickupCount;

                    //if (entry.OverridePickupCount)
                    //{
                    //    setMinCount = entry.Override_MinPickupCount;
                    //    setMaxCount = entry.Override_MaxPickupCount;
                    //}
                    //else
                    //{
                    //    setMinCount = entry.item.MinPickupCount;
                    //    setMaxCount = entry.item.MaxPickupCount;
                    //}

                    if (entry.item.MinPickupCount > 0)
                    {
                        int PickupsToSpawn;

                        if (setMinCount < setMaxCount)
                            PickupsToSpawn = Random.Range(setMinCount, setMaxCount);
                        else if (setMinCount > 0)
                            PickupsToSpawn = setMinCount;
                        else
                            PickupsToSpawn = 0;

                        if (PickupsToSpawn == 1)
                        {
                            // spawn 1 pickup
                            if (SpawnPoint != null)
                            {
                                Pickup_Pool_Master.instance.SpawnPickupFromLootTable(entry.item, SpawnPoint);
                            }
                            else
                                Pickup_Pool_Master.instance.SpawnPickupFromLootTable(entry.item);
                        }
                        else if (PickupsToSpawn > 1)
                        {
                            // spawn multiple pickups
                            if (SpawnPoint != null)
                                Pickup_Pool_Master.instance.MultiSpawnPickupsFromLootTable(PickupsToSpawn, entry.item, SpawnPoint);
                            else
                                Pickup_Pool_Master.instance.MultiSpawnPickupsFromLootTable(PickupsToSpawn, entry.item);
                        }
                        else
                        {
                            // You get NOTHING
                            continue;
                        }
                    }
                }
                else
                {
                    // You get NOTHING
                    continue;
                }
            }
            
        }
    }


    
}
