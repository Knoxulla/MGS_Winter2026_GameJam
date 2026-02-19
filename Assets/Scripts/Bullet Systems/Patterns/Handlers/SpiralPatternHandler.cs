using UnityEngine;
using System.Collections;

public class SpiralPatternHandler : IBulletPatternHandler
{
    Transform playerTransform;
    bool isBulletPattern;
    private BulletAllegiance allegiance;

    public void FirePattern(Vector2 spawnPosition, Quaternion spawnRotation, BulletPatternSO pattern, MonoBehaviour runner, bool fromBullet, BulletAllegiance incomingAllegiance)
    {
        isBulletPattern = fromBullet;
        allegiance = incomingAllegiance;

        runner.StartCoroutine(FireSpiral(spawnPosition, spawnRotation, pattern));
    }

    private IEnumerator FireSpiral(Vector2 spawnPosition, Quaternion spawnRotation, BulletPatternSO pattern)
    {
        float currentAngle = pattern.angleOffset;
        float spiralRadius = pattern.spawnDistance;  // Radius for the spiral

        // Direction modifier: positive for clockwise, negative for counter-clockwise
        float directionModifier = pattern.clockwiseSpiral ? -1f : 1f;

        // Calculate the angle step based on the total spread angle and the number of bullets
        float angleStep = pattern.spreadAngle / pattern.bulletsToFire;

        // Determine the center of the spiral
        Vector2 spiralCenter = spawnPosition;  // Default to spawn position if not player-aimed

        // If the pattern is player-aimed, check for the player transform
        if (pattern.player_aimed)
        {
            if (PlayerMASTER.Instance.transform == null)
            {
                Debug.LogError("Player Transform is null! Check if Visuals is assigned in Player_Controller.");
                yield break;
            }

            playerTransform = PlayerMASTER.Instance.transform;
            spiralCenter = (Vector2)playerTransform.position;  // Use player position as the spiral center
        }

        for (int i = 0; i < pattern.bulletsToFire; i++)
        {
            // Calculate the spawn position based on the spiral radius, current angle, and spiral center
            Vector2 bulletSpawnPosition = spiralCenter + new Vector2(Mathf.Cos(Mathf.Deg2Rad * currentAngle) * spiralRadius, Mathf.Sin(Mathf.Deg2Rad * currentAngle) * spiralRadius);

            // Bullet rotation based on the angle of the spiral
            Quaternion bulletRotation = spawnRotation * Quaternion.Euler(0, 0, currentAngle);

            Bullet_BASE bullet = BulletPoolManager.Instance.GetBullet();
            bullet.transform.position = bulletSpawnPosition;
            bullet.transform.rotation = bulletRotation;
            

            // **IMPORTANT**: Set velocity based on the spiral direction
            Vector2 spiralDirection = new Vector2(Mathf.Cos(Mathf.Deg2Rad * currentAngle), Mathf.Sin(Mathf.Deg2Rad * currentAngle));  // Direction of the spiral
            bullet.velocity = spiralDirection * pattern.bulletSpeed;
            //Debug.Log($"Before: bullet.bulletSO = {bullet.bulletSO.name}");
            bullet.bulletSO = pattern.bulletSO;
            //Debug.Log($"After: bullet.bulletSO = {bullet.bulletSO.name}");

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

            // Increment the angle for the next bullet in the spiral
            currentAngle += angleStep * directionModifier;  // Apply direction modifier

            // Wait for the specified delay between bullets
            yield return new WaitForSeconds(pattern.bulletDelay);
        }
    }

}
