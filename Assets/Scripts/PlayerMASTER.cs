using UnityEngine;

public class PlayerMASTER : MonoBehaviour
{
    public static PlayerMASTER Instance;

    public PlayerAttackController playerAttackController;
    public PlayerMovementController playerMovementController;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
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
    }
}
