using UnityEngine;

[CreateAssetMenu(fileName = "Revive", menuName = "Upgrades/Especiais/Revive")]
public class Revive : Upgrade
{
    public override void Aplicar()
    {
        PlayerStats.Instance.hasRevive = true;
    }
}
