using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    Vector2 input;
    private float speed = 10;

    public float adjustedSpeed = 1f;

    void Start()
    {
        input = Vector2.zero;
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        rb.AddForce(input * (speed * adjustedSpeed));
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