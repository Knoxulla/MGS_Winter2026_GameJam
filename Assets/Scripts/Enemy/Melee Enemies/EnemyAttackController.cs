using System.Collections;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    [SerializeField]
    public float dmg = 1;
    public float dmgMulti = 1;
    public float finalDmg = 1;

    public bool isAttacking = false;
    public float lengthOfAttack = 1f;

    public float timer = 0;
    
    public float attackCd;

    private CircleCollider2D col2D;

    void Start()
    {
        col2D = GetComponent<CircleCollider2D>();
        dmgMulti = RoundSystemController.Instance.dmgMulti;
    }

    // Update is called once per frame
   protected void Update()
    {

        CheckAttackState();
       
    }

    public virtual void CheckAttackState()
    {
        if (timer >= attackCd)
        {
            col2D.enabled = true;
        }
        else
        {
            col2D.enabled = false;
        }

        if (!isAttacking)
        {
            timer += Time.deltaTime;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PerformAttack(other);
        }
    }

    public virtual void PerformAttack(Collider2D other)
    {
        isAttacking = true;
        timer = 0;
        finalDmg = dmg * dmgMulti;
        PlayerHealthController playerHealthController = other.gameObject.GetComponent<PlayerHealthController>();
        playerHealthController.UpdateHealth(playerHealthController.currentHealth - finalDmg);
    }

    // time before isAttacking becomes false so the attack animation can finish
    private IEnumerator AttackCDAnimation()
    {
        yield return new WaitForSeconds(lengthOfAttack);
        isAttacking = false;
    }
}