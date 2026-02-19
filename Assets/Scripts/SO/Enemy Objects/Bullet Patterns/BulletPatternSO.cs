using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BulletPatternSO_", menuName = "Bullet Hell Mechanics/Create Bullet Pattern...")]
public class BulletPatternSO : ScriptableObject
{
    #region Variables

    [Header("General Settings")]
    [Tooltip("Bullet info for this pattern to use.")]
    public BulletSO bulletSO;

    [Tooltip("What pattern archetype is this attack?")]
    public BulletPatternType patternType;

    [HideInInspector]
    public BulletAllegiance allegiance;


    [Tooltip("Speed of the bullets when fired.")]
    [Range(0.01f, 50f)]
    public float bulletSpeed = 10f;

    [Tooltip("how long until bullets despawn and pattern ends. Measured in seconds")]
    [Range(0.001f, 180f)]
    public float patternLifetime = 10f;

    [Tooltip("Spread angle between multiple bullets.")]
    [Range(0, 360)]
    public int spreadAngle = 0;


    [Tooltip("Where the pattern should spawn from the spawn point.")]
    public Vector2 spawnLocation = new Vector2(0f, 0f);

  
    [Tooltip("radius distance pattern should spawn from the spawn point.")]
    [Range(0, 50)]
    public float spawnDistance = 0f;

    [Tooltip("Offset the starting angle")]
    [Range(0,360)]
    public int angleOffset = 0;

    [Header("Firing Settings")]
    [Tooltip("Should this attack aim at the player (spiral and burst will spawn on the player location)")]
    [Min(1)]
    public bool player_aimed = false;


    [Tooltip("Direction to head if player_aimed is not toggled")]

    public Vector2 defaultDirection;


    [Tooltip("Time it takes to start firing this pattern")]
    public float windUpTime;

    [Tooltip("Number of bullets per instance")]
    public int bulletsToFire = 1;

    [Tooltip("Should this pattern repeat indefinitely?")]
    public bool loop = false;

   
    [Tooltip("Number of pattern instances per call.")]

    public int numberOfInstances = 1;


    [Tooltip("Delay between pattern instances.")]

    public float delayBetween = 0.1f;


    [Header("Spiral Settings")]
    [Tooltip("Choose whether the spiral should go clockwise (true) or counter-clockwise (false).")]
    public bool clockwiseSpiral = true;
    

    [Tooltip("How long should there be between bullets firing")]
    public float bulletDelay = 0f;
    
    [Header("Bezier Path Settings")]
    [Tooltip("Should bullets follow a cubic Bezier path?")]
    public bool useBezierPath = false;


    [SerializeField]
    [Tooltip("If Toggled on the bezier will keep moving after the curve ends")]
    public bool keepMoving = true;


    [SerializeField]
    [Tooltip("If Toggled on the bezier will not use default direction to calculate the rest of the path after the curve ends. REQUIRES KeepMoving = true.")]
    public bool calculatePath = true;


    [SerializeField]
    [Tooltip("If Toggled on the path will rotate around the enemy so that it faces the area the player is [all points must be positive for this to happen, negative = away from player]")]
    public bool pathRelativeToPlayer = false;

    [Tooltip("How should paths be selected for bullets?")]
    public PathSelectionMode pathSelectionMode = PathSelectionMode.Random;

    [SerializeField]
    [Tooltip("List of Bezier paths (each path has 4 control points).")]
    public List<BezierPath> bezierPaths = new List<BezierPath>();

    [Space(5)]

    [Tooltip("Weights for each path selection. Total weight will be used to determine chance.")]
    public List<float> pathWeights = new List<float>();

    #endregion
    public enum BulletPatternType
    {
        Linear,
        Burst,
        Spiral,
        Spread,
        Random,
        ReverseBurst
    }
    public enum PathSelectionMode
    {
        Random,
        Sequential,
        Weighted,
        NoPath
    }

    #region PathSelection
    public Vector3[] GetSelectedPath()
    {
        if (useBezierPath && bezierPaths.Count > 0)
        {
            switch (pathSelectionMode)
            {
                case PathSelectionMode.Random:
                    return GetRandomPath();
                case PathSelectionMode.Sequential:
                    return GetSequentialPath();
                case PathSelectionMode.Weighted:
                    return GetWeightedPath();
            }
        }

        return new Vector3[0];
    }

    private Vector3[] GetRandomPath()
    {
        if (bezierPaths.Count == 0)
            return new Vector3[0];

        return bezierPaths[Random.Range(0, bezierPaths.Count)].pathPoints.ToArray();
    }

    private Vector3[] GetSequentialPath()
    {
        if (bezierPaths.Count == 0)
            return new Vector3[0];

        int lastUsedIndex = PlayerPrefs.GetInt("LastUsedBezierPath", -1);
        int nextPathIndex = (lastUsedIndex + 1) % bezierPaths.Count;
        PlayerPrefs.SetInt("LastUsedBezierPath", nextPathIndex);

        return bezierPaths[nextPathIndex].pathPoints.ToArray();
    }

    private Vector3[] GetWeightedPath()
    {
        if (bezierPaths.Count == 0 || pathWeights.Count != bezierPaths.Count)
            return new Vector3[0];

        float totalWeight = 0;
        foreach (float weight in pathWeights)
        {
            totalWeight += weight;
        }

        float randomWeight = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0;

        for (int i = 0; i < pathWeights.Count; i++)
        {
            cumulativeWeight += pathWeights[i];
            if (randomWeight < cumulativeWeight)
            {
                return bezierPaths[i].pathPoints.ToArray();
            }
        }

        return bezierPaths[bezierPaths.Count - 1].pathPoints.ToArray();
    }
    #endregion
    public void TriggerPattern(Transform spawnPosition, Quaternion spawnRotation, MonoBehaviour runner, bool fromBullet, BulletAllegiance incomingAllegiance)
    {

        IBulletPatternHandler handler = BulletPatternHandlerFactory.GetHandler(patternType);
        allegiance = incomingAllegiance;
        if (handler != null)
        {
            //Debug.LogWarning("Incoming Spawn Position:" + spawnPosition.x + ", " + spawnPosition.y + ", " + spawnRotation.z);
            handler.FirePattern(spawnPosition.position, spawnRotation, this, runner, fromBullet, incomingAllegiance);
        }
        else
        {
            Debug.LogWarning($"[BulletPatternSO] No handler found for pattern type: {patternType}");
        }
    }

    [System.Serializable]
    public class BezierPath
    {
        [Tooltip("Requires 3 or 4 vectors to properly calculate path for Bezier")]
        public List<Vector3> pathPoints = new List<Vector3>(4);
    }

    public static class BulletPatternHandlerFactory
    {
        public static IBulletPatternHandler GetHandler(BulletPatternType patternType)
        {
            switch (patternType)
            {
                case BulletPatternType.Linear:
                    return new LinearPatternHandler();
                case BulletPatternType.Burst:
                    return new BurstPatternHandler();
                case BulletPatternType.Spiral:
                    return new SpiralPatternHandler();
                case BulletPatternType.Random:
                    return new RandomPatternHandler();
                case BulletPatternType.Spread:
                    return new SpreadPatternHandler();
                case BulletPatternType.ReverseBurst:
                    return new ReverseBurstPatternHandler();
                default:
                    Debug.LogError("No handler found for pattern: " + patternType);
                    return null;
            }
        }
    }
}
