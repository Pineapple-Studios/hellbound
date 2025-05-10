using UnityEngine;

[CreateAssetMenu(fileName = "MovSpeed", menuName = "Upgrades/Normais/MovSpeed")]
public class MovSpeed : Upgrade
{
    public float valor = 1f;

    public override void Aplicar()
    {
        PlayerStats.Instance.moveSpeed += valor;
    }
}
