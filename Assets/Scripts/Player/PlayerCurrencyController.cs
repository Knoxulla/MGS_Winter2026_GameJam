using UnityEngine;

public class PlayerCurrencyController : MonoBehaviour
{
    public float currentXP;
    public float maxXP;
    public float currentCurrency;


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
}
