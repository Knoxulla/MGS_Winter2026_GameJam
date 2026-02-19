using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Splines;

public class EnemyMovementController : MonoBehaviour
{
    private float speed = 2;
    public float speedMulti = 1;

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
            rb.linearVelocity = targetDirection.normalized * speed * speedMulti; 
        }

        
        if (Vector2.Distance(PlayerMASTER.Instance.playerMovementController.gameObject.GetComponent<Transform>().position, transform.position) > 1f
             && targetDirection.normalized.x > 0.5f)
        {
            spriteRenderer.flipX = true;    
        }
        else if (Vector2.Distance(PlayerMASTER.Instance.playerMovementController.gameObject.GetComponent<Transform>().position,transform.position) > 1f
            && targetDirection.normalized.x < 0.5f)
        {
            spriteRenderer.flipX = false;
        }

        if (Vector2.Distance(PlayerMASTER.Instance.playerMovementController.gameObject.GetComponent<Transform>().position, transform.position) > 1f
             && targetDirection.normalized.y > 0.5f)
        {
            spriteRenderer.sortingOrder = 200;    
        }
        else if (Vector2.Distance(PlayerMASTER.Instance.playerMovementController.gameObject.GetComponent<Transform>().position,transform.position) > 1f
            && targetDirection.normalized.y < 0.5f)
        {
            spriteRenderer.sortingOrder = 20;
        }
    }

        public void SpeedMultiplier(float newSpeedMulti)
    {
        speedMulti = Mathf.Clamp(newSpeedMulti,0.01f, 10f);
    }
}
