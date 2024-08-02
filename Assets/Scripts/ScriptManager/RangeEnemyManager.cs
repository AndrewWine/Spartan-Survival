using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyManager : MonoBehaviour
{
    public RangeEnemy rangeEnemyPrefab;
    private ObjectPool<RangeEnemy> rangeEnemyPool;
    [SerializeField] protected float spawnInterval = 2.0f; // Thời gian giữa các lần spawn
    public float SpawnInterval { get => spawnInterval; set => spawnInterval = value; }
    
    [SerializeField] protected  int maxRangeEnemies = 5; // Số lượng quái vật tối đa
    public int MaxRangeEnemies { get => maxRangeEnemies; set => maxRangeEnemies = value; }

    private List<RangeEnemy> activeRangeEnemies = new List<RangeEnemy>();

    private void Awake()
    {
        CheckRangeEnemyPool();
    }

   

    private void Start()
    {
        StartCoroutine(SpawnRangeEnemiesContinuously());
    }
     private void CheckRangeEnemyPool()
    {
        rangeEnemyPool = new ObjectPool<RangeEnemy>(rangeEnemyPrefab, MaxRangeEnemies);
    }

    private IEnumerator SpawnRangeEnemiesContinuously()
    {
        while (true)
        {
            if (activeRangeEnemies.Count < MaxRangeEnemies)
            {
                SpawnRangeEnemy();
            }
            yield return new WaitForSeconds(SpawnInterval);
        }
    }

    private void SpawnRangeEnemy()
    {
        if (rangeEnemyPool != null)
        {
            RangeEnemy rangeEnemy = rangeEnemyPool.Get();
            if (rangeEnemy != null)
            {
                // Generate random position within specified range
                float randomX = Random.Range(-17f, 0f);
                float randomY = Random.Range(10f, 0f);
                Vector3 randomPosition = new Vector3(randomX, randomY, 0);

                // Set enemy position
                rangeEnemy.transform.position = randomPosition;

                // Initialize enemy
                rangeEnemy.Initialize(rangeEnemyPool, GameObject.FindGameObjectWithTag("Player").transform);

                // Add to active enemies list
                activeRangeEnemies.Add(rangeEnemy);

                // Add a listener for when the enemy is disabled
                rangeEnemy.gameObject.SetActive(true);
                rangeEnemy.OnDisabled += () => activeRangeEnemies.Remove(rangeEnemy);
            }
        }
        else
        {
            Debug.LogError("RangeEnemy pool is not initialized.");
        }
    }
}
