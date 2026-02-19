using UnityEngine;
using System.Collections;
public class EmitterBulletHandler : IBulletHandler
{
    private bool isEmitting = true; // Flag to control whether the emission is active
    float timeBetween;
    BulletPatternSO pattern;

    public void DoBehaviour(Vector2 spawnPosition, Quaternion spawnRotation, BulletSO bulletSO, MonoBehaviour runner, BulletAllegiance incomingAllegiance)
    {
        if (bulletSO.bulletPattern == null)
        {
            Debug.LogError("BulletPatternSO is null in DoBehaviour!");
            return;
        }

        pattern = bulletSO.bulletPattern;
        timeBetween = bulletSO.delayBetween;
        isEmitting = true;
        runner.StartCoroutine(StartBulletEmission(runner));
    }

    public void EndBehaviour(MonoBehaviour runner) 
    {
        isEmitting = false;
        runner.StopCoroutine(StartBulletEmission(runner));
    }
    private IEnumerator StartBulletEmission(MonoBehaviour monob)
    {
        while (isEmitting) // Continuously emit until disabled
        {
            //Debug.Log($"[EmitterBulletHandler] Emitting pattern!");
            EmitPattern(monob); // Emit the pattern
            yield return new WaitForSeconds(timeBetween); // Wait for the next emission
        }
    }

    private void EmitPattern(MonoBehaviour monob)
    {
        if (pattern != null)
        {
            pattern.TriggerPattern(monob.gameObject.transform, monob.gameObject.transform.rotation, monob, true, pattern.allegiance);
        }
    }

    
}
