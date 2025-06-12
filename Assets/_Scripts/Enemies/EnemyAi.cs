using System.Collections;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public static EnemyAi instance;

    [Header("Config")]
    [SerializeField] public EnemySO enemySO;

    [Header("Debug")]
    [SerializeField] private bool isRanged = true;

    [Header("Shoot (for ranged enemies)")]
    [SerializeField] private float timeToDestroy = 10;
    [SerializeField] private float cooldown = 1;
    [SerializeField] public float damageShoot = 10;

    private GameObject player;
    protected Transform trPlayer;

    protected SpriteRenderer spriteRenderer;
    protected float health;

    private float _distanceToPlayer;
    private bool _canShoot = true;
    private GameObject bulletParent;

    protected virtual void Start()
    {
        if (instance == null)
            instance = this;


        player = GameObject.Find("Player");
        trPlayer = player.transform;

        bulletParent = GameObject.Find("EnemyBullets");
        spriteRenderer = GetComponent<SpriteRenderer>();

        health = enemySO.healthMax;
        spriteRenderer.sprite = enemySO.sprite;
    }

    protected virtual void Update()
    {
        _distanceToPlayer = Vector2.Distance(transform.position, trPlayer.position);

        RotateTowardsPlayer();
        ChasePlayer(enemySO.stopDistance, enemySO.moveSpeed);

        if (isRanged)
        {
            OnShoot();
        }

        UpdateVisual();
    }

    protected void RotateTowardsPlayer()
    {
        Vector2 dir = trPlayer.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }

    protected void ChasePlayer(float stopDistance, float speed)
    {
        if (trPlayer == null) return;

        float dist = Vector2.Distance(transform.position, trPlayer.position);
        if (dist > stopDistance)
        {
            Vector2 direction = (trPlayer.position - transform.position).normalized;

            if (isRanged)
                direction.y = 0f;

            transform.position += (Vector3)(direction * speed * Time.deltaTime);
        }
    }

    protected void OnShoot()
    {
        if (_distanceToPlayer <= enemySO.distance && _canShoot)
        {
            StartCoroutine(ShootingCooldown());

            Vector2 direction = (trPlayer.position - transform.position).normalized;

            Vector2 firePos = (Vector2)transform.position + (Vector2)(transform.up * 0.8f);

            GameObject bullet = Instantiate(
                enemySO.bullet_prefab,
                firePos,
                Quaternion.identity,
                bulletParent.transform
            );

            bullet.GetComponent<Rigidbody2D>().AddForce(direction * 2f, ForceMode2D.Impulse);
            Destroy(bullet, timeToDestroy);
        }
    }

    IEnumerator ShootingCooldown()
    {
        _canShoot = false;
        yield return new WaitForSeconds(cooldown);
        _canShoot = true;
    }

    protected void UpdateVisual()
    {
        spriteRenderer.color = new Color(
            spriteRenderer.color.r,
            spriteRenderer.color.g,
            spriteRenderer.color.b,
            Mathf.Pow(health / enemySO.healthMax, 0.7f));

        if (health <= 0)
            Destroy(gameObject);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        //Debug.Log($"Health: {health}, Amount: {amount}");

        if (health <= 0)
        {
            if (PlayerStats.Instance.hasExplosion)
            {
                ExplosionEffect();
            }
            Destroy(gameObject);
        }
    }

    private void ExplosionEffect()
    {
        int projectiles = 8;
        float angleStep = 360f / projectiles;

        for (int i = 0; i < projectiles; i++)
        {
            float angle = i * angleStep;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject bullet = Instantiate(enemySO.bullet_prefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().AddForce(dir * 8f, ForceMode2D.Impulse);
            Destroy(bullet, 3f);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats.Instance.ReceiveDamage(damageShoot);
            Debug.Log("Inimigo causou dano ao player!");

            if (!isRanged)
            {
                Debug.Log("E1 foi destruído após encostar no player.");
                Destroy(gameObject);
            }
        }
    }
}
