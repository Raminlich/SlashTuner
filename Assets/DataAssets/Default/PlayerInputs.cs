using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public Vector2 move;
    public Vector2 look;
    public Vector2 stanceLook;
    public bool jump;
    public bool sprint;
    public Action attackAction;
    public Action lockAction;
    public Action dodgeAction;
    public Action stanceAction;
    public Action nextTarget;

    public bool analogMovement;

    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        if (cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
    }

    public void OnStancePointer(InputValue value)
    {
        StancePointerInput(value.Get<Vector2>());
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
    }

    public void OnAttack()
    {
        AttackInput();
    }

    public void OnLock()
    {
        LockInput();
    }

    public void OnDodge()
    {
        DodgeInput();
    }

    public void OnNextClosestTarget()
    {
        print("Finding next target");
        nextTarget?.Invoke();
    }

    //----------------------------------------------------------------------

    private void DodgeInput()
    {
        dodgeAction?.Invoke();
    }

    public void AttackInput()
    {
        attackAction?.Invoke();
    }

    private void LockInput()
    {
        lockAction?.Invoke();
    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void StancePointerInput(Vector2 newStance)
    {
        stanceLook = newStance;
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }

    public void SprintInput(bool newSprintState)
    {
        sprint = newSprintState;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}