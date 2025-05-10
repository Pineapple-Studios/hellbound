using UnityEngine;

[CreateAssetMenu(fileName = "UndeadKnight", menuName = "Upgrades/Arriscados/UndeadKnight")]
public class UndeadKnight : Upgrade
{
    public override void Aplicar()
    {
        PlayerStats stats = PlayerStats.Instance;
        stats.armor = stats.maxArmor;
        stats.maxHealth = stats.minHealth;
    }
}
