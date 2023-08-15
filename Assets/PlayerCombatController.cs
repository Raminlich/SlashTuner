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
    void Start()
    {
        animator = GetComponent<Animator>();
        inputs = GetComponent<PlayerInputs>();
        inputs.attackAction += OnPlayerAttack;
    }

    private void OnPlayerAttack()
    {
        animator.SetBool("Attack",true);
        StartCoroutine(GameplayHelper.FramedAction(attackFrames, () => { },() => OnAttackFinish()));
    }

    private void OnAttackFinish()
    {
        animator.SetBool("Attack", false);

    }
}
