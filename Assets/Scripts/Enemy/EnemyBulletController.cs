using UnityEditor.Rendering;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{

    [Header("Projectile Settings")]
    [SerializeField] private float dmg;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;

    [SerializeField] Rigidbody2D rb;

    protected Vector2 targetDirection;
    public PlayerDetectionController playerDetectionController;
    public void SetValues(float damage, float speedAmt, float bulletLifetime)
    {
        dmg = damage;
        speed = speedAmt;
        lifetime = bulletLifetime;
    }

    private void Start()
    {
        transform.position = Vector3.zero;

        rb = GetComponent<Rigidbody2D>();

        transform.parent = null;


        if (playerDetectionController.detectedPlayer)
        {
            targetDirection = playerDetectionController.DirectionToPlayer;
        }
        else
        {
            targetDirection = Vector2.zero;
        }
    
       

        if (targetDirection == Vector2.zero)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = targetDirection.normalized * speed;
        }
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;

        if (lifetime <= 0)
        {
            KillSelf();
        }

        rb.AddForce(targetDirection.normalized * speed);
    }

    private void KillSelf()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealthController playerHealthController = collision.gameObject.GetComponent<PlayerHealthController>();
            playerHealthController.UpdateHealth(playerHealthController.currentHealth - dmg);
            KillSelf();
        }
    }

}
