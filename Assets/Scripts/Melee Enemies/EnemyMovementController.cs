using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Rigidbody2D rb;
    private PlayerDetectionController playerDetectionController;
    private Vector2 targetDirection;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerDetectionController = GetComponent<PlayerDetectionController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateTargetDirection();
        SetVelocity();
    }

    private void UpdateTargetDirection()
    {
        if(playerDetectionController.detectedPlayer)
        {
            targetDirection = playerDetectionController.DirectionToPlayer;
        } 
        else
        {
            targetDirection = Vector2.zero;
        }
    }

    private void SetVelocity()
    {
        if (targetDirection == Vector2.zero)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = targetDirection.normalized * speed; 
        }

        
        if (targetDirection.x > 0f)
        {
            spriteRenderer.flipX = true;
        }
        else if (targetDirection.x < 0f)
        {
            spriteRenderer.flipX = false;
        }
    }
}
