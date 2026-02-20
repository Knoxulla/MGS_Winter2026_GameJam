using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;

public class ShopSystemController : MonoBehaviour
{
    public static ShopSystemController Instance { get; private set; }

    [SerializeField] public List<UpgradeSO> unlockableUpgrades;
    [SerializeField] public List<UpgradeSO> lockedUpgrades;

    [SerializeField] public ItemSlot itemslot1;
    [SerializeField] public ItemSlot itemslot2;
    [SerializeField] public ItemSlot itemslot3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseShop()
    {
        gameObject.SetActive(false);
    }

    public void OpenShop()
    {
        gameObject.SetActive(true);
    }

    public void BuyUpgrade1()
    {
        foreach (UpgradeSO x in unlockableUpgrades) 
        {
            if (x.upgradeID == itemslot1.itemID && x.cost <= PlayerMASTER.Instance.playerCurrencyController.currentCurrency)
            {
                if (!x.isReoccuring)
                {
                    unlockableUpgrades.Remove(x);
                }   
                PlayerMASTER.Instance.playerCurrencyController.AddCurrency(-x.cost);
            }
        }
    }

    public void BuyUpgrade2()
    {
        foreach (UpgradeSO x in unlockableUpgrades) 
        {
            if (x.upgradeID == itemslot2.itemID && x.cost <= PlayerMASTER.Instance.playerCurrencyController.currentCurrency)
            {
                if (!x.isReoccuring)
                {
                    unlockableUpgrades.Remove(x);
                }   
                PlayerMASTER.Instance.playerCurrencyController.AddCurrency(-x.cost);
            }
        }
    }

    public void BuyUpgrade3()
    {
        foreach (UpgradeSO x in unlockableUpgrades) 
        {
            if (x.upgradeID == itemslot3.itemID && x.cost <= PlayerMASTER.Instance.playerCurrencyController.currentCurrency)
            {
                if (!x.isReoccuring)
                {
                    unlockableUpgrades.Remove(x);
                }   
                PlayerMASTER.Instance.playerCurrencyController.AddCurrency(-x.cost);
            }
        }
    }

    public void OnEnable()
    {
        
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

                itemslot1.title.text = unlockableUpgrades[i].upgradeName;
                itemslot1.cost.text = unlockableUpgrades[i].cost.ToString();
                itemslot1.description.text = unlockableUpgrades[i].upgradeDescription;
                itemslot1.iconImg.sprite = unlockableUpgrades[i].upgradeIcon;
            }
            if (i == slot3)
            {
                itemslot3.itemID = unlockableUpgrades[i].upgradeID;
                itemslot3.slotButton.onClick.AddListener(BuyUpgrade3);

                itemslot1.title.text = unlockableUpgrades[i].upgradeName;
                itemslot1.cost.text = unlockableUpgrades[i].cost.ToString();
                itemslot1.description.text = unlockableUpgrades[i].upgradeDescription;
                itemslot1.iconImg.sprite = unlockableUpgrades[i].upgradeIcon;
            }
        }
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