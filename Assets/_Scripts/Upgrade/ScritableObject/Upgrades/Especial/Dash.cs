using UnityEngine;

[CreateAssetMenu(fileName = "Dash", menuName = "Upgrades/Especiais/Dash")]
public class Dash : Upgrade
{
    public override void Aplicar()
    {
        PlayerStats.Instance.hasDash = true;
    }
}