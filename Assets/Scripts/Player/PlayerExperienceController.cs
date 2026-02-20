using UnityEngine;

public class PlayerExperienceController : MonoBehaviour
{
    public float currentXP;
    public float maxXP;
    public int currentLevel;

    public ShopSystemController shop;


    private void Start()
    {
        ResetExp();
        currentLevel = 1;
        EventManager.Instance.player_events.PlayerExpChanged(currentXP);
    }
    public void AddExp(float xpAmt)
    { 
        currentXP += xpAmt;
        EventManager.Instance.player_events.PlayerExpChanged(currentXP);
    }

    public void ResetExp()
    { 
        currentXP = 0;
    }

    private void Update()
    {
        if (currentXP >= maxXP)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    { 
        ResetExp();
        currentLevel += 1;
        EventManager.Instance.player_events.PlayerExpChanged(currentXP);
        // Apply the new round modifiers
        RoundSystemController.Instance.UpdateStats();
        shop.OpenShop();
    }
}
