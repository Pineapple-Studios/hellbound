using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBullet : MonoBehaviour
{
    public static PlayerBullet Instance;

    [Header("DEBUG")]
    [SerializeField] bool canDebug = false;

    [Space(10)]
    [Header("Input")]
    [SerializeField] private InputActionAsset inputActions;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private float bulletSpeed = 15f;
    [SerializeField] private float timeToDestroy = 10f;

    private InputAction shootAction;
    private InputAction lookAction;
    private Camera mainCam;
    private bool _canShoot = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        var playerMap = inputActions.FindActionMap("Player");
        shootAction = playerMap.FindAction("Shoot");
        lookAction = playerMap.FindAction("Look");

        mainCam = Camera.main;
    }

    private void OnEnable()
    {
        shootAction.performed += OnShoot;
        inputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        shootAction.performed -= OnShoot;
        inputActions.FindActionMap("Player").Disable();
    }

    private void Update()
    {
        AimTowardsMouseOrStick();
    }

    void AimTowardsMouseOrStick()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        Vector2 aimDirection;

        if (Mouse.current != null && lookInput == Vector2.zero)
        {
            Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
            // Ajuste o z para a dist�ncia correta da c�mera:
            mouseScreenPos.z = Mathf.Abs(mainCam.transform.position.z);
            Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(mouseScreenPos);
            aimDirection = (mouseWorldPos - bulletOrigin.position);
        }
        else
        {
            aimDirection = lookInput;
        }

        if (aimDirection.sqrMagnitude > 0.1f)
        {
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            bulletOrigin.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (!_canShoot) return;

        StartCoroutine(ShootingCooldown());

        Vector2 aimDirection = GetAimDirection();
        if (aimDirection == Vector2.zero) return;

        Quaternion bulletRotation = Quaternion.Euler(0, 0, Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg);

        GameObject bullet = Instantiate(bulletPrefab, bulletOrigin.position, bulletRotation);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = aimDirection.normalized * bulletSpeed;

        if (bullet.TryGetComponent(out Bullet bulletScript))
        {
            bulletScript.SetDamage(PlayerStats.Instance.damage);
        }
    }

    private Vector2 GetAimDirection()
    {
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0;

        Vector2 dir = (mouseWorldPos - bulletOrigin.position);

        
        
        return dir;
    }

    IEnumerator ShootingCooldown()
    {
        _canShoot = false;

        float waitTime = 1 / PlayerStats.Instance.attackSpeed;
        yield return new WaitForSeconds(waitTime);

        _canShoot = true;

        if (canDebug)
        {
            Debug.Log($"AttackSpeed: {PlayerStats.Instance.attackSpeed}");
        }
    }
}