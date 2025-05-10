using UnityEngine;

[CreateAssetMenu(fileName = "DamegeReduction", menuName = "Upgrades/Normais/DamegeReduction")]

public class DamegeReduction : Upgrade
{
    public float valorPercentual = 5f;

    public override void Aplicar()
    {
        PlayerStats.Instance.damageReduction += valorPercentual;
    }
}