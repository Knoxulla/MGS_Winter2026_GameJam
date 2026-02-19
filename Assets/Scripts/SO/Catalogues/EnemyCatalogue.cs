using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "EnemyCatalogue", menuName = "Catalogue/EnemyCatalogue")]
public class EnemyCatalogue : ScriptableObject
{
    public List<EnemyEntry> enemies;

    [System.Serializable]
    public class EnemyEntry
    {
        public int enemyID;
        public string Name;
        public Sprite enemyIcon;
        [Tooltip("This will be how they are referenced")]
        public GameObject enemy;
        [Tooltip("Short blurb about them, like lore")]
        public string description;

        [field:Space(10)]

        [field: SerializeField, Header("Object Pooling")]
        public EnemyHealthController EnemyHead { get; private set; }
        [field: SerializeField, Header("Object Pooling")]
        public int MaxPoolCount { get; private set; }
        [field: SerializeField]
        public int Base_RespawnTimerSeconds { get; private set; }
    }
}
