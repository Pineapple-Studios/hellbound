using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private UpgradeManager upgradeManager;
    public GameObject thisobj;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projetil"))
        {
            upgradeManager.UiHandler(thisobj, false);

            if (upgradeManager != null)
            {
                upgradeManager.waveEnded = true;
            }
        }
    }
}
