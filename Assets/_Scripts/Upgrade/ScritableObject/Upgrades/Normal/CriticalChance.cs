using UnityEngine;

[CreateAssetMenu(fileName = "CriticalChance", menuName = "Upgrades/Normais/CriticalChance")]
public class CriticalChance : Upgrade
{
    public float valorPercentual = 5f;

    public override void Aplicar()
    {
        PlayerStats.Instance.critChance += valorPercentual;
    }
}
