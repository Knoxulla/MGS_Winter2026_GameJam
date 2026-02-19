using UnityEngine;
using System.Collections;

public class ReverseBurstPatternHandler : IBulletPatternHandler
{
    private Vector2 centerPosition; // Stores the center the bullets move toward
    Transform playerTransform;
    bool isBulletPattern;
    private BulletAllegiance allegiance;

    public void FirePattern(Vector2 spawnPosition, Quaternion spawnRotation, BulletPatternSO pattern, MonoBehaviour runner, bool fromBullet, BulletAllegiance incomingAllegiance)
    {
        if (runner == null)
        {
            Debug.LogError("[Reverse Burst Fire] Runner (MonoBehaviour) is null! Cannot start coroutine.");
            return;
        }

        // If player_aimed, set centerPosition to the player's position at the start
        if (pattern.player_aimed && PlayerMASTER.Instance.transform != null)
        {
            playerTransform = PlayerMASTER.Instance.transform;

            centerPosition = playerTransform.position;
        }
        else
        {
            centerPosition = spawnPosition;
        }

        allegiance = incomingAllegiance;
        isBulletPattern = fromBullet;
        runner.StartCoroutine(FireBurstCoroutine(centerPosition, spawnRotation, pattern));
    }

    private IEnumerator FireBurstCoroutine(Vector2 spawnPosition, Quaternion spawnRotation, BulletPatternSO pattern)
    {
        int burstCount = Mathf.Max(1, pattern.numberOfInstances);
        float burstDelay = Mathf.Max(0f, pattern.delayBetween);

        for (int burst = 0; burst < burstCount; burst++)
        {
            FireSingleBurst(spawnRotation, pattern);
            if (burst < burstCount - 1)
                yield return new WaitForSeconds(burstDelay);
        }
    }

    private void FireSingleBurst(Quaternion spawnRotation, BulletPatternSO pattern)
    {
        int numBullets = Mathf.Max(1, pattern.bulletsToFire);
        float angleStep = pattern.spreadAngle / Mathf.Max(1, numBullets - 1);
        float startAngle = pattern.angleOffset - (pattern.spreadAngle * 0.5f);

        for (int i = 0; i < numBullets; i++)
        {
            float angle = startAngle + (i * angleStep);
            Quaternion bulletRotation = spawnRotation * Quaternion.Euler(0, 0, angle);

            Bullet_BASE bullet = BulletPoolManager.Instance.GetBullet();
            if (bullet == null)
            {
                Debug.LogWarning("[Reverse Burst Fire] Bullet Pool is empty! Skipping bullet.");
                continue;
            }

            // Spawn bullets at the spread angle around the center position
            Vector2 spawnPos = centerPosition + (Vector2)(bulletRotation * Vector3.up * pattern.spawnDistance);
            bullet.transform.position = spawnPos;
            bullet.transform.rotation = bulletRotation;

            // Move the bullets inward toward the center position
            Vector2 directionToCenter = (centerPosition - spawnPos).normalized;
            bullet.velocity = directionToCenter * pattern.bulletSpeed;

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
        }
    }
}
