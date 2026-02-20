using UnityEngine;

public class MeleeEnemyMovement : EnemyMovementController
{

    [Header("Animation")]
    [SerializeField] Animator anim;

    [Header("Fight")]
    [SerializeField] AnimationClip FightUp;
    [SerializeField] AnimationClip FightDown;
    [SerializeField] AnimationClip FightLeft;
    [SerializeField] AnimationClip FightRight;

    [Header("Regular")]
    [SerializeField] AnimationClip walkUp;
    [SerializeField] AnimationClip walkDown;
    [SerializeField] AnimationClip walkLeft;
    [SerializeField] AnimationClip walkRight;


    public override void SetVelocity()
    {

        if (targetDirection == Vector2.zero || GetComponent<EnemyMASTER>().attackController.isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = targetDirection.normalized * speed * speedMulti;
        }

        // fighting
        if (GetComponent<EnemyMASTER>().attackController.isAttacking)
        {
            if (Vector2.Distance(PlayerMASTER.Instance.playerMovementController.gameObject.GetComponent<Transform>().position, transform.position) > 1f
                && targetDirection.normalized.x > 0f && targetDirection.normalized.y > -0.5f && targetDirection.normalized.y < 0.5f)
            {
                anim.Play(FightRight.name);
            }
            // left
            else if (Vector2.Distance(PlayerMASTER.Instance.playerMovementController.gameObject.GetComponent<Transform>().position, transform.position) > 1f
                && targetDirection.normalized.x < 0f && targetDirection.normalized.y > -0.5f && targetDirection.normalized.y < 0.5f)
            {
                anim.Play(FightLeft.name);
            }
            // Up
            else if (Vector2.Distance(PlayerMASTER.Instance.playerMovementController.gameObject.GetComponent<Transform>().position, transform.position) > 1f
                && targetDirection.normalized.y > 0f && targetDirection.normalized.x > -0.5f && targetDirection.normalized.x < 0.5f)
            {
                anim.Play(FightUp.name);
            }

            // Down
            else if (Vector2.Distance(PlayerMASTER.Instance.playerMovementController.gameObject.GetComponent<Transform>().position, transform.position) > 1f
                && targetDirection.normalized.y < 0f && targetDirection.normalized.x > -0.5f && targetDirection.normalized.x < 0.5f)
            {
                anim.Play(FightDown.name);
            }
            return;
        }

        // Regular Walking

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
