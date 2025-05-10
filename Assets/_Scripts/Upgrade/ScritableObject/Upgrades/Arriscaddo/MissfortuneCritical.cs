using UnityEngine;

[CreateAssetMenu(fileName = "MissfortuneCritical", menuName = "Upgrades/Arriscados/MissfortuneCritical")]
public class MissfortuneCritical : Upgrade
{
    public override void Aplicar()
    {
        PlayerStats stats = PlayerStats.Instance;
        stats.critDamage = stats.maxCritDamage;
        stats.critChance = stats.minCritChance;
    }
}