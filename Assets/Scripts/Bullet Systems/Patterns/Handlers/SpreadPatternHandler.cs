using UnityEngine;
using System.Collections;

public class SpreadPatternHandler : IBulletPatternHandler
{
    public Transform playerTransform;
    private bool isBulletPattern;
    private BulletAllegiance allegiance;

    public void FirePattern(Vector2 spawnPosition, Quaternion spawnRotation, BulletPatternSO pattern, MonoBehaviour runner, bool fromBullet, BulletAllegiance incomingAllegiance)
    {
        if (runner == null)
        {
            Debug.LogError("[Cleave Fire] Runner (MonoBehaviour) is null! Cannot start coroutine.");
            return;
        }

        allegiance = incomingAllegiance;
        isBulletPattern = fromBullet;
        runner.StartCoroutine(FireCleaveCoroutine(spawnPosition, spawnRotation, pattern));
    }

    private IEnumerator FireCleaveCoroutine(Vector2 spawnPosition, Quaternion spawnRotation, BulletPatternSO pattern)
    {
        int numberOfBursts = Mathf.Max(1, pattern.numberOfInstances);
        float delayBetweenBursts = Mathf.Max(0f, pattern.delayBetween);

        for (int burstIndex = 0; burstIndex < numberOfBursts; burstIndex++)
        {
            FireSingleCleave(spawnPosition, spawnRotation, pattern);

            if (burstIndex < numberOfBursts - 1) // No delay after the last burst
                yield return new WaitForSeconds(delayBetweenBursts);
        }
    }

    private void FireSingleCleave(Vector2 spawnPosition, Quaternion spawnRotation, BulletPatternSO pattern)
    {
        int numberOfBullets = Mathf.Max(1, pattern.bulletsToFire);
        float angleBetweenBullets = (numberOfBullets > 1) ? pattern.spreadAngle / (numberOfBullets - 1) : 0f;

        if (PlayerMASTER.Instance.transform == null)
        {
            Debug.LogError("Player Transform is null! Check if Visuals is assigned in Player_Controller.");
            return;
        }

        playerTransform = PlayerMASTER.Instance.playerAttackController.transform;

        // Use the mouse position as the center of the spread (i.e., the player's aim)
        Vector2 centerPosition = PlayerMASTER.Instance.playerAttackController.mousePos;

        // **Calculate the angle** between the spawn position and the mouse position (center of the spread)
        float angleToMouse = Mathf.Atan2(centerPosition.y - spawnPosition.y, centerPosition.x - spawnPosition.x) * Mathf.Rad2Deg;

        // **Starting angle** will be the angle to the mouse minus half the spread angle (for symmetrical spread)
        float startingAngle = angleToMouse - (pattern.spreadAngle * 0.5f);

        if (pattern.angleOffset != 0)
        {
            startingAngle = pattern.angleOffset; // If the pattern has an angle offset, use it
        }

        // Loop through and fire the bullets
        for (int bulletIndex = 0; bulletIndex < numberOfBullets; bulletIndex++)
        {
            // Calculate the angle for each bullet based on the starting angle and spread
            float bulletAngle = startingAngle + (bulletIndex * angleBetweenBullets);

            // Create the bullet rotation based on the calculated bullet angle
            Quaternion bulletRotation = Quaternion.Euler(0, 0, bulletAngle);

            Bullet_BASE bullet = BulletPoolManager.Instance.GetBullet();
            if (bullet == null)
            {
                Debug.LogWarning("[Spread Fire] Bullet Pool is empty! Not enough bullets for burst.");
                continue;
            }

            // Calculate the direction based on the bullet's angle
            Vector3 bulletDirection = new Vector3(Mathf.Cos(bulletAngle * Mathf.Deg2Rad), Mathf.Sin(bulletAngle * Mathf.Deg2Rad), 0f);

            // Set bullet properties (position, velocity, etc.)
            Vector3 spawnPositionOffset = (Vector3)spawnPosition + (Vector3.up * pattern.spawnDistance);
            bullet.transform.position = spawnPositionOffset;
            bullet.transform.rotation = bulletRotation;
            bullet.velocity = bulletDirection.normalized * pattern.bulletSpeed;
            bullet.bulletSO = pattern.bulletSO;

            // Set bullet allegiance (hostile or friendly)
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



//private void FireSingleCleave(Vector2 spawnPosition, Quaternion spawnRotation, BulletPatternSO pattern)
//{
//    int numberOfBullets = Mathf.Max(1, pattern.bulletsToFire);
//    float angleBetweenBullets = (numberOfBullets > 1) ? pattern.spreadAngle / (numberOfBullets - 1) : 0f;



//    if (Player_Controller.instance.ActiveCharacter.playerMovement.Visuals.transform == null)
//    {
//        Debug.LogError("Player Transform is null! Check if Visuals is assigned in Player_Controller.");
//        return;
//    }
//    else
//    {
//        PlayerTransform = Player_Controller.instance.ActiveCharacter.playerMovement.Visuals.transform;
//    }

//    Vector2 centerPosition;

//    if (pattern.player_aimed)
//    {
//        PlayerTransform = Player_Controller.instance.ActiveCharacter.playerMovement.Visuals.transform;
//        centerPosition = (Vector2)PlayerTransform.position;
//        //Debug.Log($"Player pos: {PlayerTransform.position}, centre: {centerPosition}");
//    }
//    else
//    {
//        centerPosition = spawnPosition;
//    }

//    // **Get angle to player**: Calculate the angle towards the player (center of spread).
//    float angleToPlayer = Mathf.Atan2(centerPosition.y - spawnPosition.y, centerPosition.x - spawnPosition.x) * Mathf.Rad2Deg;
//    //Debug.Log($"Angle To Player: {angleToPlayer}");

//    // **Start angle**: Offset to make sure the spread is correctly centered around `angleToPlayer`
//    float startingAngle = angleToPlayer - (pattern.spreadAngle * 0.5f);

//    for (int bulletIndex = 0; bulletIndex < numberOfBullets; bulletIndex++)
//    {
//        // **Calculate actual bullet angle**: Apply spread offset from the center angle.
//        float bulletAngle = startingAngle + (bulletIndex * angleBetweenBullets);

//        // only bullet index 1 is working to aim at player  (cannot + 1 tp bullet index, it breaks)
//        float bulletX = Mathf.Cos(bulletIndex * angleBetweenBullets * PlayerTransform.position.x) - Mathf.Sin(bulletIndex * angleBetweenBullets * PlayerTransform.position.y);
//        float bulletY = Mathf.Sin(bulletIndex * angleBetweenBullets * PlayerTransform.position.x) + Mathf.Cos(bulletIndex * angleBetweenBullets * PlayerTransform.position.y);

//        //if (numberOfBullets / 2 == 0 && bulletIndex == 0) // if event
//        //{
//        //    bulletX = Mathf.Cos(bulletIndex * angleBetweenBullets/2 * PlayerTransform.position.x) - Mathf.Sin(bulletIndex * angleBetweenBullets/2 * PlayerTransform.position.y);
//        //    bulletY = Mathf.Sin(bulletIndex * angleBetweenBullets/2 * PlayerTransform.position.x) + Mathf.Cos(bulletIndex * angleBetweenBullets/2 * PlayerTransform.position.y);

//        //}

//        Quaternion bulletRotation = Quaternion.Euler(0, 0, bulletAngle);

//        Bullet bullet = BulletPoolManager.Instance.GetBullet();
//        if (bullet == null)
//        {
//            Debug.LogWarning("[Linear Fire] Bullet Pool is empty! Not enough bullets for burst.");
//            continue;
//        }

//        // **Spawn the bullet**: Always start at the `spawnPosition`
//        Vector3 spawnPositionOffset = (Vector3)spawnPosition + (Vector3.up * pattern.spawnDistance);
//        bullet.transform.position = spawnPositionOffset;
//        bullet.transform.rotation = bulletRotation;

//        // **Bullet movement direction**: Always shoot outward based on the rotation
//        Vector3 bulletDirection = new Vector3(bulletX, bulletY, 0f);
//        //Debug.Log($"x: {bulletX}, y: {bulletY}, Direction: {bulletDirection}, Player Pos: {PlayerTransform.position}");

//        bullet.velocity = bulletDirection.normalized * pattern.bulletSpeed;

//        bullet.SetBulletProperties(pattern);


//    }
//}
//}

