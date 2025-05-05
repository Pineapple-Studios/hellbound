using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [Header("Input")]
    [SerializeField] private InputActionAsset inputActions;

    private InputAction movementAction;
    private InputAction jumpAction;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jump;
    private float _horizontal;

    [Header("Ground Check")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime;
    private float coyoteTimeCounter;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        var playerMap = inputActions.FindActionMap("Player");
        movementAction = playerMap.FindAction("Movement");
        jumpAction = playerMap.FindAction("Jump");
    }

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();

        movementAction.performed += OnMove;
        movementAction.canceled += OnMove;

        jumpAction.performed += OnJump;
        jumpAction.canceled += OnJump;
    }

    private void OnDisable()
    {
        movementAction.performed -= OnMove;
        movementAction.canceled -= OnMove;

        jumpAction.performed -= OnJump;
        jumpAction.canceled -= OnJump;

        inputActions.FindActionMap("Player").Disable();
    }

    private void Update()
    {
        rb.linearVelocity = new Vector2(_horizontal * moveSpeed, rb.linearVelocity.y);

        if (_IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        Debug.Log($"Grounded: {_IsGrounded()} | CoyoteTime: {coyoteTimeCounter}");
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
        if (context.performed && coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
            coyoteTimeCounter = 0f;
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
