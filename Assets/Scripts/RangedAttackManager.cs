using UnityEngine;
using System.Collections;

public class RangedAttackManager : BulletSpawner
{
    [Header("Attack Control")]
    [field: SerializeField]
    public PlayerAttackController playerAttackController;

    public AttackSO currentAttack;

    //[field: SerializeField] public LayerMask targetLayer { get; private set; }

    [Header("Cooldowns")]
    [field: SerializeField, Tooltip("Toggles the use of spawner cooldown")]
    public bool UseSpawnerCooldown {  get; private set; }
    [field: SerializeField, Tooltip("How many of this tick-type must pass until the base cooldown of this bullet spawner is over")]
    public CooldownType CooldownType { get; private set; }
    [field: SerializeField, Tooltip("Toggles the use of spawner cooldown")]
    public float SpawnerCooldownValue { get; private set; }
    [field: SerializeField,]
    public float _currentSpawnerCooldownValue { get; private set; }

    [field: SerializeField]
    public bool DisablePatternCooldown { get; private set; }

    [field: SerializeField]
    public float cooldownValue { get; private set; }
    [field: SerializeField]
    public float currentCooldownValue { get; private set; }

    [field:
        SerializeField,
        Tooltip("Time between bullets in seconds." +
        "Set by AttackSO")]
    public float AttackCooldown { get; private set; }

    [Tooltip("How long the attack is on cooldown.")]
    [SerializeField] private float timeUntilNextAttack;


    [Header("Bools")]
    [field:SerializeField]
    public bool OnCooldown {  get; private set; }
    [field: SerializeField, Tooltip("Keep off, controlled by an animator")]
    public bool OnCooldown_ANIM { get; private set; }
    [field: SerializeField, Tooltip("Keep off, controlled by Attack SO")]
    public bool OnWindupCooldown { get; private set; }
    [field: SerializeField, Tooltip("Keep off, controlled by Spawner")]
    public bool OnSpawnerCooldown { get; private set; }

    bool firstAttack = true;
    
    
    private void Start()
    {
        // Optionally initialize attack cooldown or trigger first attack
        //timeUntilNextAttack = AttackCooldown;
        spawnPoint = transform;

    }

    public void SetAttackController(PlayerAttackController incomingATKController)
    {
        playerAttackController = incomingATKController;
    }

    private void Update()
    {
        #region DEPRECATED
        // Countdown to the next attack
        if (timeUntilNextAttack > 0f && OnCooldown)
        {
            timeUntilNextAttack -= Time.deltaTime;
        }
        else if (timeUntilNextAttack <= 0f && OnCooldown)
        {
            currentAttack = attackPattern.GetRandomAttack();

            // If cooldown finished, perform the attack
            TriggerAttack();
            timeUntilNextAttack = AttackCooldown;
            firstAttack = false;
        }
        else
        {
            timeUntilNextAttack = AttackCooldown;
        }
        #endregion
    }

    public void SetSpawnerCooldown(float incomingValue)
    {
        SpawnerCooldownValue = incomingValue;
    }

    public bool CanFire()
    {
        if (!OnCooldown && !OnCooldown_ANIM && !OnSpawnerCooldown && !OnWindupCooldown) return true;
        else return false;
    }

    public void Attack()
    {
        if (OnCooldown_ANIM || OnCooldown || OnWindupCooldown) return;

        currentAttack = attackPattern.GetRandomAttack();

        _currentSpawnerCooldownValue = SpawnerCooldownValue;

        if (DisablePatternCooldown)
        {
            cooldownValue = 0;

            foreach (BulletPatternSO pattern in currentAttack.patterns)
            {
                cooldownValue += pattern.windUpTime + pattern.patternLifetime;
            }
        }
        

        float windupCooldown = 0;

        foreach (BulletPatternSO pattern in currentAttack.patterns)
        {
            windupCooldown += pattern.windUpTime;
        }

        windupCooldown += currentAttack.attackWindUpTime;

        //SetCooldown(tempCooldown);

        if (windupCooldown > 0)
            StartCoroutine(WindupCountTime(windupCooldown));
        else
            TriggerAttack();

        //StartCoroutine(AttackFire());
    }

    void toggleAttackManagerCooldown(bool toggle)
    {
        //if (TickManager.Instance == null || toggle == OnCooldown) return;

        OnCooldown = toggle;

        //switch (CooldownType)
        //{
        //    case CooldownType.Ticks:
        //        if (toggle)
        //            TickManager.Instance.Tick += CountdownRangedAttackCooldown;
        //        else
        //            TickManager.Instance.Tick -= CountdownRangedAttackCooldown;
        //        break;

        //    case CooldownType.MacroTicks:
        //        if (toggle)
        //            TickManager.Instance.MacroTick += CountdownRangedAttackCooldown;
        //        else
        //            TickManager.Instance.MacroTick -= CountdownRangedAttackCooldown;
        //        break;

        //    case CooldownType.Seconds:
        //        if (toggle)
        //            TickManager.Instance.SecondTick += CountdownRangedAttackCooldown;
        //        else
        //            TickManager.Instance.SecondTick -= CountdownRangedAttackCooldown;
        //        break;

        //    default:
        //        if (toggle)
        //            TickManager.Instance.SecondTick += CountdownRangedAttackCooldown;
        //        else
        //            TickManager.Instance.SecondTick -= CountdownRangedAttackCooldown;
        //        break;
        //}
    }

    public void SetCooldown()
    {
        //AttackCooldown = cooldownTime; // Set the cooldown between attacks
        //timeUntilNextAttack = AttackCooldown; // Reset the timer


        currentCooldownValue = cooldownValue;
        toggleAttackManagerCooldown(true);
    }

    public void ResetAll()
    {
        currentCooldownValue = 0;
        OnCooldown = false;
        OnWindupCooldown = false;
    }

    public void CountdownRangedAttackCooldown()
    {
        currentCooldownValue -= 1;
        currentCooldownValue = Mathf.Clamp(currentCooldownValue, 0, 2413);

        _currentSpawnerCooldownValue -= 1;
        _currentSpawnerCooldownValue = Mathf.Clamp(_currentSpawnerCooldownValue, 0, 2413);

        if (currentCooldownValue <= 0 && _currentSpawnerCooldownValue <= 0)
        {
            toggleAttackManagerCooldown(false);
        }
    }

    // Call this method to perform an attack
    public override void TriggerAttack()
    {
        if (currentAttack != null)
        {
            if (!OnCooldown_ANIM)
            {
                currentAttack.FireAttack(this);
            }
            
            SetCooldown();
        }
    }

    IEnumerator WindupCountTime(float time)
    {
        OnWindupCooldown = true;
        yield return new WaitForSeconds(time);
        OnWindupCooldown = false;
        TriggerAttack();
    }


    #region DEPRECATED
  
    IEnumerator AttackFire()
    {
        currentAttack = attackPattern.GetRandomAttack();

        //Countdown to the next attack

        float tempCooldown = 0;

        

        //SetCooldown(tempCooldown);

        if (currentAttack.attackWindUpTime > 0)
            StartCoroutine(WindupCountTime(currentAttack.attackWindUpTime));

        TriggerAttack();

        //yield return new WaitForSeconds(AttackCooldown);
        yield return null;

    }

    public virtual bool Contains(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }


    
    #endregion

    
}

public enum CooldownType
{
    Ticks,
    MacroTicks,
    Seconds
}