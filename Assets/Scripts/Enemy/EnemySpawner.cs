using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnRate = 0.1f;
    [SerializeField] private float spawnDistance = 5f;
    [SerializeField] private float minSpawnDistance = 2f;
    
    public delegate void CallVictoryEvent();

    public static event CallVictoryEvent OnVictory;
    
    private float spawnTimer;
    private int _bossCount = 0;
    

    //[SerializeField] private float maxHealth = 100.0f;

    // Update is called once per frame
    void Update()
    {
        SpawnEnemy();
    }
    
    private void OnEnable()
    {
        DayNightManager.OnDayCycleEnd += HandleDayCycleEnd;
        Boss.OnBossKilled += HandleOnBossKilled;
    }

    private void OnDisable()
    {
        DayNightManager.OnDayCycleEnd -= HandleDayCycleEnd;
        Boss.OnBossKilled -= HandleOnBossKilled;
    }

    private void HandleDayCycleEnd(int daysSurvived)
    {
        StartCoroutine(SpawnBoss(daysSurvived));
        // Spawns faster per day
        if (spawnRate > 0.1f)
        {
            spawnRate -= 0.1f;
        }
    }
    
    private void HandleOnBossKilled()
    {
        _bossCount -= 1;

        if (_bossCount <= 0)
        {
            _bossCount = 0;
            OnVictory?.Invoke();
        }
    }
    
    IEnumerator SpawnBoss(int daysSurvived)
    {
        yield return new WaitForSeconds(7);
        if (daysSurvived % 2 == 0)
        {
            Vector2 spawnPosition = GetValidSpawnPosition();
            Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
            _bossCount += 1;
        }

        // Every 4 days, spawn bosses on 2 day intervals
        if (daysSurvived % 4 == 0)
        {
            // Calculate the number of bosses to spawn based on the current 4-day interval
            int bossesToSpawn = daysSurvived / 4;

            // Spawn the calculated number of bosses
            for (int i = 0; i < bossesToSpawn; i++)
            {
                Vector2 spawnPosition = GetValidSpawnPosition();
                Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
                _bossCount += 1;
            }
        }
    }

    private void SpawnEnemy()
    {
        if (Time.time > spawnTimer && player != null)
        {
            // Attempt to spawn enemy with a valid position
            Vector2 spawnPosition = GetValidSpawnPosition();

            if (enemyPrefabs != null && enemyPrefabs.Length > 0)
            {
                // Create a random index within the array bounds
                int randomIndex = Random.Range(0, enemyPrefabs.Length); 
                // Select a random enemy prefab
                GameObject randomEnemyPrefab = enemyPrefabs[randomIndex]; 

                GameObject instantiatedEnemy = Instantiate(randomEnemyPrefab, spawnPosition, Quaternion.identity);
                spawnTimer = Time.time + spawnRate;

                //Enemy enemy = instantiatedEnemy.GetComponent<Enemy>();
                //maxHealth += 10.0f;
                //enemy.instantiateMaxHealth(maxHealth);
            }
        }
    }

    private Vector2 GetValidSpawnPosition()
    {
        Vector2 spawnPosition = Vector2.zero;
        float distanceToPlayer = 0f;

        do
        {
            // Create a random spawn position somewhere around the player
            spawnPosition = new Vector2(
                player.position.x + Random.Range(-spawnDistance, spawnDistance),
                player.position.y + Random.Range(-spawnDistance, spawnDistance));

            // Get the difference between the spawnPosition and the player position
            distanceToPlayer = Vector2.Distance(spawnPosition, player.position);

        // Keep attempting to have a spawn position that is not near the player
        } while (distanceToPlayer < minSpawnDistance);

        return spawnPosition;
    }
}
