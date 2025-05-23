using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [Header("DEBUG")]
    [SerializeField] bool canDebug = false;

    [Space(10)]
    [Header("Input")]
    [SerializeField] private InputActionAsset inputActions;

    private InputAction _movementAction;
    private InputAction _jumpAction;

    [Space(10)]
    [Header("Attributes")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jump;
    private float _horizontal;

    [Space(10)]
    [Header("Ground Check")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [Space(10)]
    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime;
    private float _coyoteTimeCounter;

    [Space(10)]
    [Header("Dash")]
    [SerializeField] private float dashForce = 50f;
    [SerializeField] private float dashCooldown = 0.5f;
    private bool _canDash = true;
    private bool _isDashing = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        var playerMap = inputActions.FindActionMap("Player");
        _movementAction = playerMap.FindAction("Movement");
        _jumpAction = playerMap.FindAction("Jump");
    }

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();

        _movementAction.performed += OnMove;
        _movementAction.canceled += OnMove;

        _jumpAction.performed += OnJump;
        _jumpAction.canceled += OnJump;
    }

    private void OnDisable()
    {
        _movementAction.performed -= OnMove;
        _movementAction.canceled -= OnMove;

        _jumpAction.performed -= OnJump;
        _jumpAction.canceled -= OnJump;

        inputActions.FindActionMap("Player").Disable();
    }

    private void Update()
    {
        moveSpeed = PlayerStats.Instance.moveSpeed;

        // Movimento normal só se não estiver dando Dash
        if (!_isDashing)
        {
            rb.linearVelocity = new Vector2(_horizontal * moveSpeed, rb.linearVelocity.y);
        }

        if (Keyboard.current.leftShiftKey.wasPressedThisFrame && PlayerStats.Instance.hasDash && _canDash)
        {
            Debug.Log("Executando Dash");
            StartCoroutine(Dash());
        }

        if (_IsGrounded())
        {
            _coyoteTimeCounter = coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }

        if (canDebug)
        {
            Debug.Log($"MoveSpeed: {moveSpeed}");
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _horizontal = 0f;
        }
        else
        {
            _horizontal = context.ReadValue<Vector2>().x;
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && (_coyoteTimeCounter > 0f || PlayerStats.Instance.hasWings))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
            if (!PlayerStats.Instance.hasWings)
                _coyoteTimeCounter = 0f;
        }

        if (context.canceled && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    private bool _IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }
    }

    private IEnumerator Dash()
    {
        Debug.Log("DASH: Aplicando força");

        _isDashing = true;
        _canDash = false;

        // Define direção do dash: se parado, vai para a direita.
        float dashDirection = (_horizontal != 0 ? Mathf.Sign(_horizontal) : 1);

        rb.linearVelocity = new Vector2(dashDirection * dashForce, rb.linearVelocity.y);

        yield return new WaitForSeconds(0.1f); // duração do dash

        _isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        _canDash = true;
    }
}
