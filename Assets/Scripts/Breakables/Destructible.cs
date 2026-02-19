using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
public class Destructible : EnemyHealthController
{
    [Header("Destructible Type Controls"), SerializeField] DestructibleType destructibleType;

    
    [Header("Layer Control"), SerializeField] private List<LayerMask> destructionLayers;
    [Header("Layer Control"), SerializeField] private LayerMask enemyLayer;
    [Header("Layer Control"), SerializeField] private LayerMask playerLayer;

    [Header("Loot Tables"), SerializeField]
    private bool hasLoot;
    [Header("Loot Tables"), SerializeField]
    private LootRarity lootRarity;
    [Header("Loot Tables"), SerializeField]
    private int numOfDrops;
    [Header("Loot Tables"), SerializeField, 
    Tooltip("Radius around the object that the loot will randomly spawn in")]
    private float spawnRadius;
    [Header("Loot Tables"), SerializeField]
    private LootTableSO rareLoot;
    [Header("Loot Tables"), SerializeField]
    private LootTableSO uncommonLoot;
    [Header("Loot Tables"), SerializeField]
    private LootTableSO commonLoot;

    [Header("Respawning"), SerializeField]
    private bool canRespawn;
    [Header("Respawning"), SerializeField]
    private float respawnTime = 2;

    private void Start()
    {
        //if (health == null)
        //{
        //    health = GetComponent<EnemyHealth_BASE>();
        //}

        ClearDestructionLayers();

        switch (destructibleType)
        {
            case DestructibleType.Basic:
                SetBasic();
                break;
            case DestructibleType.Cover:
                SetCover();
                break;
            case DestructibleType.Lootbox:
                SetLootbox();
                break;
            case DestructibleType.Unbreakable:
                break;
            default:
                SetBasic();
                break;
        }

        //health.EnemyDamaged += OnDeath;
    }

   


    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (LayerMask layer in destructionLayers)
        {
            if (GameManager.Instance.ContainsLayer(layer, collision.gameObject.layer))
            {
                if (destructibleType == DestructibleType.Unbreakable) return;

               
            }
        }
    }


    private void ClearDestructionLayers()
    {
        destructionLayers.Clear();
    }

    private void SetBasic()
    {
        ClearDestructionLayers();
        ResetCurrentHP();
        destructionLayers.Add(playerLayer);
    }

    private void SetCover()
    {
        ClearDestructionLayers();
        ResetCurrentHP();
        destructionLayers.Add(playerLayer);
        destructionLayers.Add(enemyLayer);
    }

    private void SetLootbox()
    {
        ClearDestructionLayers();
        ResetCurrentHP();
        destructionLayers.Add(playerLayer);
    }

    private void ResetCurrentHP()
    {
        currentHealth = maxHealth;
    }

    public void OnDeath()
    {
        if (currentHealth > 0) return;

        //if (!DeathSound.IsNull)
        //    AudioManager.Instance.PlayOneShot(DeathSound, transform.position);

        if (canRespawn)
        {
            StartCoroutine(StartRespawnTimer());
            
            gameObject.SetActive(false);
        }
        else
        {
           
            gameObject.SetActive(false);
        }

        

        if (!hasLoot) return;

        switch (lootRarity)
        {
            case LootRarity.None:
                return;
            case LootRarity.Common:
                commonLoot.SpawnRandomLoot(SpawnItem(), numOfDrops);
                break;
            case LootRarity.Uncommon:
                uncommonLoot.SpawnRandomLoot(SpawnItem(), numOfDrops);
                break;
            case LootRarity.Rare:
                rareLoot.SpawnRandomLoot(SpawnItem(), numOfDrops);
                break;
        }
    }

    IEnumerator StartRespawnTimer()
    {
        yield return new WaitForSeconds(respawnTime);
        gameObject.SetActive(true);
    }

    private Vector2 SpawnItem()
    {
        // Get a random angle in radians (0 to 360 degrees)
        float randomAngle = Random.Range(0f, 360f);
        Transform middleTransform = transform;

        // Convert that angle to a direction (unit vector)
        Vector2 randomDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));

        // Scale that direction by the spawn radius to determine a random position
        return (Vector2)middleTransform.position + randomDirection * Random.Range(0f, spawnRadius);
    }
    enum LootRarity
    { 
        None,
        Common,
        Uncommon,
        Rare
    }

    enum DestructibleType
    { 
        Basic,
        Cover,
        Unbreakable,
        Lootbox
    }
}
