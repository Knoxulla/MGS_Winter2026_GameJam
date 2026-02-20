using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    public GameObject chillyEnemyPrefab;
    public GameObject battyEnemyPrefab;
    public Transform[] spawnPoints; 
    public float spawnDelay = 3f;
    private float nextSpawnTime;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnDelay;
        }

    }

    void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        if (Random.Range(1,5) == 1) 
        {
            Instantiate(battyEnemyPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
        } else
        {
            Instantiate(chillyEnemyPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
        }
    }
}
