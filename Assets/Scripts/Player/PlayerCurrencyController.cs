using UnityEngine;

public class PlayerCurrencyController : MonoBehaviour
{
    public float currentXP;
    public float maxXP;
    public float currentCurrency;

    public float investedCurrency;


    private void Start()
    {
    }
    public void AddCurrency(float currencyAmt)
    { 
        currentCurrency += currencyAmt;
        EventManager.Instance.player_events.PlayerExpChanged(currentXP);
    }

    public void ResetMoney()
    { 
        currentCurrency = 0;
    }

    public void InvestMoney()
    {
        investedCurrency = currentCurrency;
        currentCurrency = 0;
    }
}
