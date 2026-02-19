using UnityEngine;

// Deadass this does nothing and is just a way to tell the handler to do nothing
public class StandardBulletHandler : IBulletHandler
{
    public void DoBehaviour(Vector2 spawnPosition, Quaternion spawnRotation, BulletSO bulletSO, MonoBehaviour runner, BulletAllegiance incomingAllegiance)
    {
        //throw new System.NotImplementedException();
    }

    public void EndBehaviour(MonoBehaviour runner)
    {
        //throw new System.NotImplementedException();
    }
}
