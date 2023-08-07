using System;
using System.Collections;
using System.Collections.Generic;
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
        speed = walkSpeed;
    }

    private void OnSprintTrigger(InputAction.CallbackContext context)
    {
        speed = walkSpeed + sprintSpeed;
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
        if(characterState != CharacterState.Attack && characterState != CharacterState.Roll)
        {
            Vector2 movementInput = moveInput.ReadValue<Vector2>();
            if (movementInput != Vector2.zero)
            {
                animator.SetBool("IsWalking", true);
                RotateTowardDirection();
            }
            animator.SetBool("IsWalking", false);
            Vector3 moveDirection = transform.TransformDirection(new Vector3(movementInput.x, 0, movementInput.y));
            moveDirection = moveDirection * speed * Time.deltaTime;
            characterController.Move(moveDirection);
            animator.SetFloat("MovementSpeed", characterController.velocity.magnitude);
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