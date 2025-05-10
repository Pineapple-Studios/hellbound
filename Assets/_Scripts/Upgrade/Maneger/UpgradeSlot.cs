using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeSlot : MonoBehaviour
{
    public Image icone;
    public TMP_Text nome;
    public Button escolherBtn;
    public Button rerollBtn;

    private int index;
    private UpgradeManager manager;

    public void SetUpgrade(Upgrade upgrade, UpgradeManager upgradeManager, int idx)
    {
        icone.sprite = upgrade.icone;
        nome.text = upgrade.nome;
        manager = upgradeManager;
        index = idx;

        escolherBtn.onClick.RemoveAllListeners();
        escolherBtn.onClick.AddListener(() => manager.EscolherUpgrade(index));

        rerollBtn.onClick.RemoveAllListeners();
        rerollBtn.onClick.AddListener(() => manager.RerollUpgrade(index));
    }
}
