using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "AttackSO_", menuName = "Bullet Hell Mechanics/Create Attack...")]
public class AttackSO : ScriptableObject
{
    [Tooltip("Delay before this attack sequence starts")]
    public float attackWindUpTime;

    [Tooltip("Patterns within this attack")]
    public BulletPatternSO[] patterns;

    [Tooltip("Toggle to combine all patterns in list")]
    public bool fireAllPatternsAtOnce = false;

    public void FireAttack(BulletSpawner spawner)
    {
        spawner.StartCoroutine(FirePatterns(spawner));
    }

    private IEnumerator FirePatterns(BulletSpawner spawner)
    {
        if (fireAllPatternsAtOnce)
        {
            yield return new WaitForSeconds(attackWindUpTime); // Apply initial delay before firing everything at once

            foreach (var pattern in patterns)
            {
                FirePatternWithPlayerAiming(pattern, spawner);
            }
        }
        else
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                float delay = (i == 0) ? attackWindUpTime + patterns[i].windUpTime : patterns[i].windUpTime;

                yield return new WaitForSeconds(delay);

                FirePatternWithPlayerAiming(patterns[i], spawner);
            }
        }
    }


    private void FirePatternWithPlayerAiming(BulletPatternSO pattern, BulletSpawner spawner)
    {
        Transform spawnPoint = spawner.spawnPoint;
        Quaternion spawnRotation = spawnPoint.rotation;

        if (spawner.BulletAllegiance == BulletAllegiance.Friendly)
        {
            switch (pattern.patternType)
            {
                case BulletPatternSO.BulletPatternType.Random:
                    // Firing in a random direction within a defined degree range based on spread angle
                    float randomAngle = Random.Range(0f, pattern.spreadAngle);
                    spawnRotation = Quaternion.AngleAxis(randomAngle + spawner.rotationOffset, Vector3.forward);
                    break;

                default:
                    spawnPoint = spawner.transform;
                    spawnRotation = spawnPoint.rotation;
                    break;
            }
        }
        else if (pattern.player_aimed && spawner.PlayerTransform != null)
        {
            Transform playerTransform = spawner.PlayerTransform;
            //spawner.PlayerTransform = PlayerTransform;

            
            switch (pattern.patternType)
            {
                case BulletPatternSO.BulletPatternType.Linear:
                    spawnPoint = spawner.transform;
                    spawnRotation = spawnPoint.rotation;
                    break;

                case BulletPatternSO.BulletPatternType.Burst:
                    spawnPoint = playerTransform;
                    spawnRotation = playerTransform.rotation;
                    break;

                case BulletPatternSO.BulletPatternType.ReverseBurst:
                    spawnPoint = playerTransform;
                    spawnRotation = playerTransform.rotation;
                    break;

                case BulletPatternSO.BulletPatternType.Spiral:
                    spawnPoint = playerTransform;
                    spawnRotation = playerTransform.rotation;
                    break;

                case BulletPatternSO.BulletPatternType.Random:
                    // Firing in a random direction within a defined degree range based on spread angle
                    float randomAngle = Random.Range(0f, pattern.spreadAngle);
                    spawnRotation = Quaternion.AngleAxis(randomAngle + spawner.rotationOffset, Vector3.forward);
                    break;

                case BulletPatternSO.BulletPatternType.Spread:
                    spawnPoint = spawner.transform;
                    spawnRotation = spawnPoint.rotation;
                    break;

                default:
                    Debug.LogError($"[AttackSO] Unhandled pattern type: {pattern.patternType}");
                    break;
            }
        }
        else if (pattern.spawnLocation != Vector2.zero)
        {
            pattern.TriggerPattern(spawnPoint, spawnRotation, spawner, false, spawner.BulletAllegiance);
            return;
        }
        else
        {
            spawnPoint = spawner.transform;
            spawnRotation = spawnPoint.rotation;
        }

        pattern.TriggerPattern(spawnPoint, spawnRotation, spawner, false, spawner.BulletAllegiance);
    }
}
