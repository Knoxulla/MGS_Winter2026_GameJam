using UnityEngine;
using TMPro;

public class CurrencyDisplayUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text currencyDisplay;



    private void OnEnable()
    {
        //GameEventsManager.instance.goldEvents.onGoldChange += UpdateCurrencyDisplay;

        UpdateCurrencyDisplay(GoldManager.Instance.currentGold);
    }

    private void OnDisable()
    {
        //GameEventsManager.instance.goldEvents.onGoldChange -= UpdateCurrencyDisplay;
    }

    private void UpdateCurrencyDisplay(int currency)
    {
        currencyDisplay.text = $"{currency}";
    }
}
