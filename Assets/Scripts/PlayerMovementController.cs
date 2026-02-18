using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [Header("Animation")]
    [SerializeField] Animator anim;
    [SerializeField] AnimationClip walkUp;
    [SerializeField] AnimationClip walkDown;
    [SerializeField] AnimationClip walkLeft;
    [SerializeField] AnimationClip walkRight;

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

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    public void SpeedAdjustment(float newSpeedAdjustment)
    {
        adjustedSpeed = Mathf.Clamp(newSpeedAdjustment,0.01f, 10f);
    }
}