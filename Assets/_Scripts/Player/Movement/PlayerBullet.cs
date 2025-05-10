using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBullet : MonoBehaviour
{
    public static PlayerBullet Instance;

    [Header("Input")]
    [SerializeField] private InputActionAsset inputActions;

    [Space(10)]
    [Header("Bullet")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject bulletOrigin;
    [SerializeField] Vector2 bulletForce = new Vector2(2, 0);
    [SerializeField] float timeToDestroy = 10;

    [Space(10)]
    [Header("Cooldown Shoot")]
    [SerializeField] float cooldown = 1;

    private InputAction _shootAction;
    private bool _canShoot = true;


    private void Awake()
    {
        if (Instance == null) Instance = this;

        var playerMap = inputActions.FindActionMap("Player");
        _shootAction = playerMap.FindAction("Shoot");
    }

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
        _shootAction.performed += OnShoot;
        _shootAction.canceled += OnShoot;
    }

    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
        _shootAction.performed -= OnShoot;
        _shootAction.canceled -= OnShoot;
    }

    void OnShoot(InputAction.CallbackContext context)
    {
        if (_canShoot)
        {
            StartCoroutine(ShootingCooldown());

            GameObject bullet = Instantiate(bulletPrefab, bulletOrigin.transform);

            bullet.GetComponent<Rigidbody2D>().AddForce(bulletForce, ForceMode2D.Impulse);

            Debug.DrawRay(bulletOrigin.transform.position, bulletOrigin.transform.right * 2, Color.red, 2f);
            
            Destroy(bullet, timeToDestroy);

        }
    }

    IEnumerator ShootingCooldown()
    {
        _canShoot = false;
        yield return new WaitForSeconds(cooldown);
        _canShoot = true;
    }
}
