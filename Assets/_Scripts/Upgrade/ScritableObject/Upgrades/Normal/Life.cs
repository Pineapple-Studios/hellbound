using UnityEngine;

[CreateAssetMenu(fileName = "Life", menuName = "Upgrades/Normais/Life")]
public class Life : Upgrade
{
    public float valor = 20f;

    public override void Aplicar()
    {
        PlayerStats.Instance.Health += valor;
    }
}
