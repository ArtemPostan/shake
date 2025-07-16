using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurviveLevel : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Wave Settings")]
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private int enemiesPerWaveStart = 3;
    [SerializeField] private int enemyIncreasePerWave = 2;

    private int currentWave = 0;
    private int enemiesAlive = 0;
    private int enemiesKilledTotal = 0;

    private void Start()
    {
        StartCoroutine(SpawnWaveRoutine());
    }

    private IEnumerator SpawnWaveRoutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => enemiesAlive == 0);
            yield return new WaitForSeconds(timeBetweenWaves);

            currentWave++;
            int enemiesThisWave = enemiesPerWaveStart + enemyIncreasePerWave * (currentWave - 1);
            SpawnEnemies(enemiesThisWave);
        }
    }

    private void SpawnEnemies(int count)
    {
        if (spawnPoints.Length == 0 || enemyPrefab == null)
        {
            Debug.LogWarning("No spawn points or enemy prefab assigned.");
            return;
        }

        enemiesAlive = count;

        for (int i = 0; i < count; i++)
        {
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(enemyPrefab, point.position, Quaternion.identity);

            //EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            //if (health != null)
            //    health.OnDeath += OnEnemyKilled;
        }

        Debug.Log($"Wave {currentWave} spawned with {count} enemies.");
    }

    private void OnEnemyKilled()
    {
        enemiesAlive--;
        enemiesKilledTotal++;

        Debug.Log($"Enemy killed. Remaining: {enemiesAlive}. Total killed: {enemiesKilledTotal}");
    }
}
