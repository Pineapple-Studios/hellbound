using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Atributos Atuais")]
    [Header("")]

    public bool bloquearAumentoDeVida = false;

    [SerializeField] private float _attackSpeed = 1f;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _Health = 100f;
    [SerializeField] private float _damageReduction = 0f;
    [SerializeField] private float _critChance = 0f;
    [SerializeField] private float _critDamage = 1.5f;
    [SerializeField] private float _armor = 0f;

    [Header("Limites dos Atributos")]
    [Header("")]

    public float minAttackSpeed = 0.1f;
    public float maxAttackSpeed = 10f;

    [Header("")]

    public float minDamage = 1f;
    public float maxDamage = 999f;

    [Header("")]

    public float minMoveSpeed = 0.5f;
    public float maxMoveSpeed = 20f;

    [Header("")]

    public float minHealth = 10f;
    public float maxHealth = 1000f;

    [Header("")]

    public float minDamageReduction = 0f;
    public float maxDamageReduction = 90f;

    [Header("")]

    public float minCritChance = 0f;
    public float maxCritChance = 100f;

    [Header("")]

    public float minCritDamage = 1f;
    public float maxCritDamage = 5f;

    [Header("")]

    public float minArmor = 0f;
    public float maxArmor = 500f;

    public float attackSpeed
    {
        get => _attackSpeed;
        set => _attackSpeed = Mathf.Clamp(value, minAttackSpeed, maxAttackSpeed);
    }

    public float damage
    {
        get => _damage;
        set => _damage = Mathf.Clamp(value, minDamage, maxDamage);
    }

    public float moveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = Mathf.Clamp(value, minMoveSpeed, maxMoveSpeed);
    }

    public float Health
    {
        get => _Health;
        set
        {
            if (bloquearAumentoDeVida) return;
            _Health = Mathf.Clamp(value, minHealth, maxHealth);
        }

    }

    public float damageReduction
    {
        get => _damageReduction;
        set => _damageReduction = Mathf.Clamp(value, minDamageReduction, maxDamageReduction);
    }

    public float critChance
    {
        get => _critChance;
        set => _critChance = Mathf.Clamp(value, minCritChance, maxCritChance);
    }

    public float critDamage
    {
        get => _critDamage;
        set => _critDamage = Mathf.Clamp(value, minCritDamage, maxCritDamage);
    }

    public float armor
    {
        get => _armor;
        set => _armor = Mathf.Clamp(value, minArmor, maxArmor);
    }

    void Awake()
    {
        Instance = this;
    }
}


