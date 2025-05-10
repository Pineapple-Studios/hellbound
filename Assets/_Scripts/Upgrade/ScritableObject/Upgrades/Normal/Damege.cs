using UnityEngine;

[CreateAssetMenu(fileName = "Damege", menuName = "Upgrades/Normais/Damage")]
public class Damage : Upgrade
{
    public float valor = 5f;

    public override void Aplicar()
    {
        PlayerStats.Instance.damage += valor;
    }
}