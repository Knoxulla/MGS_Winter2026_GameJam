using UnityEngine;

public class ShopSystemController : MonoBehaviour
{
    public static ShopSystemController Instance { get; private set; }
    
    [SerializeField]
    public int itemOneID;
    [SerializeField]
    public int itemTwoID;
    [SerializeField]
    public int item3ID;

    [SerializeField]
    public UpgradeSO[] unlockableUpgrades;
    public UpgradeSO[] lockedUpgrades;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void closeShop()
    {
        gameObject.SetActive(false);
    }

    public void openShop()
    {
        gameObject.SetActive(true);
    }

    public void buyUpgrade(int upgradeId)
    {
        foreach (UpgradeSO x in unlockableUpgrades) 
        {
            if (x.upgradeID == upgradeId)
            {
                if (x.)   
            }
        }
    }

    public void OnEnable()
    {
        
    }
}
