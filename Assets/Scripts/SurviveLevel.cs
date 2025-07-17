using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SurviveLevel : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Wave Settings")]
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private int enemiesPerWaveStart = 3;
    [SerializeField] private int enemyIncreasePerWave = 2;

    public int currentWave = 0;
    public int enemiesAlive = 0;
    public int enemiesKilledTotal = 0;

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

            Combat combat = enemy.GetComponent<Combat>();
            if (combat != null)
                combat.OnDie += OnEnemyKilled;
        }

        Debug.Log($"Wave {currentWave} spawned with {count} enemies.");
        UpdateUI(currentWave, enemiesAlive, enemiesKilledTotal);
    }

    private void OnEnemyKilled(Combat deadEnemy)
    {
        enemiesAlive--;
        enemiesKilledTotal++;
        UpdateUI(currentWave, enemiesAlive, enemiesKilledTotal);
        Debug.Log($"Enemy killed. Remaining: {enemiesAlive}. Total killed: {enemiesKilledTotal}");
    }

    private void UpdateUI(int wave, int alive, int killed)
    {
        GameManager.Instance.UIManager.UpdateCount(wave, alive, killed);
    }
}
