using UnityEngine;

public class BulletSpawner : MonoBehaviour
{

    [Tooltip("Assign your BulletPattern ScriptableObject here.")]
    public BulletPatternSO[] bulletPatterns;

    [field: SerializeField, Tooltip("Will bullets from this spawner help or harm the player?")]
    public BulletAllegiance BulletAllegiance { get; private set; }

    [Tooltip("Where the bullets should spawn.")]
    public Transform spawnPoint;

    [Tooltip("Rotation offset of the spawn direction.")]
    [HideInInspector] public float rotationOffset = 0f;

    [Tooltip("Player's transform.")]
    public Transform PlayerTransform { get; private set; }

    [Header("Attack Settings")]
    [Tooltip("Bullet patterns the enemy can use.")]
    public AttackPatternSO attackPattern;

    public delegate void OnFireEvent();
    public OnFireEvent OnRangedAttackFire;

    private void Start()
    {
        if (bulletPatterns.Length == 0)
        {
            Debug.LogWarning("[Bullet Spawner] Bullet pattern list is empty!");
        }

        spawnPoint = transform;
    }

    public void SetPlayerTarget(Transform incomingTarget)
    {
        if (PlayerTransform == incomingTarget) return;
        PlayerTransform = incomingTarget;
    }

    private void Update()
    {
        //if (attackPattern == null) return;
    }

    // Trigger an attack pattern sequence
    public virtual void TriggerAttack()
    {
        if (attackPattern != null)
        {
            AttackSO randomAttack = attackPattern.GetRandomAttack(); // Get random attack from the pattern
            randomAttack.FireAttack(this); // Fire the attack sequence using the spawner
            OnRangedAttackFire?.Invoke();
        }
    }

    // Triggers a bullet pattern at the default spawn point.
    public void TriggerPattern()
    {
        if (bulletPatterns.Length == 0) return;

        BulletPatternSO selectedPattern = bulletPatterns[Random.Range(0, bulletPatterns.Length)];
        selectedPattern.TriggerPattern(spawnPoint, spawnPoint.rotation, this, false, BulletAllegiance);
        
    }
}

public enum BulletAllegiance
{
    Hostile,
    Friendly
}
