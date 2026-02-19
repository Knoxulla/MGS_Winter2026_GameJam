using UnityEngine;

public interface IBulletHandler
{
    void DoBehaviour(Vector2 spawnPosition, Quaternion spawnRotation, BulletSO bulletSO, MonoBehaviour runner, BulletAllegiance incomingAllegiance);

    void EndBehaviour(MonoBehaviour runner);
}

