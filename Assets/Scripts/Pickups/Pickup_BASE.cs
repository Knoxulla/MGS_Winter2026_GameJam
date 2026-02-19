using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.Pool;

public class Pickup_BASE : MonoBehaviour
{
    [SerializeField]
    public PickupSO CurrentSO {  get; private set; }

    [field: Space(10)]

    [field: SerializeField]

    [field: Space(20)]

    [Header("Events"), SerializeField, Tooltip("When the target enters its trigger")]
    public UnityEvent OnTriggerEntered { get; private set; }

    [field: Header("Events"), SerializeField, Tooltip("When the target leaves its trigger"), Space(5)]
    public UnityEvent OnTriggerExit { get; private set; }

    [field: Header("Events"), SerializeField, Tooltip("When the player picks up the item." +
        "Normally called when entering its trigger. But if not, it's when interacting"), Space(5)]
    public UnityEvent OnPickup { get; private set; }

    [field: Header("PickupStats"), SerializeField, Tooltip("How much this pickup will effect stats." +
        "How much health is healed, fate is gained, etc.")]
    public int Value { get; private set; }

    [Header("PickupStats")]
    public float timeBeforeFadeout;
    [Header("PickupStats"), SerializeField] float timeToFadeOut;
    bool startFadeOut = false;
    float originalTimeBeforeFadeout;

    [field: Header("Visuals"), SerializeField]
    public SpriteRenderer PickupSpriteRenderer {  get; private set; }
    [Header("Visuals"), SerializeField]
    Light2D spriteLight;
    [field: Header("Visuals"), SerializeField]
    public bool ShouldRotate { get; private set; }


    [field: Header("Debug"), SerializeField]
    public PickupSO BlankSO { get; private set; }


    private ObjectPool<Pickup_BASE> _pickupPool;
    private EnemyHealthController _enemyHealth;

    public float FloatSpeed {  get; private set; }
    public float FloatAmp { get; private set; }

    private void Awake()
    {

        if (PickupSpriteRenderer == null)
        {
            if (GetComponentInChildren<SpriteRenderer>())
                PickupSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        
        if (spriteLight)
        {
            if (GetComponentInChildren<Light2D>())
                spriteLight = GetComponentInChildren<Light2D>();
        }

        originalTimeBeforeFadeout = timeBeforeFadeout;
    }

    private void OnEnable()
    {
        startFadeOut = false;
        ShouldRotate = true;

        if (timeBeforeFadeout > 0 && timeToFadeOut > 0)
            StartCoroutine(pickup_FadeOutTimer());
    }

    private void OnDisable()
    {
        timeBeforeFadeout = originalTimeBeforeFadeout;

        PickupSpriteRenderer.color = new Color(1f, 1f, 1f, 0f);

        if (spriteLight)
            spriteLight.intensity = 0f;

        ShouldRotate = false;

        CurrentSO = null;

        if (_enemyHealth != null)
        {
            _enemyHealth = null;
        }
            
        
        
    }

    IEnumerator pickup_FadeOutTimer()
    {
        while (timeBeforeFadeout > 0)
        {
            timeBeforeFadeout--;
            yield return new WaitForSeconds(1);
        }

        startFadeOut = true;
    }

    private void Update()
    {
        if (timeBeforeFadeout <= 0 && timeToFadeOut <= 0) return;

        float stepOut = timeToFadeOut * Time.deltaTime;

        if (startFadeOut && timeToFadeOut > 0)
        {
            PickupSpriteRenderer.color = new Color(1f, 1f, 1f, Mathf.Lerp(PickupSpriteRenderer.color.a, 0f, stepOut));
            if (spriteLight)
                spriteLight.intensity = Mathf.Lerp(spriteLight.intensity, 0f, stepOut);
            if (PickupSpriteRenderer.color.a <= 0.1f)
                DestroyPickup();
        }
    }

    public void OnPickupRotate()
    {

    }

    //private void OnBecameVisible()
    //{
    //    ShouldRotate = true;
    //}

    //private void OnBecameInvisible()
    //{
    //    ShouldRotate = false;
    //}



    void interacted()
    {
        PickedUp();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (CurrentSO == null) return;
        else
        {
            switch (CurrentSO.PickupTargetEffector)
            {
                case PickupTarget.Players:
                    if (collision.CompareTag("Player"))
                    {
                        OnTriggerEntered?.Invoke();
                    }
                    break;

                case PickupTarget.Enemies:
                    if (collision.CompareTag("Enemy"))
                    {
                        if (collision.GetComponent<EnemyHealthController>())
                            _enemyHealth = collision.GetComponent<EnemyHealthController>();
                        else if (collision.GetComponentInParent<EnemyHealthController>())
                            _enemyHealth = collision.GetComponentInParent<EnemyHealthController>();

                        OnTriggerEntered?.Invoke();
                    }
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CurrentSO == null) return;
        else
        {
            switch (CurrentSO.PickupTargetEffector)
            {
                case PickupTarget.Players:
                    if (collision.CompareTag("Player"))
                    {
                        OnTriggerExit?.Invoke();
                    }
                    break;

                case PickupTarget.Enemies:
                    if (collision.CompareTag("Enemy"))
                    {
                        OnTriggerExit?.Invoke();
                    }
                    break;
            }
        }
    }

    public void SetValue(PickupSO incomingSO, int incomingValue, int incomingMaxValue = 0)
    {
        if (incomingMaxValue > 0)
        {
            if (incomingValue < incomingMaxValue)
            {
                Value = Random.Range(incomingValue, incomingMaxValue);
            }
            else
            {
                // min is higher than max for some reason
                Value = incomingMaxValue;
            }
        }
        else
        {
            Value = incomingMaxValue;
        }
        

        CurrentSO = incomingSO;


        SetSOVariables(CurrentSO);
    }

    void SetSOVariables(PickupSO incomingSO)
    {
        timeBeforeFadeout = incomingSO.TimeBeforeFadeout;
        timeToFadeOut = incomingSO.TimeToFadeOut;

        if (incomingSO.PickupSprites.Count  > 0)
        {
            int SetPickupSprite = Random.Range(0, incomingSO.PickupSprites.Count - 1);
            PickupSpriteRenderer.sprite = incomingSO != null ? incomingSO.PickupSprites[SetPickupSprite] : null;
        }

        gameObject.name = incomingSO.name;

        float size = Random.Range(incomingSO.PickupSize.x, incomingSO.PickupSize.y);
        if (size == 0)
            size = 1;
        transform.localScale = new Vector3(size, size, size);

        FloatSpeed = Random.Range(incomingSO.FloatSpeed.x, incomingSO.FloatSpeed.y);
        FloatAmp = Random.Range(incomingSO.FloatAmp.x, incomingSO.FloatAmp.y);

        PickupSpriteRenderer.color = incomingSO.SpriteColor;

        spriteLight.color = incomingSO.LightColor;
        spriteLight.intensity = incomingSO.LightIntensity;

       
        
    }


    /*
     * Effects
     */

    public virtual void PickedUp()
    {
        //ReImplement once PlayerHealth is integrated

        //if (checkTargetHealth)
        //{
        //    if (PlayerEffector && playerHealth != null)
        //    {
        //        if (isTargetHurt(playerHealth.currentHealth, playerHealth.maxHealth))
        //            OnPickup?.Invoke();
        //        else
        //            return;
        //    }
        //    else
        //        OnPickup?.Invoke();

        //    if (effectEnemies && enemyHealth != null)
        //    {
        //        if (isTargetHurt(enemyHealth.currentHealth, enemyHealth.maxHealth))
        //            OnPickup?.Invoke();
        //        else
        //            return;
        //    }
        //    else
        //        OnPickup?.Invoke();
        //}
        if (CurrentSO != null)
        {
            if (CurrentSO.AffectHealth)
                AffectHealth();
        }
        

        OnPickup?.Invoke();

    }

    public virtual bool isTargetHurt(float currentHP, float maxHP)
    {
        if (maxHP <= currentHP)
            return false;
        else
            return true;
    }

    public virtual void AffectHealth()
    {
        switch (CurrentSO.PickupTargetEffector)
        {
            case PickupTarget.Players:
                if (PlayerMASTER.Instance.playerHealthController != null)
                {
                    if (CurrentSO.CheckTargetHealth &&
                        !isTargetHurt
                        (
                        PlayerMASTER.Instance.playerHealthController.currentHealth,
                        PlayerMASTER.Instance.playerHealthController.maxHealth
                        )) break;

                    if (Value > 0)
                        PlayerMASTER.Instance.playerHealthController.currentHealth += Value;
                    else if (Value < 0)
                        PlayerMASTER.Instance.playerHealthController.currentHealth -= Value;
                }
                break;

            case PickupTarget.Enemies:
                if (_enemyHealth != null)
                {
                    if (CurrentSO.CheckTargetHealth &&
                        !isTargetHurt
                        (
                        _enemyHealth.currentHealth,
                        _enemyHealth.maxHealth
                        )) break;

                    if (Value > 0)
                        _enemyHealth.currentHealth += Value;
                    else if (Value < 0)
                        _enemyHealth.takeDamage(Value);
                }
                break;
        }
    }

    public virtual void PickupBattery()
    {
        //if (PlayerEffector)
        //{
        //    batteryUser = true;

        //    if (playerHealth.batteryHolder != true)
        //    {
        //        batteryUser = false;
        //        playerHealth.batteryPickedUp();
        //    }
        //    else
        //    {
        //        batteryUser = true;
        //        bark_hasBattery();
        //        return;
        //    }
        //}
    }

    void bark_hasBattery()
    {
        //int b = Random.Range(1, 7);
        //string dialogue;

        //switch (b)
        //{
        //    case 1:
        //        dialogue = "Hands full...";
        //        BarkManager.Instance.ArchieBark(dialogue);
        //        break;
        //    case 2:
        //        dialogue = "Already carrying";
        //        BarkManager.Instance.ArchieBark(dialogue);
        //        break;
        //    case 3:
        //        dialogue = "Must drop...";
        //        BarkManager.Instance.ArchieBark(dialogue);
        //        break;
        //    default:
        //        break;

        //}
    }


    public virtual void DestroyPickup()
    {
        //if (CurrentSO.PickupParticles)
        //{
        //    Particle_Master_Pool.instance.SummonParticle(transform, CurrentSO.PickupParticles);
        //}

        if (_pickupPool != null)
            _pickupPool.Release(this);
        else
            Destroy(this);
    }

    public void SetPool(ObjectPool<Pickup_BASE> pool)
    {
        _pickupPool = pool;
    }

    public void SetCurrentSO()
    {
        if (CurrentSO == null) return;

        SetSOVariables(CurrentSO);
    }

    public void ResetSO()
    {
        if (BlankSO == null) return;

        SetSOVariables(BlankSO);
    }
}
