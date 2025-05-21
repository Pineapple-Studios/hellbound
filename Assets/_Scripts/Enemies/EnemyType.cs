using UnityEngine;

[System.Serializable]
public class EnemyType
{
    public string name;
    public GameObject prefab;
    public EnemySO config;
    [Range(0, 1)] public float spawnChance = 0.5f;
}