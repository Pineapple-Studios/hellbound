using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Wave Config")]
    [SerializeField] private List<WaveSettings> waveConfigs;

    [Header("Dependencies")]
    [SerializeField] private EnemySpawn spawner;

    [Header("Debug")]
    [SerializeField] public int currentWaveIndex = 0;
    [SerializeField] private float phaseTimer = 0f;

    private WaveSettings currentWave;
    private int enemiesSpawned = 0;
    private bool isSpawning = true;

    private GameObject enemyParent;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        enemyParent = GameObject.Find("Enemies");
    }

    void Start()
    {
        StartWave(currentWaveIndex);
    }

    void Update()
    {
        phaseTimer += Time.deltaTime;

        if (!isSpawning && enemyParent.transform.childCount == 0)
        {
            AdvanceWave();
        }
    }

    public void StartWave(int index)
    {
        currentWaveIndex = index;
        currentWave = waveConfigs[Mathf.Clamp(index, 0, waveConfigs.Count - 1)];
        phaseTimer = 0f;
        enemiesSpawned = 0;
        isSpawning = true;

        for (int i = 0; i < currentWave.minEnemies; i++)
        {
            spawner.SpawnRandomEnemy();
            enemiesSpawned++;
        }

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (enemiesSpawned < currentWave.maxEnemies)
        {
            yield return new WaitForSeconds(currentWave.spawnInterval);

            spawner.SpawnRandomEnemy();
            enemiesSpawned++;
        }

        isSpawning = false;
    }

    public void AdvanceWave()
    {
        Debug.Log("WAVE COMPLETA!");

        currentWaveIndex++;

        Time.timeScale = 0f;
        UpgradeManager.Instance.waveEnded = true;
    }
}
