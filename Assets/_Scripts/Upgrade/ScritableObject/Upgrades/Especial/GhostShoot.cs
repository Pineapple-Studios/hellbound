using UnityEngine;

[CreateAssetMenu(fileName = "GhostShoot", menuName = "Upgrades/Especiais/GhostShoot")]
public class GhostShoot : Upgrade
{
    public override void Aplicar()
    {
        PlayerStats.Instance.hasGhostShoot = true;
    }
}
