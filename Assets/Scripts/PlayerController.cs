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

    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        SetPlayerSate(CharacterState.Walk);
        speed = walkSpeed;
    }

    private void OnSprintTrigger(InputAction.CallbackContext context)
    {
        speed = walkSpeed + sprintSpeed;
        SetPlayerSate(CharacterState.Sprint);

    }

    private void OnAttackTrigger(InputAction.CallbackContext context)
    {
        //animator.SetTrigger("AttackTrigger");
        if(characterState != CharacterState.Roll) 
        {
            SetPlayerSate(CharacterState.Attack);
            animator.SetBool("OnAttack", true);
        }

    }

    private void OnRollTrigger(InputAction.CallbackContext context)
    {
        animator.SetTrigger("RollTrigger");
        SetPlayerSate(CharacterState.Roll);
        animator.SetFloat("CharacterSpeed", 0);

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
        Movement();
        SetGravity();

    }

    public void SetPlayerSate(CharacterState state)
    {
        characterState = state;
    }

    private void Movement()
    {
        if (characterState != CharacterState.Attack && characterState != CharacterState.Roll)
        {
            Vector2 inputValue = moveInput.ReadValue<Vector2>();
            var xMotion = inputValue.y;
            var zMotion = inputValue.x;
            if (inputValue != Vector2.zero)
            {
                animator.SetBool("IsWalking", true);
                Vector3 moveDirection = transform.TransformDirection(new Vector3(inputValue.x, 0, inputValue.y));
                movementDirection = moveDirection * speed * Time.deltaTime;
                characterController.Move(movementDirection);
                RotateTowardDirection();

            }
            else
            {
                SetPlayerSate(CharacterState.Idle);
                animator.SetBool("IsWalking", false);
            }
            animator.SetFloat("CharacterSpeed", (float)(Mathf.Clamp(characterController.velocity.sqrMagnitude,0f,12.5f)/12.5));
            animator.SetFloat("TrajecetoryForwad",zMotion);
            animator.SetFloat("TrajecetorySide",xMotion);
        }

    }

    private void SetGravity()
    {
        if(!characterController.isGrounded)
        {
            Debug.Log("Is not grounded");
            characterController.Move(new Vector3(0, gravity, 0) * Time.deltaTime * gravityMultiplier);
        }
        else
        {
            Debug.Log("Is grounded");

        }
    }

    private void RotateTowardDirection()
    {
        var direction = Camera.main.transform.eulerAngles;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, direction.y, ref turnSmoothVelocity, turnSmooth);
        transform.rotation = Quaternion.Euler(0, angle, 0);
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