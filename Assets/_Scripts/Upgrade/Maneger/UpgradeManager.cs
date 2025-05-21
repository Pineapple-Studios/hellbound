using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [SerializeField] GameObject enemy;

    private List<Upgrade> availableSpecialUpgrades;
    private List<Upgrade> availableRiskyUpgrades;


    [Header("UI")]
    [SerializeField] GameObject upgradeMenu;
    [SerializeField] UpgradeSlot[] upgradeSlots;

    [Header("Controle")]
    public bool waveEnded = true;
    public int availableRerolls = 1;

    [Header("Upgrades")]
    public List<Upgrade> normalUpgrades;
    public List<Upgrade> specialUpgrades;
    public List<Upgrade> riskyUpgrades;

    private List<Upgrade> currentUpgrades = new List<Upgrade>();

    private bool upgradesDisplayed;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        availableSpecialUpgrades = new List<Upgrade>(specialUpgrades);
        availableRiskyUpgrades = new List<Upgrade>(riskyUpgrades);

        UiHandler(upgradeMenu, false);
        waveEnded = false;
    }

    void Update()
    {
        if (waveEnded && !upgradesDisplayed)
        {
            upgradesDisplayed = true;
            StartCoroutine(OpenUpgradeMenu());
        }
    }

    public void SelectUpgrade(int index)
    {
        Upgrade chosen = currentUpgrades[index];
        chosen.Aplicar();

        // Remove se for especial ou arriscado
        if (availableSpecialUpgrades.Contains(chosen))
            availableSpecialUpgrades.Remove(chosen);
        else if (availableRiskyUpgrades.Contains(chosen))
            availableRiskyUpgrades.Remove(chosen);

        UiHandler(upgradeMenu, false);
        Time.timeScale = 1f;

        upgradesDisplayed = false;
        waveEnded = false;

        GameManager.Instance.StartWave(GameManager.Instance.currentWaveIndex);

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void RerollUpgrade(int index)
    {
        if (availableRerolls <= 0) return;

        availableRerolls--;

        var novasOpcoes = GenerateUpgradeOptions(1, currentUpgrades.Select(u => u).ToList());
        currentUpgrades[index] = novasOpcoes[0];

        upgradeSlots[index].SetUpgrade(currentUpgrades[index], this, index);
    }

    private List<Upgrade> GenerateUpgradeOptions(int quantity = 3, List<Upgrade> excluded = null)
    {
        List<Upgrade> options = new List<Upgrade>();
        excluded ??= new List<Upgrade>();

        while (options.Count < quantity)
        {
            float chance = Random.value;
            Upgrade @new = null;

            if (chance <= 0.05f)
            {
                if (availableSpecialUpgrades.Count > 0)
                    @new = PickRandomUpgrade(availableSpecialUpgrades, options, excluded);
            }
            else if (chance <= 0.25f)
            {
                if (availableRiskyUpgrades.Count > 0)
                    @new = PickRandomUpgrade(availableRiskyUpgrades, options, excluded);
            }
            else
            {
                List<Upgrade> normalPool = new List<Upgrade>(normalUpgrades);

                if (PlayerStats.Instance.bloquearUpgradeAumentaVida)
                {
                    // Remove especificamente o upgrade "Aumenta Vida"
                    normalPool = normalPool.Where(upg => upg.nome != "Life").ToList();
                }

                @new = PickRandomUpgrade(normalPool, options, excluded);
            }


            if (@new != null)
                options.Add(@new);
        }

        return options;
    }


    private Upgrade PickRandomUpgrade(List<Upgrade> pool, List<Upgrade> existing, List<Upgrade> excluded)
    {
        var filtered = pool.Except(existing).Except(excluded).ToList();
        if (filtered.Count == 0) return null;

        int index = Random.Range(0, filtered.Count);
        return filtered[index];
    }

    public void UiHandler(GameObject ui, bool isActive)
    {
        ui.gameObject.SetActive(isActive);
    }

    IEnumerator OpenUpgradeMenu()
    {
        yield return new WaitForEndOfFrame(); // espera UI inicializar

        Time.timeScale = 0f;
        UiHandler(upgradeMenu, true);

        currentUpgrades = GenerateUpgradeOptions();

        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            upgradeSlots[i].SetUpgrade(currentUpgrades[i], this, i); // define visual e dados
        }
    }
}
