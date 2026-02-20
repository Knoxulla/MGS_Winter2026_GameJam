using UnityEngine;

public class RangedEnemyAttackController : EnemyAttackController
{
    [Header("Projectiles")]
    [SerializeField] float projectileSpd = 1f;
    [SerializeField] float projectileLifetime = 3f;
    [SerializeField] GameObject bullet;

    [SerializeField] Collider2D col;

    private void Start()
    {
        col = GetComponent<Collider2D>();
    }

    public override void CheckAttackState()
    {
        if (timer >= attackCd)
        {
            PerformAttack(col);
        }

            timer += Time.deltaTime;
        
    }

    public override void PerformAttack(Collider2D other)
    {
        //base.PerformAttack(other);

        isAttacking = true;
        timer = 0;
        finalDmg = dmg * dmgMulti;


        // Instantiate enemy bullet
       GameObject obj = Instantiate(bullet, transform);
        EnemyBulletController bc = obj.GetComponent<EnemyBulletController>();

        bc.SetValues(finalDmg, projectileSpd, projectileLifetime);

        timer = 0;
    }
}
