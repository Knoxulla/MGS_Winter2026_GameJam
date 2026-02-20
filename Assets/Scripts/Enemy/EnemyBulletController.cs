using UnityEditor.Rendering;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{

    [Header("Projectile Settings")]
    [SerializeField] private float dmg;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;

    [SerializeField] Rigidbody rb;

    protected Vector2 targetDirection;


    public void SetValues(float damage, float speedAmt, float bulletLifetime)
    {
        dmg = damage;
        speed = speedAmt;
        lifetime = bulletLifetime;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        transform.parent = null;

        if (targetDirection == Vector2.zero || GetComponent<EnemyMASTER>().attackController.isAttacking)
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
