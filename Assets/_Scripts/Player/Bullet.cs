using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;

    private void Start()
    {
        Destroy(gameObject, 2.5f);
    }

    public void SetDamage(float value)
    {
        damage = value;
    }

    public float GetDamage()
    {
        return damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent(out EnemyAi enemy))
            {
                float finalDamage = damage;

                float critRoll = Random.Range(0f, 100f);
                if (critRoll < PlayerStats.Instance.critChance)
                {
                    finalDamage *= PlayerStats.Instance.critDamage;
                    Debug.Log($"Dano crítico aplicado: {finalDamage}");
                }

                enemy.TakeDamage(finalDamage);
            }

            bool hasGhost = PlayerStats.Instance.hasGhostShoot;
            bool hasReflect = PlayerStats.Instance.hasReflectedShoot;

            if (hasReflect)
            {
                Vector2 reflectDir = Vector2.Reflect(
                    GetComponent<Rigidbody2D>().linearVelocity.normalized,
                    (collision.transform.position - transform.position).normalized
                );

                GetComponent<Rigidbody2D>().linearVelocity = reflectDir * GetComponent<Rigidbody2D>().linearVelocity.magnitude;
                return;
            }

            if (hasGhost) return;

            Destroy(gameObject);
        }
    }
}

