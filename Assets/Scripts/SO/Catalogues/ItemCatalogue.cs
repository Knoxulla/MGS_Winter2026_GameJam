using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemCatalogue", menuName = "Catalogue/ItemCatalogue")]
public class ItemCatalogue : ScriptableObject
{
    public List<ItemEntry> items;

    public ItemEntry GetItem(int itemID)
    {
        return items.Find(item => item.itemID == itemID);
    }

    [System.Serializable]
    public class ItemEntry
    {
        [Header("Basic Info")]
        public int itemID;
        [Tooltip("This will be how they are referenced")]
        public string Name;
        public Sprite icon;
        [Tooltip("Short blurb about them, like lore")]
        public string description;

        [Space(20)]

        [Header("Inventory Information")]
        [SerializeField, Tooltip("Does this item have a max amount that can be held at any given time?")]
        public bool hasMaxHoldAmt = false;
        [SerializeField,
            Tooltip("How many of this item can the player hold at any given time?")]
        public int maxHoldAmt;

        [Header("Game Information"), Tooltip("Can be spawned as an object in scene")]
        public bool canBeEntity;
        public GameObject itemPrefab;

        [field: Space(20)]

        [Header("Pickup Information"), Tooltip("Can be spawned as a pickup in scene")]
        public bool canBePickup;
        public PickupSO Pickup;

        [field: Space(10)]

        [field: SerializeField]
        public bool OverridePickupSpawn { get; private set; }

        [field: Space(10)]

        [field: SerializeField]
        public int MinPickupValue { get; private set; }
        [field: SerializeField]
        public int MaxPickupValue { get; private set; }

        [field: Space(10)]

        [field: Tooltip("How many pickups will this spawn")]
        public int MinPickupCount = 1;

        [field: Tooltip("How many pickups will this spawn")]
        public int MaxPickupCount = 1;

        [field: Tooltip("How much time should pass between spawning pickups"), SerializeField, Range(0, 0.15f)]
        public Vector2 TimeBetweenSpawns { get; private set; }

    }
}
