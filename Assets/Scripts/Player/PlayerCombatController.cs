using StarterAssets;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField] private Transform sphereOverlapOrigin;
    [SerializeField] private ComboData comboData;
    public int currentAttackFrames;
    private Animator animator;
    private PlayerInputs inputs;
    private ThirdPersonController thirdPersonController;
    private int attackCombo;
    private GameplayHelper gameplayHelper;
    private ComboController comboController;

    void Start()
    {
        gameplayHelper = new GameplayHelper();
        comboController = GetComponent<ComboController>();
        animator = GetComponent<Animator>();
        inputs = GetComponent<PlayerInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        inputs.attackAction += OnPlayerAttack;
        comboController.SetFinalAction(OnPlayerAttackFinished);
        comboController.SetDamageAction(DoDamage);
    }

    private void OnPlayerAttack()
    {
        if (thirdPersonController.cameraState == LocomotionState.Free) return;
        thirdPersonController.SetPlayerState(CharacterState.Attack);
        if (!comboController.IsBusy())
        {
            if (attackCombo < comboData.comboList.Count)
            {
                comboController.AddCombo(comboData.comboList[attackCombo]);
                attackCombo++;
                animator.SetBool("Attack", true);
                animator.SetInteger("AttackCombo", attackCombo);
            }
        }
    }

    private void OnPlayerAttackFinished()
    {
        currentAttackFrames = 0;
        attackCombo = 0;
        animator.SetBool("Attack", false);
        thirdPersonController.SetPlayerState(CharacterState.Locomotion);
    }

    private void DoDamage()
    {
        print("Trying to do damage...");
        Collider[] hitColliders = Physics.OverlapSphere(sphereOverlapOrigin.position, .3f);
        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                print("Damaging the Enemy....");
                collider.GetComponent<HealthController>().TakeDamage(comboController.GetComboDamage());
            }
        }
    }

    private void AttackQueue()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(sphereOverlapOrigin.transform.position, .3f);
    }

    private void Update()
    {
        currentAttackFrames = gameplayHelper.GetCurrentInterval();
        
    }
}
