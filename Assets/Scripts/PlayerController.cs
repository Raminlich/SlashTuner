using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    public float sprintSpeed;
    public float turnSmooth;
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
        animator.SetTrigger("AttackTrigger");
        SetPlayerSate(CharacterState.Attack);
    }

    private void OnRollTrigger(InputAction.CallbackContext context)
    {
        animator.SetTrigger("RollTrigger");
        SetPlayerSate(CharacterState.Roll);

    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        speed = walkSpeed;
    }
    void Update()
    {
        Movement();

    }

    public void SetPlayerSate(CharacterState state)
    {
        characterState = state;
    }

    private void Movement()
    {
        if (characterState != CharacterState.Attack && characterState != CharacterState.Roll)
        {
            Vector2 movementInput = moveInput.ReadValue<Vector2>();
            var xMotion = movementInput.y;
            var zMotion = movementInput.x;
            Vector3 motionVector = new Vector3(xMotion, 0, zMotion);
            motionVector = transform.TransformPoint(motionVector);
            if (movementInput != Vector2.zero)
            {
                animator.SetBool("IsWalking", true);
                Vector3 moveDirection = transform.TransformDirection(new Vector3(movementInput.x, 0, movementInput.y));
                moveDirection = moveDirection * speed * Time.deltaTime;
                characterController.Move(moveDirection);
                RotateTowardDirection();
                if (characterState == CharacterState.Sprint)
                {

                  //  animator.SetBool("IsRunning", true) ;

                }
                else
                {
                   // animator.SetBool("IsRunning", false);


                }

            }
            else
            {
                SetPlayerSate(CharacterState.Idle);
                animator.SetBool("IsWalking", false);
                characterController.Move(Vector3.zero);

            }
            Debug.Log("speed is: " + characterController.velocity.sqrMagnitude);
            animator.SetFloat("CharacterSpeed", (float)(Mathf.Clamp(characterController.velocity.sqrMagnitude,0f,12.5f)/12.5));
            animator.SetFloat("TrajecetoryForwad",zMotion);
            animator.SetFloat("TrajecetorySide",xMotion);
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
    Attack
}