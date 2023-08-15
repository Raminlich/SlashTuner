using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField] private int attackFrames;
    private Animator animator;
    private PlayerInputs inputs;
    private ThirdPersonController thirdPersonController;
    void Start()
    {
        animator = GetComponent<Animator>();
        inputs = GetComponent<PlayerInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        inputs.attackAction += OnPlayerAttack;
    }

    private void OnPlayerAttack()
    {
        if(thirdPersonController.GetPlayerState() == CharacterState.Locomotion)
        {
            animator.SetBool("Attack", true);
            StartCoroutine(GameplayHelper.FramedAction(attackFrames, () => { }, () => OnAttackFinish()));
        }

    }

    private void OnAttackFinish()
    {
        animator.SetBool("Attack", false);

    }
}
