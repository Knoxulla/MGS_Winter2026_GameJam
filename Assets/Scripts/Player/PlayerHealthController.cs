using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] public float maxHealth;
    [SerializeField] public float currentHealth;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        UpdateHealth(currentHealth);
    }

    public void UpdateHealth(float newHealth)
    {
        currentHealth = Mathf.RoundToInt(Mathf.Clamp(newHealth, 0, maxHealth));
        EventManager.Instance.player_events.PlayerHealthChanged(currentHealth);
    }
}
