using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [Header("Animation")]
    [SerializeField] Animator anim;

    [Header("With gun")]
    [SerializeField] AnimationClip walkUpGun;
    [SerializeField] AnimationClip walkDownGun;
    [SerializeField] AnimationClip walkLeftGun;
    [SerializeField] AnimationClip walkRightGun;
    [SerializeField] AnimationClip idleGunUp;
    [SerializeField] AnimationClip idleGunDown;
    [SerializeField] AnimationClip idleGunLeft;
    [SerializeField] AnimationClip idleGunRight;

    [Header("No Gun")]
    [SerializeField] AnimationClip walkUp;
    [SerializeField] AnimationClip walkDown;
    [SerializeField] AnimationClip walkLeft;
    [SerializeField] AnimationClip walkRight;
    [SerializeField] AnimationClip idleUp;
    [SerializeField] AnimationClip idleDown;
    [SerializeField] AnimationClip idleLeft;
    [SerializeField] AnimationClip idleRight;

    Vector2 input;
    [SerializeField] private float speed = 10;


    public float adjustedSpeed = 1f;

    void Start()
    {
        input = Vector2.zero;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.AddForce(input * (speed * adjustedSpeed));

        RotatePlayer();

    }

    private void RotatePlayer()
    {
        Vector2 mousePos = PlayerMASTER.Instance.playerAttackController.mousePos;


        if (input.x == 0f && input.y == 0 && !PlayerMASTER.Instance.playerAttackController.isAttacking)
        {
            if (mousePos.y > 0 && mousePos.x > -0.5f && mousePos.x < 0.5f)
            {
                anim.Play(idleUp.name);
            }
            else if (mousePos.y < 0 && mousePos.x > -0.5f && mousePos.x < 0.5f)
            {
                anim.Play(idleDown.name);
            }
            else if (mousePos.x > 0f && mousePos.y > -0.5f && mousePos.y < 0.5f)
            {
                anim.Play(idleRight.name);
            }
            else if (mousePos.x < 0f && mousePos.y > -0.5f && mousePos.y < 0.5f)
            {
                anim.Play(idleLeft.name);
            }

            return;
        }
        else if (input.x == 0f && input.y == 0 && !PlayerMASTER.Instance.playerAttackController.isAttacking)
        {
            if (mousePos.y > 0 && mousePos.x > -0.5f && mousePos.x < 0.5f)
            {
                anim.Play(idleGunUp.name);
            }
            else if (mousePos.y < 0 && mousePos.x > -0.5f && mousePos.x < 0.5f)
            {
                anim.Play(idleGunDown.name);
            }
            else if (mousePos.x > 0f && mousePos.y > -0.5f && mousePos.y < 0.5f)
            {
                anim.Play(idleGunRight.name);
            }
            else if (mousePos.x < 0f && mousePos.y > -0.5f && mousePos.y < 0.5f)
            {
                anim.Play(idleGunLeft.name);
            }

            return;
        }



        if (PlayerMASTER.Instance.playerAttackController.isAttacking)
        {
            if (mousePos.y > 0 && mousePos.x > -0.5f && mousePos.x < 0.5f)
            {
                anim.Play(walkUpGun.name);
            }
            else if (mousePos.y < 0 && mousePos.x > -0.5f && mousePos.x < 0.5f)
            {
                anim.Play(walkDownGun.name);
            }
            else if (mousePos.x > 0f && mousePos.y > -0.5f && mousePos.y < 0.5f)
            {
                anim.Play(walkRightGun.name);
            }
            else if (mousePos.x < 0f && mousePos.y > -0.5f && mousePos.y < 0.5f)
            {
                anim.Play(walkLeftGun.name);
            }
        }

        else
        {
            if (mousePos.y > 0 && mousePos.x > -0.5f && mousePos.x < 0.5f)
            {
                anim.Play(walkUp.name);
            }
            else if (mousePos.y < 0 && mousePos.x > -0.5f && mousePos.x < 0.5f)
            {
                anim.Play(walkDown.name);
            }
            else if (mousePos.x > 0f && mousePos.y > -0.5f && mousePos.y < 0.5f)
            {
                anim.Play(walkRight.name);
            }
            else if (mousePos.x < 0f && mousePos.y > -0.5f && mousePos.y < 0.5f)
            {
                anim.Play(walkLeft.name);
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    public void SpeedAdjustment(float newSpeedAdjustment)
    {
        adjustedSpeed = Mathf.Clamp(newSpeedAdjustment, 0.01f, 10f);
    }
}