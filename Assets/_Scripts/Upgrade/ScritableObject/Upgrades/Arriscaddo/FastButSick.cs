using UnityEngine;

[CreateAssetMenu(fileName = "FastButSick", menuName = "Upgrades/Arriscados/FastButSick")]
public class FastButSick : Upgrade
{
    public override void Aplicar()
    {
        PlayerStats stats = PlayerStats.Instance;
        stats.moveSpeed = stats.maxMoveSpeed;
        stats.maxHealth = stats.minHealth;
    }
}
