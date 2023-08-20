using StarterAssets;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField] private int framesBetweenCombo;
    [SerializeField] private int firstAttackFrames;
    [SerializeField] private int secondAttackFrames;
    [SerializeField] private float targetFindRadius;
    public int currentAttackFrames;
    private Animator animator;
    private PlayerInputs inputs;
    private ThirdPersonController thirdPersonController;
    private int attackCombo;
    private GameplayHelper gameplayHelper;

    void Start()
    {
        gameplayHelper = new GameplayHelper();
        animator = GetComponent<Animator>();
        inputs = GetComponent<PlayerInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        inputs.attackAction += OnPlayerAttack;
    }

    private void OnPlayerAttack()
    {
        thirdPersonController.SetPlayerState(CharacterState.Attack);
        animator.applyRootMotion = true;
        if (attackCombo < 2)
        {
            attackCombo++;
            if (animator.GetBool("Attack"))
                currentAttackFrames = secondAttackFrames + gameplayHelper.GetCurrentInterval();
            else
                currentAttackFrames = firstAttackFrames;
            StopAllCoroutines();
            StartCoroutine(gameplayHelper.FramedAction(currentAttackFrames, null, OnPlayerAttackFinished));
            animator.SetBool("Attack", true);
            animator.SetInteger("AttackCombo", attackCombo);
        }
    }

    private void OnPlayerAttackFinished()
    {
        currentAttackFrames = 0;
        attackCombo = 0;
        animator.SetBool("Attack", false);
        animator.applyRootMotion = false;
        thirdPersonController.SetPlayerState(CharacterState.Locomotion);
    }

    private void Update()
    {
        currentAttackFrames = gameplayHelper.GetCurrentInterval();
    }
}
