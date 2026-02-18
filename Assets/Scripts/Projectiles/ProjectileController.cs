using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] Rigidbody2D rb;

    [SerializeField] float damage = 1f;
    [SerializeField] float speed = 1f;
    [SerializeField] float lifetime = 5f;
    [SerializeField] float pierceCount = 1f;

    // Add SO for info here

    float timer = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.parent = null; // maybe add bullet limits through pooling
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(transform.up * speed);

        if (pierceCount <= 0 || lifetime <= timer)
        {
            // kill self
            Destroy(gameObject);
        }

        timer += Time.deltaTime;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            
            var enemyHealthController = collision.gameObject.GetComponent<EnemyHealthController>();
            enemyHealthController.takeDamage(damage);
            Destroy(gameObject);
        }
    }

    public void AssignTarget(Transform transform)
    { 
       target = transform;
    }

    public void AssignDamage(float damageAmount)
    {
        damage = damageAmount;
    }

    public float GetDamage()
    { 
        return damage;
    }
    public void AssignSpeed(float speedAmount)
    {
        speed = speedAmount;
    }
}
