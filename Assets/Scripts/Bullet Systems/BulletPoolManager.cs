using UnityEngine;
using System.Collections.Generic;

public class BulletPoolManager : Singleton<BulletPoolManager>
{
    [Tooltip("Bullet to pool.")]
    public Bullet_BASE bulletPrefab;

    [Tooltip("How many bullets to pre-instantiate in the pool.")]
    public int poolSize = 20;

    [field: SerializeField] private Queue<Bullet_BASE> bulletPool = new Queue<Bullet_BASE>();
    [SerializeField] private List<Bullet_BASE> activeBullets = new List<Bullet_BASE>(); // Track active bullets

    [field: SerializeField] 
    public GameObject bulletContainer { get; private set; }

    void Start()
    {
        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Bullet_BASE bullet = Instantiate(bulletPrefab, transform);
            if (bulletContainer != null)
                bullet.gameObject.transform.parent = bulletContainer.transform;
            else
                bullet.gameObject.transform.parent = transform;
            bullet.gameObject.SetActive(false); // Initially disable the bullet
            bulletPool.Enqueue(bullet);
        }
    }

    public Bullet_BASE GetBullet()
    {
        // Check if the pool has bullets
        if (bulletPool.Count > 0)
        {
            Bullet_BASE bullet = bulletPool.Dequeue(); // Take a bullet from the pool
            bullet.gameObject.SetActive(true);
            activeBullets.Add(bullet); // Add it to the active bullets list
            return bullet;
        }
        // If the pool is empty, and we have active bullets that we can reuse
        else if (activeBullets.Count > 0 && activeBullets.Count < poolSize)
        {
            Bullet_BASE bullet = activeBullets[0]; // Get the oldest active bullet
            activeBullets.RemoveAt(0); // Remove it from the active list
            bullet.ResetBullet(); // Reset its state, position, velocity, etc.
            bullet.gameObject.SetActive(true);
            activeBullets.Add(bullet); // Add it back to the active list
            return bullet;
        }
        // If the pool is empty and we've reached max active bullets, reuse the first one
        else if (activeBullets.Count >= poolSize)
        {
            // Reuse the first active bullet when pool size is maxed out
            Bullet_BASE bullet = activeBullets[0];
            activeBullets.RemoveAt(0); // Remove it from active list
            bullet.ResetBullet(); // Reset its state, position, etc.
            bullet.gameObject.SetActive(true); // Reactivate it
            activeBullets.Add(bullet); // Add it back to active bullets
            return bullet;
        }
        // If there is room to create a new bullet (fallback case)
        else
        {
            Bullet_BASE newBullet = Instantiate(bulletPrefab, transform);
            activeBullets.Add(newBullet); // Add the new bullet to the active list
            return newBullet; // Return the newly created bullet
        }
    }


    public void ReturnBulletToPool(Bullet_BASE bullet)
    {
        bullet.ResetBullet(); // Reset the bullet's state
        if (bulletContainer != null)
            bullet.gameObject.transform.parent = bulletContainer.transform;
        else
            bullet.gameObject.transform.parent = transform;
        bullet.gameObject.SetActive(false); // Deactivate it
        activeBullets.Remove(bullet); // Remove it from the active list
        bulletPool.Enqueue(bullet); // Return it to the pool
    }



    // Return all active bullets to the pool
    public void ReturnAllBulletsToPool()
    {
        foreach (Bullet_BASE bullet in activeBullets)
        {
            ReturnBulletToPool(bullet);
        }

        activeBullets.Clear(); // Clear the active bullets list
    }
}
