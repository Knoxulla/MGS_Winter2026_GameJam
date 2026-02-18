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
        else
        {
            col2D.enabled = false;
        } 
        
        timer += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PerformAttack(other);
        }
    }

    void PerformAttack(Collider2D other)
    {
        timer = 0;
        PlayerHealthController playerHealthController = other.gameObject.GetComponent<PlayerHealthController>();
        playerHealthController.UpdateHealth(playerHealthController.currentHealth - dmg*dmgMulti);
    }
}