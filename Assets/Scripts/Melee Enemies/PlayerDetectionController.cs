using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectionController : MonoBehaviour
{
    public bool detectedPlayer {get; private set;}

    public Vector2 DirectionToPlayer {get; private set;}

    [SerializeField] 
    private float detectedPlayerDistance;

    private Transform player;

    void Awake()
    {
        PlayerMovementController playerController = FindAnyObjectByType<PlayerMovementController>();

        if (playerController != null)
        {
            player = playerController.transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        
        Vector2 enemyToPlayerVector = player.position - transform.position;
        DirectionToPlayer = enemyToPlayerVector.normalized;

        if (enemyToPlayerVector.magnitude <= detectedPlayerDistance)
        {
            detectedPlayer = true;
        } else
        {
            detectedPlayer = false;
        }
    }
}
