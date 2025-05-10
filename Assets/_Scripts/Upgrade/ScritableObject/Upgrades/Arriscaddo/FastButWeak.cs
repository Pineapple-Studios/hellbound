using UnityEngine;

[CreateAssetMenu(fileName = "FastButWeak", menuName = "Upgrades/Arriscados/FastButWeak")]
public class FastButWeak : Upgrade
{
    public override void Aplicar()
    {
        PlayerStats stats = PlayerStats.Instance;
        stats.attackSpeed = stats.maxAttackSpeed;
        stats.damage = stats.minDamage;
    }
}