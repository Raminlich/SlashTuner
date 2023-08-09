using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    public float sprintSpeed;
    public float turnSmooth;
    public float gravity;
    public float gravityMultiplier;
    [SerializeField] private CharacterState characterState;
    private CharacterController characterController;
    private PlayerInputs playerInputs;
    private InputAction moveInput;
    private InputAction fireInput;
    private InputAction rollInput;
    private InputAction sprintInput;
    private Animator animator;
    private float turnSmoothVelocity;
    private float speed;
    private Vector3 movementDirection;
    private Vector2 moveInputValue;

    private void OnEnable()
    {
        moveInput = playerInputs.Player.Move;
        fireInput = playerInputs.Player.Fire;
        rollInput = playerInputs.Player.Roll;
        sprintInput = playerInputs.Player.Sprint;
        rollInput.performed += OnRollTrigger;
        fireInput.performed += OnAttackTrigger;
        sprintInput.performed += OnSprintTrigger;
        sprintInput.canceled += OnSprintCanceled;
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

    }

    void Start()
    {
        speed = walkSpeed;
    }

    void Update()
    {
        GetInput();
        ApplyMovement();
    }

    private void GetInput()
    {
        moveInputValue = moveInput.ReadValue<Vector2>();
        if (moveInputValue != Vector2.zero)
            StartMove();
        else
            CancelMove();
    }

    private void StartMove()
    {
        var moveDirection = transform.TransformDirection(new Vector3(moveInputValue.x, 0, moveInputValue.y));
        if (characterState == CharacterState.Roll || characterState == CharacterState.Attack) return;
        animator.SetBool("IsWalking", true);
        movementDirection = moveDirection * speed * Time.deltaTime;
        ApplyCharacterRotation();
    }

    private void CancelMove()
    {
        SetPlayerSate(CharacterState.Idle);
        animator.SetBool("IsWalking", false);
        movementDirection = Vector3.zero;
    }

    private void ApplyMovement()
    {
        movementDirection += GetGravityMovementValue();
        characterController.Move(movementDirection);

        animator.SetFloat("CharacterSpeed", (float)(Mathf.Clamp(characterController.velocity.sqrMagnitude, 0f, 12.5f) / 12.5));
        animator.SetFloat("TrajecetoryForwad", moveInputValue.x);
        animator.SetFloat("TrajecetorySide", moveInputValue.y);
    }

    public void SetPlayerSate(CharacterState state)
    {
        characterState = state;
    }

    private void ApplyCharacterRotation()
    {
        var direction = Camera.main.transform.eulerAngles;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, direction.y, ref turnSmoothVelocity, turnSmooth);
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    private Vector3 GetGravityMovementValue()
    {
        return new Vector3(0, gravity, 0) * Time.deltaTime * gravityMultiplier;
    }

    private void OnSprintTrigger(InputAction.CallbackContext context)
    {
        speed = walkSpeed + sprintSpeed;
        SetPlayerSate(CharacterState.Sprint);
    }

    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        SetPlayerSate(CharacterState.Walk);
        speed = walkSpeed;
    }

    private void OnAttackTrigger(InputAction.CallbackContext context)
    {
        if (characterState != CharacterState.Roll)
        {
            SetPlayerSate(CharacterState.Attack);
            animator.SetBool("OnAttack", true);
        }
        movementDirection = Vector3.zero;
    }

    private void OnRollTrigger(InputAction.CallbackContext context)
    {
        animator.SetTrigger("RollTrigger");
        SetPlayerSate(CharacterState.Roll);
        animator.SetFloat("CharacterSpeed", 0);
        movementDirection = Vector3.zero;
    }
}
public enum CharacterState
{
    Idle,
    Walk,
    Sprint,
    Roll,
    Attack,
    Fall
}