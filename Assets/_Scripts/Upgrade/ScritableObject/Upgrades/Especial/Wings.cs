using UnityEngine;

[CreateAssetMenu(fileName = "Wings", menuName = "Upgrades/Especiais/Wings")]
public class Wings : Upgrade
{
    public override void Aplicar()
    {
        PlayerStats.Instance.hasWings = true;
    }
}
