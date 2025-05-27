using UnityEngine;

[CreateAssetMenu(fileName = "DivineShield", menuName = "Upgrades/Especiais/DivineShield")]
public class DivineShield : Upgrade
{
    public override void Aplicar()
    {
        PlayerStats.Instance.hasDivineShield = true;
    }
}
