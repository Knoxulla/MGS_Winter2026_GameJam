using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class Bullet_BASE : MonoBehaviour
{
    //private const string IGNORE_BULLETS_TAG = "IgnoreProjectiles";

    [Tooltip("The Current BulletSO")]
    public BulletSO bulletSO;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField] public float damage = 1;

    [SerializeField]
    public Light2D BulletLight { get; private set; }

    public BulletAllegiance CurrentBulletAllegiance;

    [Tooltip("Hostile bullets damage target this layer")]
    public LayerMask HostileTargetLayers;

    [Tooltip("Friendly bullets damage target this layer")]
    public LayerMask FriendlyTargetLayers;

    [field: SerializeField, Tooltip("These are the layers they will die to")]
    protected LayerMask deathLayers { get; private set; }

    [SerializeField, Tooltip("How long the bullet will live")]
    protected float lifetime;


    // Unnecessary to be in Inspector so they are hidden here
    [HideInInspector] public Vector3 velocity;
    protected BulletPatternSO patternSO;
    [SerializeField] private float speed = 10f; // Speed factor for movement
    [HideInInspector] public Transform playerTransform;


    private Vector3[] pathPoints;  // Store the current path
    private float progress = 0f;  // Progress along the Bezier curve
    private bool hasSetDirection = false;  // Flag to track if direction has been set
    private bool isActive; // allows for bullets to only check for when they are enabled.

    bool isPlayerAllied = true;

    private void OnEnable()
    {
        progress = 0f; // Reset progress when bullet is reused
        hasSetDirection = false;  // Reset the flag when the bullet is enabled
        
    
        if (isPlayerAllied)
        {
            damage = PlayerMASTER.Instance.playerAttackController.modifiedDmg + bulletSO.damageAmount;
            transform.position = PlayerMASTER.Instance.transform.position;

            if (damage < PlayerMASTER.Instance.playerAttackController.minDmg)
            {
                damage = PlayerMASTER.Instance.playerAttackController.minDmg;

            }
        }
        else
        {
            damage = GetComponent<EnemyAttackController>().finalDmg;
        }
        
        spriteRenderer.sprite = bulletSO.bulletSprite;
    }

    private void Start()
    {

        if (isPlayerAllied)
        {
           damage = PlayerMASTER.Instance.playerAttackController.modifiedDmg + bulletSO.damageAmount;
            transform.position = PlayerMASTER.Instance.transform.position;

            if (damage < PlayerMASTER.Instance.playerAttackController.minDmg)
            {
                damage = PlayerMASTER.Instance.playerAttackController.minDmg;
                    
            }
        }
        else
        {
            damage = GetComponent<EnemyAttackController>().finalDmg;
        }

        if (patternSO == null)
        {
            Debug.Log("BulletPatternSO is not assigned, bullet will not be fired.");
            return;
        }

        if (PlayerMASTER.Instance != null)
        {
            if (PlayerMASTER.Instance.playerAttackController == null ||
                PlayerMASTER.Instance.playerMovementController == null ||
                PlayerMASTER.Instance.playerHealthController == null)
            {
                Debug.LogError("Player_Controller properties are not properly initialized.");
                return;
            }

            playerTransform = PlayerMASTER.Instance.transform;
            UpdateVelocityForPlayerAimed();
        }
        else
        {
            Debug.LogError("Player_Controller instance is not initialized, bullet can't track the player.");
        }

        SetBulletPropertiesFromPattern(patternSO, CurrentBulletAllegiance);

        if (patternSO.useBezierPath)
        {
            pathPoints = patternSO.GetSelectedPath();
            progress = 0f; // Reset for Bezier path
        }
        spriteRenderer.sprite = bulletSO.bulletSprite;
    }

    public void SetBulletAllegiance(BulletAllegiance incomingAllegiance)
    {
        CurrentBulletAllegiance = incomingAllegiance;
        //Debug.Log("Receiving Bullet allegiance as: " + incomingAllegiance);

        switch (CurrentBulletAllegiance)
        {
            case BulletAllegiance.Hostile:
                gameObject.layer = 16; // Enemy/Hurtbox
                break;

            case BulletAllegiance.Friendly:
                gameObject.layer = 12; // Player/Hurtbox
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayerAllied)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("HIT ENEMY WHERE DMG");
                DealDamageToEnemy(collision.gameObject.GetComponent<EnemyMASTER>().healthController);

                BulletPoolManager.Instance.ReturnBulletToPool(this);
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                
                    DealDamageToPlayer();

                

                BulletPoolManager.Instance.ReturnBulletToPool(this);
            }
                    
        }
        
    }

    public virtual bool Contains(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    public void DealDamageToPlayer()
    {
        if (bulletSO == null)
        {
            Debug.LogError("bulletSO is null in DealDamageToPlayer()");
            return;
        }

        PlayerMASTER.Instance.playerHealthController.UpdateHealth(PlayerMASTER.Instance.playerHealthController.currentHealth - damage);
    }

    public void DealDamageToEnemy(EnemyHealthController enemyHitbox)
    {
        if (bulletSO == null)
        {
            Debug.LogError("bulletSO is null in DealDamageToPlayer()"); 
            return;
        }

        
        enemyHitbox.takeDamage(damage);
    }

    private void SetLighting()
    {
        if (bulletSO == null)
        {
            Debug.LogError("bulletSO is null in SetLighting()");
            return;
        }

      //  BulletLight.color = bulletSO.LightingColor;
       // BulletLight.intensity = bulletSO.LightIntensity;
    }

    private void SetSize()
    {
        if (!bulletSO.modifySize)
        {
            return;
        }

        if (bulletSO.bulletSprite == null)
        {
            Debug.LogWarning($"[Bullet_BASE] bullet sprite is null in SetSize");
            spriteRenderer.gameObject.transform.localScale = new Vector3(1f, 1f, 0);
            return;
        }

        spriteRenderer.sprite = bulletSO.bulletSprite;

        if ((bulletSO.spriteSizeModifier > 0) || (bulletSO.bulletSizeModifier > 0))
        {
            if ((bulletSO.spriteSizeModifier > 0))
            {
                spriteRenderer.gameObject.transform.localScale = new Vector3(bulletSO.spriteSizeModifier, bulletSO.spriteSizeModifier, 0);
            }
            if ((bulletSO.bulletSizeModifier > 0))
            {
                gameObject.transform.localScale = new Vector3(bulletSO.bulletSizeModifier, bulletSO.bulletSizeModifier, 0);
            }

            return;
        }
        else
        {
            spriteRenderer.gameObject.transform.localScale = new Vector3(1f, 1f, 0);
            gameObject.transform.localScale = new Vector3(1f, 1f, 0);
        }
        

    }

    public virtual void SetBulletPropertiesFromPattern(BulletPatternSO pattern, BulletAllegiance allegiance)
    {
        if (pattern == null)
        {
            Debug.LogError("Pattern is null, can't set bullet properties.");
            return;
        }

        CurrentBulletAllegiance = allegiance;
        patternSO = pattern;
        speed = patternSO.bulletSpeed;
        lifetime = patternSO.patternLifetime;

        SetSize();
        SetLighting();

        // Play Fire Sound
        //if (!bulletSO.FireSound.IsNull && AudioManager.Instance != null)
        //{
        //   // AudioManager.Instance.PlayOneShot(bulletSO.FireSound, transform.position);
        //}

        // Summon Fire Particles
        //if (bulletSO.SummonParticle != null && Particle_Master_Pool.instance != null)
        //{
        //    //Particle_Master_Pool.instance.SummonParticle(transform, bulletSO.SummonParticle);
        //}

        if (patternSO.player_aimed &&
            patternSO.patternType != BulletPatternSO.BulletPatternType.Spiral &&
            patternSO.patternType != BulletPatternSO.BulletPatternType.Burst &&
            patternSO.patternType != BulletPatternSO.BulletPatternType.Spread)
        {
            UpdateVelocityForPlayerAimed();
        }
        else if (!patternSO.useBezierPath &&
            patternSO.patternType != BulletPatternSO.BulletPatternType.Spiral &&
            patternSO.patternType != BulletPatternSO.BulletPatternType.Burst &&
            patternSO.patternType != BulletPatternSO.BulletPatternType.Spread &&
            patternSO.patternType != BulletPatternSO.BulletPatternType.ReverseBurst)
        {
            velocity =  PlayerMASTER.Instance.playerAttackController.aimObject.transform.up * speed;
        }

        SetBulletBehaviours(bulletSO);

        isActive = true;
        StartCoroutine(RunLifeDown());
    }

    public virtual void SetBulletPropertiesFromBullet(BulletSO bulletInfo, BulletPatternSO backUpPattern, BulletAllegiance incomingAllegiance)
    {
        if (bulletInfo.bulletPattern == null)
        {
            Debug.Log($"{bulletInfo.bulletType} has no pattern. Setting back up pattern...");
            SetBulletPropertiesFromPattern(backUpPattern, incomingAllegiance);
            return;
        }

        SetBulletPropertiesFromPattern(bulletInfo.bulletPattern, incomingAllegiance);
    }


    private void SetBulletBehaviours(BulletSO bulletSO)
    {
        if (bulletSO == null)
        {
            Debug.LogError("bulletSO is NULL before calling BulletHandlerFactory!");
            return;
        }


        // Get the appropriate handler based on the bullet type
        IBulletHandler handler = BulletSO.BulletHandlerFactory.GetHandler(bulletSO.bulletType);

        if (handler == null)
        {
            Debug.LogError("No handler found for this bullet type!");
            return;
        }
        if (bulletSO.bulletType == BulletSO.BulletType.EmitOnDeath)
        {
            // Handled in the ResetBullet Function
            return;
        }

        handler.DoBehaviour(transform.position, transform.rotation, bulletSO, this, CurrentBulletAllegiance);
    }

    private void UpdateVelocityForPlayerAimed()
    {
        if (patternSO.patternType == BulletPatternSO.BulletPatternType.Spiral ||
            patternSO.patternType == BulletPatternSO.BulletPatternType.Burst ||
            patternSO.patternType == BulletPatternSO.BulletPatternType.Spread)
        {
            return;
        }

       

        playerTransform = PlayerMASTER.Instance.transform;

        if (patternSO == null || bulletSO == null)
        {
            Debug.LogError("patternSO or bulletSO is null in UpdateVelocityForPlayerAimed.");
            return;
        }

        if (playerTransform != null && patternSO.player_aimed)
        {
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

            if (directionToPlayer != Vector3.zero)
            {
                velocity = directionToPlayer * patternSO.bulletSpeed;
            }
        }
    }

    void Update()
    {
        ActivateBehaviours();

    }

    private void ActivateBehaviours()
    {
        if (isActive)
        {
            if (patternSO.useBezierPath && pathPoints != null && pathPoints.Length > 0 && !hasSetDirection)
            {
                // Move along the Bezier curve
                progress += Time.deltaTime * (speed / Vector3.Distance(pathPoints[0], pathPoints[pathPoints.Length - 1]));

                if (progress > 1f)
                {
                    progress = 1f; // Clamp to prevent overshooting
                }

                if (patternSO.pathRelativeToPlayer)
                {
                    if (PlayerMASTER.Instance.transform != null)
                    {
                        playerTransform = PlayerMASTER.Instance.transform;
                    }

                    // Get the point on the Bezier curve
                    Vector3 newPosition = BezierUtility.GetPointOnCurve(pathPoints, progress);

                    // Get the sign of the player's position (positive or negative)
                    float signX = Mathf.Sign(playerTransform.position.x);
                    float signY = Mathf.Sign(playerTransform.position.y);

                    // Adjust the Bezier point to match the sign of the player's position
                    newPosition.x *= signX;
                    newPosition.y *= signY;

                    // Set the final position based on the adjusted point
                    transform.position = newPosition;
                }
                else
                {
                    transform.position = BezierUtility.GetPointOnCurve(pathPoints, progress);
                }

                if (progress >= 1f && patternSO.keepMoving)
                {
                    // Set direction once after the Bezier path is completed
                    SetFinalDirection();
                    hasSetDirection = true;  // Ensure it doesn't re-enter this block again
                }
            }
            else if (hasSetDirection)
            {
                // Move in a straight line after the Bezier path is completed and the direction has been set
                if (velocity != Vector3.zero)  // Ensure velocity is non-zero
                {
                    transform.position += velocity * Time.deltaTime;
                }
                else
                {
                    Debug.LogError("Velocity is zero! Check SetFinalDirection logic.");
                }
            }
            else
            {
                transform.position += velocity * Time.deltaTime;
            }
        }
    }

    private void SetFinalDirection()
    {
        if (patternSO.player_aimed)
        {
            if (PlayerMASTER.Instance == null)
            {
                Debug.LogError("Player_Controller properties are not properly initialized in SetFinalDirection.");
                return;
            }

            playerTransform = PlayerMASTER.Instance.transform;

            // Set the direction towards the player (using Vector2)
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            velocity = directionToPlayer * patternSO.bulletSpeed;
        }
        else if (patternSO.calculatePath)
        {
            // Use the second-to-last Bezier point for direction calculation
            Vector2 secondLastBezierPoint = new Vector2(pathPoints[pathPoints.Length - 2].x, pathPoints[pathPoints.Length - 2].y);

            // Set the direction from the second-to-last point to the final Bezier point (using Vector2)
            Vector2 finalBezierPoint = new Vector2(pathPoints[pathPoints.Length - 1].x, pathPoints[pathPoints.Length - 1].y);
            velocity = (finalBezierPoint - secondLastBezierPoint).normalized * patternSO.bulletSpeed;
        }
        else
        {
            velocity = patternSO.defaultDirection.normalized * speed;
        }
    }


    private IEnumerator RunLifeDown()
    {
        yield return new WaitForSeconds(lifetime);
        ResetBullet();
        BulletPoolManager.Instance.ReturnBulletToPool(this);
    }

    public void ResetBullet()
    {
        // Play Break Sound
        //if (!bulletSO.BreakSound.IsNull && AudioManager.Instance != null)
        //{
        //   // AudioManager.Instance.PlayOneShot(bulletSO.BreakSound, transform.position);
        //}

        // Summon Break Particles
        //if (bulletSO.BreakParticle != null && Particle_Master_Pool.instance != null)
        //{
        //    Particle_Master_Pool.instance.SummonParticle(transform, bulletSO.BreakParticle);
        //}

        // Set all values to defaults
        velocity = Vector3.zero;
        progress = 0f;
        hasSetDirection = false;  // Reset the flag when resetting the bullet
        transform.position = GetComponentInParent<Transform>().position;
        isActive = false; // Set to false to avoid unnecessary performance usage

        

        // Get Handler
        IBulletHandler handler = BulletSO.BulletHandlerFactory.GetHandler(bulletSO.bulletType);

        // if it should emit on death, call doBehaviour
        if (bulletSO.bulletType == BulletSO.BulletType.EmitOnDeath)
        {
            handler.DoBehaviour(transform.position, transform.rotation, bulletSO, this, CurrentBulletAllegiance);
        }

        // Then end the behaviour
        handler.EndBehaviour(this);

    }
}

public static class BezierUtility
{
    public static Vector3 GetPointOnCurve(Vector3[] controlPoints, float t)
    {
        if (controlPoints.Length == 4) // Cubic Bezier
        {
            Vector3 p0 = controlPoints[0];
            Vector3 p1 = controlPoints[1];
            Vector3 p2 = controlPoints[2];
            Vector3 p3 = controlPoints[3];

            Vector3 q0 = Vector3.Lerp(p0, p1, t);
            Vector3 q1 = Vector3.Lerp(p1, p2, t);
            Vector3 q2 = Vector3.Lerp(p2, p3, t);

            Vector3 r0 = Vector3.Lerp(q0, q1, t);
            Vector3 r1 = Vector3.Lerp(q1, q2, t);

            return Vector3.Lerp(r0, r1, t);
        }
        else if (controlPoints.Length == 3) // Quadratic Bezier
        {
            Vector3 p0 = controlPoints[0];
            Vector3 p1 = controlPoints[1];
            Vector3 p2 = controlPoints[2];

            Vector3 q0 = Vector3.Lerp(p0, p1, t);
            Vector3 q1 = Vector3.Lerp(p1, p2, t);

            return Vector3.Lerp(q0, q1, t);
        }
        else
        {
            Debug.LogError("Bezier path must have 3 (quadratic) or 4 (cubic) control points.");
            return Vector3.zero;
        }
    }
}
