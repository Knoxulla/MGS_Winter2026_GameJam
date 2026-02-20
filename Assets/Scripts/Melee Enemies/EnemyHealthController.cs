using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] public float maxHealth = 10;
    [SerializeField] public float currentHealth = 10;
    [SerializeField] public float expToDrop = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = maxHealth * RoundSystemController.Instance.healthMulti;
        currentHealth = maxHealth;
    }


    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        checkDeath();
    }

    public void checkDeath()
    {
        if (currentHealth <= 0)
        {
            PlayerMASTER.Instance.playerExperienceController.AddExp(expToDrop);
            Destroy(gameObject);
        }   
    }
}
