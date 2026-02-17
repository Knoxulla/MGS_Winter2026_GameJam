using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    [SerializeField]
    private float dmg = 1;
    public float dmgMulti = 1;

    public float timer = 0;
    
    public float attackCd;

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")  && timer >= attackCd)
        {
            PerformAttack();
        }
    }

    void PerformAttack()
    {
        timer = 0;
    }
}