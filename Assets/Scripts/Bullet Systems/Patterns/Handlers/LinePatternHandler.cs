using UnityEngine;
using System.Collections;

public class LinearPatternHandler : IBulletPatternHandler
{
    private Transform playerTransform;
    bool isBulletPattern;
    private BulletAllegiance allegiance;
    float damage = 1f;
    public void FirePattern(Vector2 spawnPosition, Quaternion spawnRotation, BulletPatternSO pattern, MonoBehaviour runner, bool fromBullet, BulletAllegiance incomingAllegiance)
    {
        if (runner == null)
        {
            Debug.LogError("[Linear Fire] Runner (MonoBehaviour) is null! Cannot start coroutine.");
            return;
        }

        allegiance = incomingAllegiance;
        isBulletPattern = fromBullet;
        runner.StartCoroutine(FireLinearCoroutine(spawnPosition, spawnRotation, pattern, allegiance));
    }

    private IEnumerator FireLinearCoroutine(Vector2 spawnPosition, Quaternion spawnRotation, BulletPatternSO pattern, BulletAllegiance incomingAllegiance)
    {
        int instanceCount = Mathf.Max(1, pattern.numberOfInstances);
        int numberOfBullets = Mathf.Max(1, pattern.bulletsToFire);
        float instanceDelay = Mathf.Max(0f, pattern.delayBetween);

        //if (Player_Controller.instance == null ||
        //    Player_Controller.instance.ActiveCharacter == null ||
        //    Player_Controller.instance.ActiveCharacter.playerMovement == null ||
        //    Player_Controller.instance.ActiveCharacter.playerMovement.Visuals == null)
        //{
        //    Debug.LogError("[Linear Fire] Player Transform is null! Ensure Visuals is assigned in Player_Controller.");
        //    yield break;
        //}

        playerTransform = PlayerMASTER.Instance.transform;

        damage = PlayerMASTER.Instance.playerAttackController.modifiedDmg;

        for (int instance = 0; instance < instanceCount; instance++)
        {
            for (int i = 0; i < numberOfBullets; i++)
            {
                FireSingleBullet(spawnPosition, spawnRotation, pattern, incomingAllegiance);
            }

            if (instance < instanceCount - 1)
                yield return new WaitForSeconds(instanceDelay);
        }
    }

    private void FireSingleBullet(Vector2 spawnPosition, Quaternion spawnRotation, BulletPatternSO pattern, BulletAllegiance incomingAllegiance)
    {
        Bullet_BASE bullet = BulletPoolManager.Instance.GetBullet();
        if (bullet == null)
        {
            Debug.LogWarning("[Linear Fire] Bullet Pool is empty! Skipping bullet spawn.");
            return;
        }

        Vector2 bulletDirection;

        if (pattern.player_aimed)
        {
            // Aim at player
            bulletDirection = (playerTransform.position - (Vector3)spawnPosition).normalized;
        }
        else if ((pattern.defaultDirection.x != 0 || pattern.defaultDirection.y != 0) && incomingAllegiance == BulletAllegiance.Hostile)
        {


            // Use predefined default direction from BulletPatternSO
            bulletDirection = pattern.defaultDirection.normalized;

            if (bulletDirection == Vector2.zero && !pattern.useBezierPath)
            {
                //Debug.LogWarning("[Linear Fire] defaultDirection is zero! Defaulting to upward.");
                bulletDirection = Vector2.up;
            }
        }
        else
        {
            // Use spawn position
            bulletDirection = -1 * (playerTransform.position - (Vector3)spawnPosition).normalized;
        }

        //Debug.Log("Bullet Direction: " + bulletDirection);
        // Set bullet position and rotation
        bullet.transform.SetPositionAndRotation(spawnPosition, spawnRotation);


        float angle;
        if (pattern.player_aimed)
        {
            angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg; // Adjust for upward rotation
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
        else
        {
            bullet.transform.rotation = spawnRotation;
        }
        bullet.bulletSO = pattern.bulletSO;
        // Apply velocity
        
        bullet.velocity = bulletDirection.normalized * pattern.bulletSpeed;
        //Debug.LogWarning("Bullet Velocity: " + bullet.velocity);
        // Set bullet allegiance
        bullet.SetBulletAllegiance(incomingAllegiance);
        bullet.damage = damage;

        // Apply pattern properties to bullet
        if (isBulletPattern)
        {
            bullet.SetBulletPropertiesFromPattern(pattern, incomingAllegiance);

        }
        else
        {
            bullet.SetBulletPropertiesFromBullet(pattern.bulletSO, pattern, incomingAllegiance);
        }
    }
}
