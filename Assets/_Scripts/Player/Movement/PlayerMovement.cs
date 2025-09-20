using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [Header("DEBUG")]
    [SerializeField] private bool canDebug = false;

    [Space(10)]
    [Header("Input")]
    [SerializeField] private InputActionAsset inputActions;

    private InputAction _movementAction;
    private InputAction _jumpAction;

    [Space(10)]
    [Header("Attributes")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    private float _horizontalInput;

    [Space(10)]
    [Header("Ground Check")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [Space(10)]
    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime = 0.2f;
    private float _coyoteTimeCounter;

    private bool facingRight = true;


    // Referência ao controlador de animação
    private PlayerAnimationController animController;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        var playerMap = inputActions.FindActionMap("Player");
        _movementAction = playerMap.FindAction("Movement");
        _jumpAction = playerMap.FindAction("Jump");

        animController = GetComponent<PlayerAnimationController>();
    }

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
        _jumpAction.started += OnJump;
        _jumpAction.canceled += OnJump;
    }

    private void OnDisable()
    {
        _jumpAction.started -= OnJump;
        _jumpAction.canceled -= OnJump;
        inputActions.FindActionMap("Player").Disable();
    }

    private void Update()
    {
        _horizontalInput = _movementAction.ReadValue<Vector2>().x;
        moveSpeed = PlayerStats.Instance.moveSpeed;

        HandleAnimations();
        HandleCoyoteTime();

        //Debug.Log($"InputX: {_horizontalInput}, VelX: {rb.linearVelocity.x}, PosX: {rb.position.x}");
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(_horizontalInput * moveSpeed, rb.linearVelocity.y);
        HandleFlip();

        Debug.Log($"Simulated: {rb.simulated}, InputX: {_horizontalInput}, VelX: {rb.linearVelocity.x}, PosX: {rb.position.x}, BodyType: {rb.bodyType}");
    }




    //private void HandleMovement()
    //{
    //    _horizontalInput = _movementAction.ReadValue<Vector2>().x;
    //    Debug.Log($"InputX: {_horizontalInput}, VelX: {rb.linearVelocity.x}");
    //    rb.linearVelocity = new Vector2(_horizontalInput * moveSpeed, rb.linearVelocity.y);
    //}


    private void HandleAnimations()
    {
        // Atualiza floats no Animator
        animController.SetWalkSpeed(Mathf.Abs(rb.linearVelocity.x / moveSpeed));
        
    }

    private void HandleCoyoteTime()
    {
        if (_IsGrounded())
        {
            _coyoteTimeCounter = coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && (_coyoteTimeCounter > 0f || PlayerStats.Instance.hasWings))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            _coyoteTimeCounter = 0f;

            animController.SetJumpVelocity(rb.linearVelocity.y);
        }

        if (context.canceled && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0);

            animController.SetJumpVelocity(rb.linearVelocity.y);
        }
    }

    private void HandleFlip()
    {
        if (rb.linearVelocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (rb.linearVelocity.x < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        // Gira apenas no eixo Y
        transform.Rotate(0f, 180f, 0f);
        facingRight = !facingRight;
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
