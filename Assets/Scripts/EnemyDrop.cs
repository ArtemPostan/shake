using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject diamondPrefab;
    public GameObject healthPrefab;
    public GameObject magnetPrefab;
    public GameObject bombPrefab;

    [Header("Drop Chances (0 to 1)")]
    [Range(0f, 1f)] public float healthChance = 0.1f;
    [Range(0f, 1f)] public float magnetChance = 0.15f;
    [Range(0f, 1f)] public float bombChance = 0.05f;

    [Header("Drop Settings")]
    public Vector3 dropOffset = Vector3.up * 0.5f;
    public float scatterForce = 3f;

    public void DropItems()
    {
        Vector3 dropPosition = transform.position + dropOffset;

        // Алмаз (всегда
        SpawnItemWithForce(diamondPrefab, dropPosition);

        // Остальные — с шансом
        if (Random.value < healthChance)
            SpawnItemWithForce(healthPrefab, dropPosition);

        if (Random.value < magnetChance)
            SpawnItemWithForce(magnetPrefab, dropPosition);

        if (Random.value < bombChance)
            SpawnItemWithForce(bombPrefab, dropPosition);
    }

    private void SpawnItemWithForce(GameObject prefab, Vector3 position)
    {
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);

        if (obj.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            Vector3 randomDirection = (Vector3.up + Random.insideUnitSphere).normalized;
            rb.AddForce(randomDirection * scatterForce, ForceMode.Impulse);
        }
    }
}
