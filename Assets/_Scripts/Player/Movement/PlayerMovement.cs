using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

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
        rb.linearVelocity = new Vector2(_horizontal * moveSpeed, rb.linearVelocity.y);

        if (_IsGrounded())
        {
            _coyoteTimeCounter = coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }

        Debug.Log($"Grounded: {_IsGrounded()} | CoyoteTime: {_coyoteTimeCounter}");
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
        if (context.performed && _coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
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
}
