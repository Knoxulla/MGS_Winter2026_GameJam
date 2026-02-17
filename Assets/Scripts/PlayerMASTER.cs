using UnityEngine;

public class PlayerMASTER : MonoBehaviour
{
    public static PlayerMASTER Instance { get; private set; }

    public PlayerAttackController playerAttackController;
    public PlayerMovementController playerMovementController;
    public PlayerHealthController playerHealthController;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else 
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        playerAttackController = GetComponent<PlayerAttackController>();
        playerMovementController = GetComponent<PlayerMovementController>();
        playerHealthController = GetComponent<PlayerHealthController>();
    }
}
