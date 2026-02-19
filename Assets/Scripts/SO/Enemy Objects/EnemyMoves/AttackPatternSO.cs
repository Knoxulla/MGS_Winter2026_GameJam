using UnityEngine;

[CreateAssetMenu(fileName = "AttackPatternSO_", menuName = "Bullet Hell Mechanics/Create Attack Pattern...")]
public class AttackPatternSO : ScriptableObject
{
    [Tooltip("Array of attack sequences to choose from.")]
    public AttackSequence[] attackSequences;

    [Tooltip("Time delay between attacks.")]
    public float timeBetweenAttacks;

    // This method will return an attack based on the weighted chance to happen.
    public AttackSO GetRandomAttack()
    {
        int totalChance = 0;

        // Calculate the total chance 
        foreach (AttackSequence sequence in attackSequences)
        {
            totalChance += sequence.chanceToHappen;
        }

        // Pick a random value based on total chance
        int randomValue = Random.Range(0, totalChance);
        int cumulativeChance = 0;

        // Find the attack sequence that corresponds to the random value
        foreach (AttackSequence sequence in attackSequences)
        {
            cumulativeChance += sequence.chanceToHappen;
            if (randomValue < cumulativeChance)
            {
                return sequence.attack; // Return the selected attack based on the weighted chance
            }
        }

        // If something goes wrong, fallback to the a random attack (shouldn't happen)
        return attackSequences[Random.Range(0,attackSequences.Length)].attack;
    }
}

[System.Serializable]
public class AttackSequence
{
    public AttackSO attack; // The attack to perform
    public int chanceToHappen;  // The probability of this attack happening (weight)
}
