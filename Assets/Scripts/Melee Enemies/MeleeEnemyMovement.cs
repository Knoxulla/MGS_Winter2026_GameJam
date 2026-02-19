using UnityEngine;

public class MeleeEnemyMovement : EnemyMovementController
{

    [Header("Animation")]
    [SerializeField] Animator anim;
    [SerializeField] AnimationClip walkUp;
    [SerializeField] AnimationClip walkDown;
    [SerializeField] AnimationClip walkLeft;
    [SerializeField] AnimationClip walkRight;


    public override void SetVelocity()
    {
        if (targetDirection == Vector2.zero)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = targetDirection.normalized * speed * speedMulti;
        }


       // if()

        // Right
        if (Vector2.Distance(PlayerMASTER.Instance.playerMovementController.gameObject.GetComponent<Transform>().position, transform.position) > 1f
             && targetDirection.normalized.x > 0f && targetDirection.normalized.y > -0.5f && targetDirection.normalized.y < 0.5f)
        {
            anim.Play(walkRight.name);
        }
        // left
        else if (Vector2.Distance(PlayerMASTER.Instance.playerMovementController.gameObject.GetComponent<Transform>().position, transform.position) > 1f
            && targetDirection.normalized.x < 0f && targetDirection.normalized.y > -0.5f && targetDirection.normalized.y < 0.5f)
        {
            anim.Play(walkLeft.name);
        }
        // Up
        else if (Vector2.Distance(PlayerMASTER.Instance.playerMovementController.gameObject.GetComponent<Transform>().position, transform.position) > 1f
            && targetDirection.normalized.y > 0f && targetDirection.normalized.x > -0.5f && targetDirection.normalized.x < 0.5f)
        {
            anim.Play(walkUp.name);
        }

        // Down
        else if (Vector2.Distance(PlayerMASTER.Instance.playerMovementController.gameObject.GetComponent<Transform>().position, transform.position) > 1f
            && targetDirection.normalized.y < 0f && targetDirection.normalized.x > -0.5f && targetDirection.normalized.x < 0.5f)
        {
            anim.Play(walkDown.name);
        }
    }
}
