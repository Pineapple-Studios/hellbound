using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyDistance : MonoBehaviour
{
    [Header("Separation Settings")]
    [SerializeField] private float radius;
    [SerializeField] private float forceStrength;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private bool drawGizmos = false;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Collider2D[] neighbors = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);

        Vector2 separation = Vector2.zero;
        int count = 0;

        foreach (var col in neighbors)
        {
            if (col.gameObject == gameObject) continue;

            Vector2 diff = (Vector2)(transform.position - col.transform.position);
            float dist = diff.magnitude;

            if (dist > 0)
            {
                separation += diff.normalized / dist;
                count++;
            }
        }

        if (count > 0)
        {
            Vector2 repulsion = separation * forceStrength;
            rb.AddForce(repulsion, ForceMode2D.Force);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}

