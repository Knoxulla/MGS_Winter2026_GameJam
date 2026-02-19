using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pickup_Pool_Master : MonoBehaviour
{
    public static Pickup_Pool_Master instance { get; private set; }

    [field: SerializeField]
    public Pickup_BASE PooledPickup_Prefab { get; set; }
    [field: SerializeField]
    public Transform PickupSpawnTransform { get; set; }




    [field: SerializeField]
    public int SoftLimitPickupCount { get; private set; }

    [field: SerializeField]
    public int HardLimitPickupCount { get; private set; }

    [field: SerializeField]
    public Transform PickupPoolContainer { get; set; }
    public List<Pickup_BASE> ActivePickups = new();

    [field: Space(10)]

    [field: SerializeField]
    public PickupSO SetPickupSO { get; private set; }

    [field: SerializeField]
    public PickupSO DefaultPickupSO { get; private set; }

    int currentMinValue, currentMaxValue;

    void Awake()
    {
        SetupInstance();
        SetDefaultVariables();
        SetupPool();
    }

    private void Update()
    {
        if (ActivePickups.Count <= 0) return;

        foreach (Pickup_BASE pickup in ActivePickups)
        {
            if (pickup.CurrentSO == null || !pickup.ShouldRotate) continue;
            if (pickup.PickupSpriteRenderer == null) continue;

            if (pickup.CurrentSO.RotationSpeed != 0)
                pickup.PickupSpriteRenderer.transform.rotation *= Quaternion.Euler(0f, pickup.CurrentSO.RotationSpeed * Time.deltaTime, 0f);

            if (pickup.FloatAmp != 0 && pickup.FloatSpeed != 0)
            {
                Vector3 tempPos = pickup.transform.position;

                tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * pickup.FloatSpeed) * pickup.FloatAmp;
                pickup.PickupSpriteRenderer.transform.position = tempPos;
            }
        }
    }

    void SetupInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void SetDefaultVariables()
    {
        if (SoftLimitPickupCount < 5)
            SoftLimitPickupCount = 5;

        if (HardLimitPickupCount < 25)
            HardLimitPickupCount = 25;
    }

    void SetupPool()
    {
        //_PickupPool = new ObjectPool<Pickup_BASE>(
        //    CreatePickup,
        //    OnTakePickupFromPool,
        //    OnReturnPickupToPool,
        //    OnDestroyPickupLikeFrFr,
        //    true,
        //    SoftLimitPickupCount,
        //    HardLimitPickupCount);
    }

    private Pickup_BASE CreatePickup()
    {
        if (PickupSpawnTransform == null)
            SetPickupSpawnPoint(transform);

        Pickup_BASE particleToBePooled = Instantiate(
            PooledPickup_Prefab,
            PickupSpawnTransform.transform.position,
            PickupSpawnTransform.transform.rotation);

        // Assign pool
        //particleToBePooled.SetPool(_PickupPool);

        particleToBePooled.gameObject.transform.SetParent(PickupPoolContainer);


        return particleToBePooled;
    }

    private void OnTakePickupFromPool(Pickup_BASE pooledPickup)
    {
        //activate
        pooledPickup.gameObject.SetActive(true);
        if (!ActivePickups.Contains(pooledPickup))
            ActivePickups.Add(pooledPickup);

        if (PickupSpawnTransform != null)
        {
            //Reset transform and rotation
            pooledPickup.transform.SetPositionAndRotation(
                PickupSpawnTransform.transform.position,
                PickupSpawnTransform.transform.rotation);
        }
        else
        {
            //Debug.LogError("Invalid Spawn Position");
            //return;
        }

        //Set PickupSO
        if (SetPickupSO != null)
        {
            pooledPickup.SetValue(SetPickupSO, currentMinValue, currentMaxValue);
            SetPickupSO = null;
        }
        else
        {
            //pooledPickup.SetParticleSO(defaultParticleSO);
            //Debug.LogError("Particle was spawned without ParticleSO");
        }


    }

    private void OnReturnPickupToPool(Pickup_BASE pickupToBeReturned)
    {
        if (ActivePickups.Contains(pickupToBeReturned))
            ActivePickups.Remove(pickupToBeReturned);

        pickupToBeReturned.gameObject.SetActive(false);
    }

    private void OnDestroyPickupLikeFrFr(Pickup_BASE pickup)
    {
        Destroy(pickup.gameObject);
    }

    public void DestroyAllPickups()
    {
        foreach (Pickup_BASE particle in ActivePickups)
        {
            particle.DestroyPickup();
        }
    }

    public void SummonPickup(Transform spawnPoint, PickupSO incomingPickupSO, int MinValue, int MaxValue = 0)
    {
        if (spawnPoint == null || incomingPickupSO == null) return;

        currentMinValue = MinValue;
        currentMaxValue = MaxValue;
        SetPickupSpawnPoint(spawnPoint);

        SetPickupSO = incomingPickupSO;

        //_PickupPool.Get();
    }

    public void MultiSpawnPickupsFromLootTable(int incomingPickupsToSpawn, ItemCatalogue.ItemEntry incomingItem, Transform incomingSpawnPoint = null)
    {
        StartCoroutine(MultiSpawnPickupsCoroutine(incomingPickupsToSpawn, incomingItem, incomingSpawnPoint));
    }

    IEnumerator MultiSpawnPickupsCoroutine(int incomingPickupsToSpawn, ItemCatalogue.ItemEntry incomingItem, Transform incomingSpawnPoint = null)
    {
        //Debug.LogError($"{incomingItem.Name}: Spawning {incomingPickupsToSpawn} pickups");
        float timeBetweenSpawn;

        for (int i = 0; i <= incomingPickupsToSpawn; i++)
        {
            SpawnPickupFromLootTable(incomingItem, incomingSpawnPoint);

            timeBetweenSpawn = Random.Range(incomingItem.TimeBetweenSpawns.x, incomingItem.TimeBetweenSpawns.y);

            yield return new WaitForSeconds(timeBetweenSpawn);
        }
        yield return null;
    }

    public void SpawnPickupFromLootTable(ItemCatalogue.ItemEntry incomingItem, Transform incomingSpawnPoint = null)
    {
        if (incomingItem.Pickup == null) return;

        //PickupSpawnPoint pickupSpawnPoint;

        //if (incomingItem.OverridePickupSpawn)
        //    pickupSpawnPoint = incomingItem.PickupSpawnOverride;
        //else
        //    pickupSpawnPoint = incomingItem.Pickup.PickupSpawnPosition;


        if (incomingSpawnPoint != null)
        {
            SummonPickup
                (
                incomingSpawnPoint,
                incomingItem.Pickup,
                incomingItem.MinPickupValue,
                incomingItem.MaxPickupValue);
        }


    }

    public void SetPickupSpawnPoint(Transform spawnPoint = null)
    {
        if (PickupSpawnTransform == spawnPoint) return; // if it's the same position, no need to set it

        PickupSpawnTransform = spawnPoint;
    }
}
