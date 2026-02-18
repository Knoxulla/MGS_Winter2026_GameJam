using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    [SerializeField]
    private float dmg = 1;
    public float dmgMulti = 1;

    public float timer = 0;
    
    public float attackCd;

    private CircleCollider2D col2D;

    void Start()
    {
        col2D = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= attackCd)
        {
            col2D.enabled = true;
        }

        timer += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")  && timer >= attackCd)
        {
            PerformAttack(other);
        }
    }

    void PerformAttack(Collider2D other)
    {
        timer = 0;
        var playerHealthController = other.gameObject.GetComponent<PlayerHealthController>();
        playerHealthController.UpdateHealth(dmg*dmgMulti);
        col2D.enabled = false;
    }
}