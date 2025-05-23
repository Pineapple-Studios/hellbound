using UnityEngine;

[CreateAssetMenu(fileName = "ReflectedShoot", menuName = "Upgrades/Especiais/ReflectedShoot")]
public class ReflectedShoot : Upgrade
{
    public override void Aplicar()
    {
        PlayerStats.Instance.hasReflectedShoot = true;
    }
}
