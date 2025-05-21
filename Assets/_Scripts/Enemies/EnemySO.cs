using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemies/EnemySO")]
public class EnemySO : ScriptableObject
{
    [Header("Prefab")]
    public GameObject bullet_prefab;

    [Space(5)]
    [Header("Sprite")]
    public Sprite sprite;

    [Space(5)]
    [Header("Floats")]
    public float height;
    public float distance;
    public float healthMax;
    public float cooldown;

    [Space(5)]
    [Header("Movimentação")]
    public float moveSpeed = 2f;
    public float stopDistance = 1f;
}



