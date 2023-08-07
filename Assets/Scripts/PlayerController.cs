using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float turnSmooth;
    public AnimationCurve playerRotationCurve;
    private CharacterController characterController;
    private PlayerInputs playerInputs;
    private InputAction move;
    private InputAction fire;
    private InputAction look;
    private InputAction roll;
    private Animator animator;
    private float turnSmoothVelocity;

    private void OnEnable()
    {
        move = playerInputs.Player.Move;
        fire = playerInputs.Player.Fire;
        look = playerInputs.Player.Look;
        roll = playerInputs.Player.Roll;
        roll.performed += RollTrigger;
        fire.performed += AttackTrigger;
        playerInputs.Enable();

    }

    private void AttackTrigger(InputAction.CallbackContext context)
    {
        animator.SetTrigger("AttackTrigger");
    }

    private void RollTrigger(InputAction.CallbackContext context)
    {
        animator.SetTrigger("RollTrigger");
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

    private void Movement()
    {
        Vector2 movementInput = move.ReadValue<Vector2>();
        if (movementInput != Vector2.zero)
        {
            animator.SetBool("IsWalking", true);
            var direction = Camera.main.transform.eulerAngles;
            if(direction.magnitude >= 0.01)
            {
                float angle = Mathf.SmoothDamp(transform.eulerAngles.y, direction.y, ref turnSmoothVelocity, turnSmooth);
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }
        }
        animator.SetBool("IsWalking", false);
        Vector3 moveDirection = transform.TransformDirection(new Vector3(movementInput.x, 0, movementInput.y));
        moveDirection = moveDirection * speed * Time.deltaTime;
        characterController.Move(moveDirection);
        animator.SetFloat("MovementSpeed",characterController.velocity.magnitude);
    }
}
