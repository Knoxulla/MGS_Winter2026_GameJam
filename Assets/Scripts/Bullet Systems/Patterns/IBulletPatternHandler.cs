using UnityEngine;

public interface IBulletPatternHandler
{
    void FirePattern(Vector2 spawnPosition, Quaternion spawnRotation, BulletPatternSO pattern, MonoBehaviour coroutineRunner, bool fromBullet, BulletAllegiance incomingAllegiance);

}

