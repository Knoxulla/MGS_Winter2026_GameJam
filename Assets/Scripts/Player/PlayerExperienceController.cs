using NUnit.Framework;
using UnityEngine;

public class PlayerExperienceController : MonoBehaviour
{
    public float currentXP;
    public float maxXP;
    public int currentLevel;
    public PlayerLevelValues playerLvlValues;

    public void AddExp(float xpAmt)
    { 
        currentXP = xpAmt;
    }

    public void ResetExp()
    { 
        currentXP = 0;
    }

    private void Update()
    {
        if (currentXP == maxXP)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    { 
        ResetExp();
        currentLevel += 1;
        // Apply the new round modifiers
    }
}

[System.Serializable]
public class PlayerLevelValues
{
    public float enemyDifficultyIncrease;
}