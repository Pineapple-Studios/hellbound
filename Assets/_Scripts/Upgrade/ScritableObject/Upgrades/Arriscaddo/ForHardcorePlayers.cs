using UnityEngine;

[CreateAssetMenu(fileName = "ForHardcorePlayers", menuName = "Upgrades/Arriscados/HardcorePlayers")]
public class ForHardcorePlayers : Upgrade
{
    public override void Aplicar()
    {
        PlayerStats stats = PlayerStats.Instance;
        stats.attackSpeed = stats.maxAttackSpeed;
        stats.damage = stats.maxDamage;
        stats.moveSpeed = stats.maxMoveSpeed;
        stats.Health = stats.minHealth;

        stats.bloquearAumentoDeVida = true;

        stats.bloquearUpgradeAumentaVida = true;
    }
}