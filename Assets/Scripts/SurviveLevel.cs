using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurviveLevel : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject enemyPrefab;

    [Tooltip("Список зон, где могут появляться враги")]
    [SerializeField] private List<Transform> spawnAreas = new List<Transform>();

    [Tooltip("Размер одной зоны (ширина, длина)")]
    [SerializeField] private Vector2 areaSize = new Vector2(20f, 20f);

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
        if (spawnAreas.Count == 0 || enemyPrefab == null)
        {
            Debug.LogWarning("No spawn areas or enemy prefab assigned.");
            return;
        }

        enemiesAlive = count;

        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = GetRandomSpawnPosition();
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            Combat combat = enemy.GetComponent<Combat>();
            if (combat != null)
                combat.OnDie += OnEnemyKilled;
        }

        Debug.Log($"Wave {currentWave} spawned with {count} enemies.");
        UpdateUI(currentWave, enemiesAlive, enemiesKilledTotal);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Transform selectedArea = spawnAreas[Random.Range(0, spawnAreas.Count)];

        float halfX = areaSize.x / 2f;
        float halfZ = areaSize.y / 2f;

        float offsetX = Random.Range(-halfX, halfX);
        float offsetZ = Random.Range(-halfZ, halfZ);

        Vector3 spawnPos = selectedArea.position + new Vector3(offsetX, 0f, offsetZ);

        // (опционально) подгонка по земле:
        // RaycastHit hit;
        // if (Physics.Raycast(spawnPos + Vector3.up * 10f, Vector3.down, out hit, 20f))
        //     spawnPos.y = hit.point.y;

        return spawnPos;
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

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);

        foreach (var area in spawnAreas)
        {
            if (area == null) continue;
            Gizmos.DrawCube(area.position, new Vector3(areaSize.x, 0.1f, areaSize.y));
        }
    }
#endif
}
