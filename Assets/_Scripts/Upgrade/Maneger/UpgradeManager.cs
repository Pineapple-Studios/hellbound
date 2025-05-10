using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] GameObject enemy;

    private List<Upgrade> upgradesEspeciaisDisponiveis;
    private List<Upgrade> upgradesArriscadosDisponiveis;


    [Header("UI")]
    [SerializeField] GameObject upgradeMenu;
    [SerializeField] UpgradeSlot[] upgradeSlots;

    [Header("Controle")]
    public bool faseTerminou = true;
    public int rerollsDisponiveis = 1;

    [Header("Upgrades")]
    public List<Upgrade> upgradesNormais;
    public List<Upgrade> upgradesEspeciais;
    public List<Upgrade> upgradesArriscados;

    private List<Upgrade> upgradesAtuais = new List<Upgrade>();

    private void Start()
    {
        upgradesEspeciaisDisponiveis = new List<Upgrade>(upgradesEspeciais);
        upgradesArriscadosDisponiveis = new List<Upgrade>(upgradesArriscados);

        UiHandler(upgradeMenu, false);
        faseTerminou = false;
    }

    void Update()
    {
        if (!faseTerminou) return;

        Time.timeScale = 0f; // Pausa o jogo
        UiHandler(upgradeMenu, true);

        upgradesAtuais = GerarOpcoesUpgrades();

        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            upgradeSlots[i].SetUpgrade(upgradesAtuais[i], this, i);
        }

        faseTerminou = false; // Evita que continue rodando
    }

    public void EscolherUpgrade(int index)
    {
        Upgrade escolhido = upgradesAtuais[index];
        escolhido.Aplicar();

        // Remove se for especial ou arriscado
        if (upgradesEspeciaisDisponiveis.Contains(escolhido))
            upgradesEspeciaisDisponiveis.Remove(escolhido);
        else if (upgradesArriscadosDisponiveis.Contains(escolhido))
            upgradesArriscadosDisponiveis.Remove(escolhido);

        UiHandler(upgradeMenu, false);
        Time.timeScale = 1f;

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void RerollUpgrade(int index)
    {
        if (rerollsDisponiveis <= 0) return;

        rerollsDisponiveis--;

        var novasOpcoes = GerarOpcoesUpgrades(1, upgradesAtuais.Select(u => u).ToList());
        upgradesAtuais[index] = novasOpcoes[0];

        upgradeSlots[index].SetUpgrade(upgradesAtuais[index], this, index);
    }

    private List<Upgrade> GerarOpcoesUpgrades(int quantidade = 3, List<Upgrade> excluidos = null)
    {
        List<Upgrade> opcoes = new List<Upgrade>();
        excluidos ??= new List<Upgrade>();

        while (opcoes.Count < quantidade)
        {
            float chance = Random.value;
            Upgrade novo = null;

            if (chance <= 0.05f)
            {
                if (upgradesEspeciaisDisponiveis.Count > 0)
                    novo = PegarAleatorio(upgradesEspeciaisDisponiveis, opcoes, excluidos);
            }
            else if (chance <= 0.25f)
            {
                if (upgradesArriscadosDisponiveis.Count > 0)
                    novo = PegarAleatorio(upgradesArriscadosDisponiveis, opcoes, excluidos);
            }
            else
            {
                List<Upgrade> poolNormais = new List<Upgrade>(upgradesNormais);

                if (PlayerStats.Instance.bloquearUpgradeAumentaVida)
                {
                    // Remove especificamente o upgrade "Aumenta Vida"
                    poolNormais = poolNormais.Where(upg => upg.nome != "Life").ToList();
                }

                novo = PegarAleatorio(poolNormais, opcoes, excluidos);
            }


            if (novo != null)
                opcoes.Add(novo);
        }

        return opcoes;
    }


    private Upgrade PegarAleatorio(List<Upgrade> pool, List<Upgrade> existentes, List<Upgrade> excluidos)
    {
        var filtrados = pool.Except(existentes).Except(excluidos).ToList();
        if (filtrados.Count == 0) return null;

        int index = Random.Range(0, filtrados.Count);
        return filtrados[index];
    }

    public void UiHandler(GameObject ui, bool isActive)
    {
        ui.gameObject.SetActive(isActive);
    }
}
