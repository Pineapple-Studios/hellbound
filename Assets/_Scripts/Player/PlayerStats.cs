using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Death UI Maneger")]
    [SerializeField] private GameObject deathScreen;  // tela de derrota
    [SerializeField] private TMPro.TMP_Text waveText; // texto para mostrar wave
    [SerializeField] private Button menuButton; // botão para menu

    [Header("Atributos Atuais")]
    [SerializeField] private float _attackSpeed = 1f;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _Health = 100f;
    [SerializeField] private float _damageReduction = 0f;
    [SerializeField] private float _critChance = 0f;
    [SerializeField] private float _critDamage = 1.5f;
    [SerializeField] private float _armor = 0f;

    [Space(10)]
    [Header("Limites dos Atributos")]
    public float minAttackSpeed = 0.1f;
    public float maxAttackSpeed = 10f;

    public float minDamage = 1f;
    public float maxDamage = 999f;

    public float minMoveSpeed = 0.5f;
    public float maxMoveSpeed = 20f;

    public float minHealth = 10f;
    public float maxHealth = 1000f;

    public float minDamageReduction = 0f;
    public float maxDamageReduction = 90f;

    public float minCritChance = 0f;
    public float maxCritChance = 100f;

    public float minCritDamage = 1f;
    public float maxCritDamage = 5f;

    public float minArmor = 0f;
    public float maxArmor = 500f;

    // ➡️ Flags para upgrades especiais:
    [Header("Upgrades Especiais")]
    public bool hasGhostShoot = false;
    public bool hasReflectedShoot = false;
    public bool hasRevive = false;
    public bool hasInvulnerability = false;
    public bool hasDivineShield = false;
    public bool hasDash = false;
    public bool hasWings = false;
    public bool hasExplosion = false;

    [Header("Status temporários")]
    public bool shieldAvailable = false;   // Divine Shield
    public bool isInvulnerable = false;    // Invulnerability
    public bool canDash = true;            // Dash cooldown
    public bool hasUsedRevive = false;     // Revive controle

    [Header("Ataque especial")]
    public int projectilesPerShot = 1;

    [Header("Configuração de Invulnerabilidade")]
    [SerializeField] private float invulnerabilityDuration = 3f; // tempo padrão de 3 segundos

    public bool bloquearUpgradeAumentaVida = false;
    public bool bloquearAumentoDeVida = false;

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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        deathScreen.SetActive(false);
    }

    public void ReceiveDamage(float amount)
    {
        if (hasInvulnerability || isInvulnerable) return;

        if (hasDivineShield && shieldAvailable)
        {
            shieldAvailable = false;
            Debug.Log("Divine Shield bloqueou o dano!");
            return;
        }

        float reductionPercent = Mathf.Clamp(damageReduction, 0f, 100f);
        float remainingDamage = amount * (1f - reductionPercent / 100f);


        if (armor > 0)
        {
            if (armor >= remainingDamage)
            {
                armor -= remainingDamage;
                Debug.Log($"Armadura absorveu todo o dano. Armadura restante: {armor}");
                return; // todo dano foi absorvido, não mexe na vida
            }
            else
            {
                remainingDamage -= armor;
                Debug.Log($"Armadura absorveu {armor} de dano.");
                armor = 0;
            }
        }

        // Aplica dano restante à vida
        Health -= remainingDamage;
        Debug.Log($"Vida perdeu {remainingDamage}. Vida restante: {Health}");

        if (Health <= 0)
        {
            if (hasRevive && !hasUsedRevive)
            {
                hasUsedRevive = true;
                Health = 1f;
                Debug.Log("Revive ativado!");
            }
            else
            {
                Die();
            }
        }
    }



    private void Die()
    {
        Debug.Log("Player morreu!");

        Time.timeScale = 0f;

        if (deathScreen != null)
        {
            deathScreen.SetActive(true);

            if (waveText != null)
                waveText.text = $"Você perdeu na fase {GameManager.Instance.currentWaveIndex + 1}";

            if (menuButton != null)
                menuButton.onClick.RemoveAllListeners();
            menuButton.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("Menu"));
        }
    }

    public void ActivateInvulnerability()
    {
        if (!hasInvulnerability) return;

        StartCoroutine(InvulnerabilityRoutine(invulnerabilityDuration));
    }

    private IEnumerator InvulnerabilityRoutine(float duration)
    {
        isInvulnerable = true;
        Debug.Log("Invulnerável!");

        yield return new WaitForSeconds(duration);

        isInvulnerable = false;
        Debug.Log("Invulnerabilidade acabou.");
    }

}
