using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeSO_", menuName = "Scriptable Objects/UpgradeSO")]
public class UpgradeSO : ScriptableObject
{
    [Tooltip("For programming, please add one on every upgrade and note which upgrade with ID needs to be bought in order for another to becoem available")] public int upgradeID;
    [Tooltip("Can this item be bought over and over again?")] public bool isReoccuring;

    [Space(5)]

    [Header("Shop visuals")]
    public Sprite upgradeIcon;
    public string upgradeName;
    public string upgradeDescription;
    public int cost;

    [Space(5)]

    [Header("Stat changes")]
    [Tooltip("all are additive, so negative values will make someone slower/less health/etc for negatives to certain upgrades")]
    public float speed;
    public float maxHealth;
    [Tooltip("Quick buyable heal")] public float currentHealth;
    public float baseDamage;

    [Space(5)]

    [Header("Spawning Objects")]
    [Tooltip("For things like spinning blades that live in the world")] public bool spawnsInWorld = false;
    [Tooltip("Put prefab object here if above is true")] public GameObject spawnableObj;

    [Space(5)]

    [Header("Attack Changes")]
    public AttackPatternSO attackChange;

}
