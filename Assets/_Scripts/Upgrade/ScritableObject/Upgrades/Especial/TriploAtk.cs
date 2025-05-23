using UnityEngine;

[CreateAssetMenu(fileName = "TriploAtk", menuName = "Upgrades/Especiais/TriploAtk")]
public class TriploAtk : Upgrade
{
    public override void Aplicar()
    {
        PlayerStats.Instance.projectilesPerShot = 3;
    }
}