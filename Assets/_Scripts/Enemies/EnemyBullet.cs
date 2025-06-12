using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float _damage;

    private void Start()
    {
        _damage = EnemyAi.instance.damageShoot;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats.Instance.ReceiveDamage(_damage);
            Destroy(gameObject);
        }
    }
}