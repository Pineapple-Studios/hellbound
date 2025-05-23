using UnityEngine;

[CreateAssetMenu(fileName = "DuploAtk", menuName = "Upgrades/Especiais/DuploAtk")]
public class DuploAtk : Upgrade
{
    public override void Aplicar()
    {
        PlayerStats.Instance.projectilesPerShot = Mathf.Max(PlayerStats.Instance.projectilesPerShot, 2);
    }
}
