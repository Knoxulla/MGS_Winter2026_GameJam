using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class RoundSystemController : MonoBehaviour
{
    public static RoundSystemController Instance { get; private set; }

    public EnemyHealthController enemyHealthController;
    public EnemyAttackController enemyAttackController;
    public EnemyMovementController enemyMovementController;
    public PlayerExperienceController playerExperienceController;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
