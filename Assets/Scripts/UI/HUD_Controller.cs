using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class HUD_Controller : MonoBehaviour
{
    [Header("Health Display")]
    [SerializeField] Image playerHealthFill;
    [SerializeField] TMP_Text playerHealthText;
    [Header("Experience Display")]
    [SerializeField] Image playerExperienceFill;
    private void OnEnable()
    {
        EventManager.Instance.player_events.OnPlayerHealthChanged += UpdatePlayerHealth;
        EventManager.Instance.player_events.OnPlayerExpChanged += UpdatePlayerExp;
    }

    private void OnDisable()
    {
        EventManager.Instance.player_events.OnPlayerHealthChanged -= UpdatePlayerHealth;
        EventManager.Instance.player_events.OnPlayerExpChanged -= UpdatePlayerExp;
    }

    private void UpdatePlayerHealth(float health)
    {
        playerHealthFill.fillAmount = PlayerMASTER.Instance.playerHealthController.currentHealth / PlayerMASTER.Instance.playerHealthController.maxHealth;

        playerHealthText.text = $"{PlayerMASTER.Instance.playerHealthController.currentHealth} / {PlayerMASTER.Instance.playerHealthController.maxHealth}\n {Mathf.RoundToInt(playerHealthFill.fillAmount * 100)}%";
    
    }

    private void UpdatePlayerExp(float exp)
    {
        playerExperienceFill.fillAmount = PlayerMASTER.Instance.playerExperienceController.currentXP / PlayerMASTER.Instance.playerExperienceController.maxXP;
    }

}
