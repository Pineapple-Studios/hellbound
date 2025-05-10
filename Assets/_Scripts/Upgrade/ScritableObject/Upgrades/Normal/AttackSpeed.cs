using UnityEngine;

[CreateAssetMenu(fileName = "AttackSpeed", menuName = "Upgrades/Normais/AttackSpeed")]
public class AttackSpeed : Upgrade
{
    public float valor = 0.2f;

    public override void Aplicar()
    {
        PlayerStats.Instance.attackSpeed += valor;
    }
}