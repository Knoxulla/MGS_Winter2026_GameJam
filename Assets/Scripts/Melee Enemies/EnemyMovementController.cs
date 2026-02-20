using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Splines;

public class EnemyMovementController : MonoBehaviour
{
    [SerializeField]
    protected float speed = 2;
    public float speedMulti = 1;

    protected Rigidbody2D rb;
    protected PlayerDetectionController playerDetectionController;
    protected Vector2 targetDirection;

    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerDetectionController = GetComponent<PlayerDetectionController>();
    }

    void Start()
    {
        speedMulti = RoundSystemController.Instance.speedMulti;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateTargetDirection();
        SetVelocity();
    }

    private void UpdateTargetDirection()
    {
        if (playerDetectionController.detectedPlayer)
        {
            targetDirection = playerDetectionController.DirectionToPlayer;
        }
        else
        {
            targetDirection = Vector2.zero;
        }
    }

    public virtual void SetVelocity()
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
        else if (Vector2.Distance(PlayerMASTER.Instance.playerMovementController.gameObject.GetComponent<Transform>().position, transform.position) > 1f
            && targetDirection.normalized.x < 0.5f)
        {
            spriteRenderer.flipX = false;
        }
    }
}
