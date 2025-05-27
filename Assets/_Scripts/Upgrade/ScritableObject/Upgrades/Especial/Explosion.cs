using UnityEngine;

[CreateAssetMenu(fileName = "Explosion", menuName = "Upgrades/Especiais/Explosion")]
public class Explosion : Upgrade
{
    public override void Aplicar()
    {
        PlayerStats.Instance.hasExplosion = true;
    }
}