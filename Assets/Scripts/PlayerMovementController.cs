using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    Vector2 input;
    public float speed = 5;

    void Start()
    {
        input = Vector2.zero;
    }
    private void Update()
    {
        rb.AddForce(input * speed);
    }
    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }
}