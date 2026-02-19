using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : Singleton<GoldManager>
{
    [Header("Configuration")]
    [SerializeField] private int startingGold = 5;
    [SerializeField] public string currencyName = "Data Chips";

    public int _currentGold;
    [SerializeField]
    public int currentGold
    {
        get { return _currentGold; }
        private set
        {
            if (AudioManager.Instance != null && value > _currentGold && hasGivenStartingGold) // Only do if gaining curency
            {
                CurrencyCombo += Mathf.Clamp(value, 0, 5);
                //AudioManager.Instance.PlayOneShotUI(AudioManager.Instance.AudioCatalogueSO.Player_Sounds.CurrencyGained);
            }
                

            _currentGold = value;
        }
    }
    [field:SerializeField] public int maxGold { get; private set; }


    private int currencyCombo;
    public int CurrencyCombo
    {
        get { return currencyCombo; }
        set
        {

            currencyCombo = value;
            currencyCombo = Mathf.Clamp(currencyCombo, 0, 30);

            if (AudioManager.Instance != null)
            {
                //AudioManager.Instance.SetGlobalParameter(AudioManager.Instance.MUSICPARAM_CURRENCYCOMBO, CurrencyCombo);
            }

            

            if (currencyCombo > 0)
            {
                CurrencyComboTimer = true;
            }
            else
            {
                CurrencyComboTimer = false;
            }
        }
    }


    bool currencyComboTimer;
    bool CurrencyComboTimer
    {
        get { return currencyComboTimer; }
        set
        {
            if (currencyComboTimer == value) return;

            currencyComboTimer = value;

            if (value)
                StartCoroutine(CurrencyComborCoroutine());
            else
                StopCoroutine(CurrencyComborCoroutine());
        }
    }

    bool hasGivenStartingGold = false;

    private void OnEnable() 
    {
        //GameEventsManager.instance.goldEvents.onGoldGained += GoldGained;
        //GameEventsManager.instance.goldEvents.onGoldLost += GoldLost;
    }

    private void OnDisable() 
    {
        //GameEventsManager.instance.goldEvents.onGoldGained -= GoldGained;
        //GameEventsManager.instance.goldEvents.onGoldLost -= GoldLost;
    }

    private void Start()
    {
        currentGold = startingGold;
        hasGivenStartingGold = true;
        //GameEventsManager.instance.goldEvents.GoldChange(currentGold);
    }

    private void GoldGained(int gold) 
    {
        currentGold += gold;
        if (currentGold > maxGold)
        {
            currentGold = maxGold;
        }
        //GameEventsManager.instance.goldEvents.GoldChange(currentGold);
    }

    private void GoldLost(int gold)
    {
        currentGold -= gold;
        if (currentGold < 0)
        {
            currentGold = 0;
        }

        //GameEventsManager.instance.goldEvents.GoldChange(currentGold);
    }

    public IEnumerator CurrencyComborCoroutine()
    {
        while (currencyCombo > 0)
        {
            yield return new WaitForSeconds(0.35f);
            CurrencyCombo--;
        }

        CurrencyComboTimer = false;
    }
}
