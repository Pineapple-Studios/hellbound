using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public static EnemySpawn Instance { get; private set; }

    [Header("Spawn Config")]
    [SerializeField] private float spawnRangeX = 8f;
    [SerializeField] private float spawnY = 6f;

    [Header("Enemy Types")]
    [SerializeField] private EnemyType[] enemyTypes;

    [SerializeField] private Transform enemyParent;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnRandomEnemy()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(-spawnRangeX, spawnRangeX),
            spawnY,
            0f
        );

        EnemyType selected = PickEnemyByChance();
        if (selected == null)
        {
            Debug.LogWarning("No enemy type selected. Check spawn chances.");
            return;
        }

        GameObject enemy = Instantiate(selected.prefab, spawnPos, Quaternion.identity, enemyParent);

        var ai = enemy.GetComponent<EnemyAi>();
        if (ai != null)
        {
            ai.enemySO = selected.config;
        }
        else
        {
            Debug.LogWarning("Spawned enemy missing EnemyAi component!");
        }
    }

    private EnemyType PickEnemyByChance()
    {
        float total = 0f;
        foreach (var e in enemyTypes)
            total += e.spawnChance;

        float rand = Random.value * total;

        float cumulative = 0f;
        foreach (var e in enemyTypes)
        {
            cumulative += e.spawnChance;
            if (rand <= cumulative)
                return e;
        }

        return null;
    }
}
