using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class HUD_Controller : MonoBehaviour
{
    [Header("Health Display")]
    [SerializeField] Image playerHealthFill;
    [SerializeField] TMP_Text playerHealthText;

    private void OnEnable()
    {
        EventManager.Instance.player_events.OnPlayerHealthChanged += UpdatePlayerHealth;
    }

    private void OnDisable()
    {
        EventManager.Instance.player_events.OnPlayerHealthChanged -= UpdatePlayerHealth;
    }

    private void UpdatePlayerHealth(float health)
    {
        playerHealthFill.fillAmount = PlayerMASTER.Instance.playerHealthController.currentHealth / PlayerMASTER.Instance.playerHealthController.maxHealth;

        playerHealthText.text = $"{PlayerMASTER.Instance.playerHealthController.currentHealth} / {PlayerMASTER.Instance.playerHealthController.maxHealth}\n {Mathf.RoundToInt(playerHealthFill.fillAmount * 100)}%";
    
    }

}
