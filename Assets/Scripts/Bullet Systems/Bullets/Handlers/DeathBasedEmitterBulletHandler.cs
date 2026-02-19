using UnityEngine;

public class DeathBasedEmitterBulletHandler : IBulletHandler
{
    public void DoBehaviour(Vector2 spawnPosition, Quaternion spawnRotation, BulletSO bulletSO, MonoBehaviour runner, BulletAllegiance incomingAllegiance)
    {
        BulletPatternSO pattern = bulletSO.bulletPattern;
        pattern.allegiance = incomingAllegiance;

        pattern.TriggerPattern(runner.gameObject.transform, runner.gameObject.transform.rotation, runner, true, pattern.allegiance);
    }

    public void EndBehaviour(MonoBehaviour runner)
    {
        // No end behaviour bc it happens once
    }
}
