using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private CharacterController characterController;
    private PlayerInputs playerInputs;
    private InputAction move;
    private InputAction fire;
    private InputAction look;

    private void OnEnable()
    {
        move = playerInputs.Player.Move;
        fire = playerInputs.Player.Fire;
        look = playerInputs.Player.Look;
        playerInputs.Enable();

    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    private void Awake()
    {
        playerInputs = new PlayerInputs();
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
        if(movementInput != Vector2.zero)
        {
            var direction = Camera.main.transform.forward;
            Debug.Log(direction);
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        Vector3 moveDirection = transform.TransformDirection(new Vector3(movementInput.x, 0, movementInput.y));
        moveDirection = moveDirection * speed * Time.deltaTime;
        characterController.Move(moveDirection);
    }
}
