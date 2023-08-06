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

    private void OnEnable()
    {
        move = playerInputs.Player.Move;
        fire = playerInputs.Player.Fire;
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
        Vector2 movement = move.ReadValue<Vector2>() * Time.deltaTime * speed;
        characterController.Move(new Vector3(movement.x,0,movement.y));
        Debug.Log(move.ReadValue<Vector2>());
    }
}
