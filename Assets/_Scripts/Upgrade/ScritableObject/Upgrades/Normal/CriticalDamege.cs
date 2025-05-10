using UnityEngine;

[CreateAssetMenu(fileName = "CriticalDamege", menuName = "Upgrades/Normais/CriticalDamage")]
public class CriticalDamage : Upgrade
{
    public float valorMultiplicador = 0.25f;

    public override void Aplicar()
    {
        PlayerStats.Instance.critDamage += valorMultiplicador;
    }
}
