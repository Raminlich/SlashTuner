using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float turnSmooth;
    [SerializeField] private CharacterState characterState;
    public AnimationCurve playerRotationCurve;
    private CharacterController characterController;
    private PlayerInputs playerInputs;
    private InputAction moveInput;
    private InputAction fireInput;
    private InputAction rollInput;
    private Animator animator;
    private float turnSmoothVelocity;

    private void OnEnable()
    {
        moveInput = playerInputs.Player.Move;
        fireInput = playerInputs.Player.Fire;
        rollInput = playerInputs.Player.Roll;
        rollInput.performed += RollTrigger;
        fireInput.performed += AttackTrigger;
        playerInputs.Enable();

    }

    private void AttackTrigger(InputAction.CallbackContext context)
    {
        animator.SetTrigger("AttackTrigger");
        SetPlayerSate(CharacterState.Attack);
    }

    private void RollTrigger(InputAction.CallbackContext context)
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
                var direction = Camera.main.transform.eulerAngles;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, direction.y, ref turnSmoothVelocity, turnSmooth);
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }
            animator.SetBool("IsWalking", false);
            Vector3 moveDirection = transform.TransformDirection(new Vector3(movementInput.x, 0, movementInput.y));
            moveDirection = moveDirection * speed * Time.deltaTime;
            characterController.Move(moveDirection);
            animator.SetFloat("MovementSpeed", characterController.velocity.magnitude);
        }

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