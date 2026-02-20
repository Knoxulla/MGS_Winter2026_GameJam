using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;
using System.Security.Cryptography;
using Unity.Collections;
using UnityEngine.Networking;

public class ShopSystemController : MonoBehaviour
{
    public static ShopSystemController Instance { get; private set; }

    public GameObject shopObject;

    public Button invest;
    public Button closeShop;
    public TMP_Text currentInvestment;
    public TMP_Text ROI;

    [SerializeField] public List<UpgradeSO> unlockableUpgrades;
    [SerializeField] public List<UpgradeSO> lockedUpgrades;

    [SerializeField] public ItemSlot itemslot1;
    [SerializeField] public ItemSlot itemslot2;
    [SerializeField] public ItemSlot itemslot3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shopObject.SetActive(false);
        invest.onClick.AddListener(PlayerMASTER.Instance.playerCurrencyController.InvestMoney);
        invest.onClick.AddListener(UpdateInvestment);
        closeShop.onClick.AddListener(CloseShop);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CloseShop()
    {
        for (int i = 0; i < unlockableUpgrades.Count; i ++)
        {
            if (unlockableUpgrades[i].upgradeID == itemslot1.itemID || unlockableUpgrades[i].upgradeID == itemslot2.itemID || unlockableUpgrades[i].upgradeID == itemslot3.itemID)
            {
                if (!unlockableUpgrades[i].isReoccuring)
                {
                    unlockableUpgrades.Remove(unlockableUpgrades[i]);
                } 
            }  
        }
        shopObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenShop()
    {
        Time.timeScale = 0f;

        if (PlayerMASTER.Instance.playerExperienceController.currentLevel % 3 == 0)
        {
            if (PlayerMASTER.Instance.playerCurrencyController.investedCurrency <= 100) 
            {
              PlayerMASTER.Instance.playerCurrencyController.AddCurrency(PlayerMASTER.Instance.playerCurrencyController.investedCurrency * 1.20f);
            } 
            else if (PlayerMASTER.Instance.playerCurrencyController.investedCurrency < 500) 
            {
              PlayerMASTER.Instance.playerCurrencyController.AddCurrency(PlayerMASTER.Instance.playerCurrencyController.investedCurrency * 1.40f);   
            }
            else if (PlayerMASTER.Instance.playerCurrencyController.investedCurrency >= 900) 
            {
              PlayerMASTER.Instance.playerCurrencyController.AddCurrency(PlayerMASTER.Instance.playerCurrencyController.investedCurrency * 1.60f);  
            }
        }

        if (PlayerMASTER.Instance.playerCurrencyController.investedCurrency <= 100) 
        {
            ROI.text = (PlayerMASTER.Instance.playerCurrencyController.investedCurrency * 1.20f).ToString();
        }  
        else if (PlayerMASTER.Instance.playerCurrencyController.investedCurrency < 500) 
        { 
            ROI.text = (PlayerMASTER.Instance.playerCurrencyController.investedCurrency * 1.40f).ToString();
        }
        else if (PlayerMASTER.Instance.playerCurrencyController.investedCurrency >= 900) 
        {   
            ROI.text = (PlayerMASTER.Instance.playerCurrencyController.investedCurrency * 1.60f).ToString(); 
        }

        int slot1 = Random.Range(1, unlockableUpgrades.Count);
        itemslot1.slotButton.enabled = true;
        int slot2 = Random.Range(1, unlockableUpgrades.Count);
        itemslot2.slotButton.enabled = true;
        int slot3 = Random.Range(1, unlockableUpgrades.Count);
        itemslot3.slotButton.enabled = true;

        while (slot2 == slot1)
        {
            slot2 = Random.Range(1, unlockableUpgrades.Count);
        }

        while (slot3 == slot2 || slot3 == slot1)
        {
            slot3 = Random.Range(1, unlockableUpgrades.Count);
        }

        AssignButtons(slot1, slot2, slot3);

        shopObject.SetActive(true);
    }

    public void BuyUpgrade1()
    {
        foreach (UpgradeSO x in unlockableUpgrades) 
        {
            if (x.upgradeID == itemslot1.itemID && x.cost <= PlayerMASTER.Instance.playerCurrencyController.currentCurrency)
            {
            
                PlayerMASTER.Instance.playerCurrencyController.AddCurrency(-x.cost);
                itemslot1.slotButton.enabled = false;

                ApplyUpgrade(x);
            }
        }
    }

    public void ApplyUpgrade(UpgradeSO upgrade)
    {

        if (upgrade.spawnsInWorld == true)
        {
            Instantiate(upgrade.spawnableObj,PlayerMASTER.Instance.transform);
        }

        PlayerMASTER.Instance.playerMovementController.speed += upgrade.speed;
        PlayerMASTER.Instance.playerHealthController.maxHealth += upgrade.maxHealth;
        PlayerMASTER.Instance.playerHealthController.currentHealth += upgrade.currentHealth;
        PlayerMASTER.Instance.playerAttackController.baseDmg += upgrade.baseDamage;

        if (upgrade.attackChange != null)
        {
            PlayerMASTER.Instance.playerAttackController.attackPattern = upgrade.attackChange;
        }
    }

    public void BuyUpgrade2()
    {
        foreach (UpgradeSO x in unlockableUpgrades) 
        {
            if (x.upgradeID == itemslot2.itemID && x.cost <= PlayerMASTER.Instance.playerCurrencyController.currentCurrency)
            {
                PlayerMASTER.Instance.playerCurrencyController.AddCurrency(-x.cost);
                itemslot2.slotButton.enabled = false;

                ApplyUpgrade(x);
            }
        }
    }

    public void BuyUpgrade3()
    {
        foreach (UpgradeSO x in unlockableUpgrades) 
        {
            if (x.upgradeID == itemslot3.itemID && x.cost <= PlayerMASTER.Instance.playerCurrencyController.currentCurrency)
            {
                PlayerMASTER.Instance.playerCurrencyController.AddCurrency(-x.cost);
                itemslot3.slotButton.enabled = false;

                ApplyUpgrade(x);
            }
        }
    }

    public void AssignButtons(int slot1, int slot2, int slot3)
    {

        for (int i = 0; i < unlockableUpgrades.Count; i++ ) {

            if (i == slot1)
            {
                itemslot1.itemID = unlockableUpgrades[i].upgradeID;
                itemslot1.slotButton.onClick.AddListener(BuyUpgrade1);

                itemslot1.title.text = unlockableUpgrades[i].upgradeName;
                itemslot1.cost.text = unlockableUpgrades[i].cost.ToString();
                itemslot1.description.text = unlockableUpgrades[i].upgradeDescription;
                itemslot1.iconImg.sprite = unlockableUpgrades[i].upgradeIcon;
            }
            if (i == slot2)
            {
                itemslot2.itemID = unlockableUpgrades[i].upgradeID;
                itemslot2.slotButton.onClick.AddListener(BuyUpgrade2);

                itemslot2.title.text = unlockableUpgrades[i].upgradeName;
                itemslot2.cost.text = unlockableUpgrades[i].cost.ToString();
                itemslot2.description.text = unlockableUpgrades[i].upgradeDescription;
                itemslot2.iconImg.sprite = unlockableUpgrades[i].upgradeIcon;
            }
            if (i == slot3)
            {
                itemslot3.itemID = unlockableUpgrades[i].upgradeID;
                itemslot3.slotButton.onClick.AddListener(BuyUpgrade3);

                itemslot3.title.text = unlockableUpgrades[i].upgradeName;
                itemslot3.cost.text = unlockableUpgrades[i].cost.ToString();
                itemslot3.description.text = unlockableUpgrades[i].upgradeDescription;
                itemslot3.iconImg.sprite = unlockableUpgrades[i].upgradeIcon;
            }
        }
    }
    public void UpdateInvestment()
    {
        currentInvestment.text = PlayerMASTER.Instance.playerCurrencyController.investedCurrency.ToString();
    }
}

[System.Serializable]
public class ItemSlot
{
    public Button slotButton;
    public TMP_Text title;
    public TMP_Text cost;
    public TMP_Text description;
    public Image iconImg;
    public int itemID;


    
}