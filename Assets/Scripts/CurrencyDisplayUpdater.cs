using UnityEngine;
using TMPro;

public class CurrencyDisplayUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text currencyDisplay;



    private void Update()
    {
        //GameEventsManager.instance.goldEvents.onGoldChange += UpdateCurrencyDisplay;

        UpdateCurrencyDisplay(PlayerMASTER.Instance.playerCurrencyController.currentCurrency);
    }

    private void OnDisable()
    {
        //GameEventsManager.instance.goldEvents.onGoldChange -= UpdateCurrencyDisplay;
    }

    private void UpdateCurrencyDisplay(float currency)
    {
        currencyDisplay.text = $"{currency}";
    }
}
