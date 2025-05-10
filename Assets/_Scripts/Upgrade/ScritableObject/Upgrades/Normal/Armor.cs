    using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "Upgrades/Normais/Armor")]
public class Armor : Upgrade
{
    public float valor = 3f;

    public override void Aplicar()
    {
        PlayerStats.Instance.armor += valor;
    }
}
