using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private int movementRotationFrames;
    [SerializeField] private int rollFrames;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private CharacterState characterState;
    [SerializeField] private MovementRotationType movementRotationType;
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
    private const float gravity = -9.81f;
    public float yLocal;
    public float yGlobal;

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
        if (characterState == CharacterState.Roll || characterState == CharacterState.Attack) return;
        var moveDirection = transform.TransformDirection(new Vector3(moveInputValue.x, 0, moveInputValue.y));
        animator.SetBool("IsWalking", true);
        movementDirection = moveDirection * speed * Time.deltaTime;
        CameraBaseRotation();
    }

    private void CameraBaseRotation()
    {
        var direction = Camera.main.transform.eulerAngles;
        ApplyCharacterRotation(direction);
    }

    private void InputBasedRotation()
    {
        var angle = Mathf.Atan2(moveInputValue.x, moveInputValue.y) * Mathf.Rad2Deg;
        var direction = new Vector3(0, angle+transform.eulerAngles.y, 0);
        ApplyCharacterRotation(direction);
    }

    private void CancelMove()
    {
        animator.SetBool("IsWalking", false);
        movementDirection = Vector3.zero;
    }

    private void ApplyMovement()
    {
        if (characterState == CharacterState.Roll || characterState == CharacterState.Attack) return;
        movementDirection += GetGravityMovementValue();
        characterController.Move(movementDirection);
        var charSpeed = Mathf.Clamp(characterController.velocity.sqrMagnitude, 0, 12.5f) / 12.5f;
        animator.SetFloat("CharacterSpeed", charSpeed);
        animator.SetFloat("TrajecetoryForwad", moveInputValue.x);
        animator.SetFloat("TrajecetorySide", moveInputValue.y);

    }

    public void SetPlayerSate(CharacterState state)
    {
        characterState = state;
    }

    private void ApplyCharacterRotation(Vector3 direction)
    {
        float rotationFrames = (float)movementRotationFrames / 60;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, direction.y, ref turnSmoothVelocity, rotationFrames);
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
        movementDirection = Vector3.zero;
        if (characterState != CharacterState.Roll)
        {
            animator.SetBool("OnAttack", true);
        }
    }

    private void OnRollTrigger(InputAction.CallbackContext context)
    {
        if (characterState != CharacterState.Roll)
        {
            if (moveInputValue != Vector2.zero)
                StartCoroutine(GameplayHelper.FramedAction(() => InputBasedRotation(), rollFrames));

            animator.SetTrigger("RollTrigger");
            animator.SetFloat("CharacterSpeed", 0);
            movementDirection = Vector3.zero;
        }
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

public enum MovementRotationType
{
    Camera,
    Input
}