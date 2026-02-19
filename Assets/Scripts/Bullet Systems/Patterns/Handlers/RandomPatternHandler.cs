using UnityEngine;
using System.Collections;

public class RandomPatternHandler : IBulletPatternHandler
{
    bool isBulletPattern;
    private BulletAllegiance allegiance;

    public void FirePattern(Vector2 spawnPosition, Quaternion spawnRotation, BulletPatternSO pattern, MonoBehaviour runner, bool fromBullet, BulletAllegiance incomingAllegiance)
    {
        allegiance = incomingAllegiance;

        for (int i = 0; i < pattern.bulletsToFire; i++)
        {
            float randomAngle = Random.Range(-pattern.spreadAngle / 2f, pattern.spreadAngle / 2f);
            Quaternion bulletRotation = spawnRotation * Quaternion.Euler(0, 0, randomAngle);
            Bullet_BASE bullet = BulletPoolManager.Instance.GetBullet();
            bullet.transform.position = spawnPosition;
            bullet.transform.rotation = bulletRotation;
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

            if (pattern.player_aimed)
            {
                bullet.velocity = (bullet.playerTransform.position - bullet.transform.position).normalized * pattern.bulletSpeed;
            }
            else
            {
                bullet.velocity = Vector2.up.normalized * pattern.bulletSpeed;
            }

            // Apply any delays as necessary
            if (pattern.bulletDelay > 0)
            {
                runner.StartCoroutine(ApplyBulletDelay(pattern.bulletDelay));
            }
        }
    }

    private IEnumerator ApplyBulletDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
