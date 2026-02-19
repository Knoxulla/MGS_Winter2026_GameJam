using UnityEngine;
using System.Collections;

public class BurstPatternHandler : IBulletPatternHandler
{
    bool isBulletPattern;
    private BulletAllegiance allegiance;

    public void FirePattern(Vector2 spawnPosition, Quaternion spawnRotation, BulletPatternSO pattern, MonoBehaviour runner, bool fromBullet, BulletAllegiance incomingAllegiance)
    {
        if (runner == null)
        {
            Debug.LogError("[Burst Fire] Runner (MonoBehaviour) is null! Cannot start coroutine.");
            return;
        }

        allegiance = incomingAllegiance;
        isBulletPattern = fromBullet;
        runner.StartCoroutine(FireBurstCoroutine(spawnPosition, spawnRotation, pattern));
    }

    private IEnumerator FireBurstCoroutine(Vector2 spawnPosition, Quaternion spawnRotation, BulletPatternSO pattern)
    {
        int burstCount = Mathf.Max(1, pattern.numberOfInstances);
        float burstDelay = Mathf.Max(0f, pattern.delayBetween);

        //Debug.Log($"[Burst Fire] Starting {burstCount} bursts with {burstDelay}s delay each.");

        for (int burst = 0; burst < burstCount; burst++)
        {
            FireSingleBurst(spawnPosition, spawnRotation, pattern);
            //Debug.Log($"[Burst {burst + 1}/{burstCount}] Fired at {Time.time}");

            if (burst < burstCount - 1) // Don't delay after the last burst
                yield return new WaitForSeconds(burstDelay);
        }
    }

    private void FireSingleBurst(Vector2 spawnPosition, Quaternion spawnRotation, BulletPatternSO pattern)
    {
        int numBullets = Mathf.Max(0, pattern.bulletsToFire) + 1;
        float angleStep = pattern.spreadAngle / Mathf.Max(1, numBullets - 1);
        float startAngle = pattern.angleOffset;

        //Debug.Log($"[Burst Fire] Spawning {numBullets} bullets at {spawnPosition} with base rotation {spawnRotation.eulerAngles.z}");

        for (int i = 0; i < numBullets; i++)
        {
            float angle = startAngle + (i * angleStep);
            Quaternion bulletRotation = spawnRotation * Quaternion.Euler(0, 0, angle);

            Bullet_BASE bullet = BulletPoolManager.Instance.GetBullet();
            
            if (bullet == null)
            {
                Debug.LogWarning($"[Burst Fire] Bullet Pool is empty! Not enough bullets for burst.");
                continue; // Skip this bullet
            }

            // Offset spawn position slightly forward to avoid instant collision
            Vector3 offsetSpawnPos = (Vector3)spawnPosition + (bulletRotation * Vector3.up * 0.5f);

            bullet.transform.position = offsetSpawnPos;
            bullet.transform.rotation = bulletRotation;

            Vector3 bulletDirection = bulletRotation * Vector3.up;
            bullet.velocity = bulletDirection.normalized * pattern.bulletSpeed;
            bullet.bulletSO = pattern.bulletSO;

            // Set bullet allegiance
            bullet.SetBulletAllegiance(allegiance);

            // Apply pattern properties to bullet
            if (isBulletPattern)
            {
                bullet.SetBulletPropertiesFromPattern(pattern, allegiance);

            }
            else
            {
                bullet.SetBulletPropertiesFromBullet(pattern.bulletSO, pattern, allegiance);
            }

            //Debug.Log($"[Bullet {i}] Fired at {Time.time} | Pos: {bullet.transform.position} | Rot: {bullet.transform.rotation.eulerAngles.z}‹ | Vel: {bullet.velocity}");
        }
        
        numBullets = Mathf.Max(0, pattern.bulletsToFire);
    }
}
