using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float rotationSpeed;

    private Rigidbody2D rb;
    private PlayerDetectionController playerDetectionController;
    private Vector2 targetDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerDetectionController = GetComponent<PlayerDetectionController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
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

    private void RotateTowardsTarget()
    {
        if (targetDirection == Vector2.zero)
        {
            return;
        } 
        
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        rb.SetRotation(rotation);
    }

    private void SetVelocity()
    {
        if (targetDirection == Vector2.zero)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = transform.up * speed; 
        }
    }
}
