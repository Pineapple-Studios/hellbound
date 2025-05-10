using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] GameObject enemy;

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
        upgradesAtuais[index].Aplicar();
        UiHandler(upgradeMenu, false);
        Time.timeScale = 1f;

        // Troca de cena (carrega próxima fase)
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        UiHandler(enemy, true);
        UiHandler(upgradeMenu, false);
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
            if (chance <= 0.05f) novo = PegarAleatorio(upgradesEspeciais, opcoes, excluidos);
            else if (chance <= 0.25f) novo = PegarAleatorio(upgradesArriscados, opcoes, excluidos);
            else novo = PegarAleatorio(upgradesNormais, opcoes, excluidos);

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
