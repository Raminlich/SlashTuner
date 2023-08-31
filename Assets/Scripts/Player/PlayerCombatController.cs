using DG.Tweening;
using StarterAssets;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour, IStagger
{
    [SerializeField] private Transform sphereOverlapOrigin;
    [SerializeField] private ComboData comboData;
    [SerializeField] private ParticleSystem weaponVFX;
    public int currentAttackFrames;
    private Animator animator;
    private PlayerInputs inputs;
    private ThirdPersonController thirdPersonController;
    private int attackCombo;
    private GameplayHelper gameplayHelper;
    private ComboController comboController;
    private float weaponDirX;
    private float weaponDirY;
    public float swingTime = 1f;
    public Vector2 currentWeaponDirection;
    public float stanceMultiplier;
    public float stanceThreshold;
    public Collider weaponCollider;
    private Vector2 weaponTargetDir;
    public CinemachineCameraShaker CameraShaker;

    void Start()
    {
        //Time.timeScale = .4f;
        gameplayHelper = new GameplayHelper();
        comboController = GetComponent<ComboController>();
        animator = GetComponent<Animator>();
        inputs = GetComponent<PlayerInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        inputs.attackAction += OnPlayerAttack;
        comboController.SetFinalAction(OnPlayerAttackFinished);
        comboController.SetDamageAction(DoDamage);
    }

    private void WeaponStance()
    {
        if (!thirdPersonController.LockOn) return;

        if (inputs.stanceLook.sqrMagnitude > 0)
        {
            if (inputs.stanceLook.sqrMagnitude > stanceThreshold)
            {
                currentWeaponDirection += (inputs.stanceLook) * stanceMultiplier;
            }

            currentWeaponDirection.x = Mathf.Clamp(currentWeaponDirection.x, -1, 1);
            currentWeaponDirection.y = Mathf.Clamp(currentWeaponDirection.y, -1, 1);

            animator.SetFloat("StanceX", currentWeaponDirection.x);
            animator.SetFloat("StanceY", currentWeaponDirection.y);
        }



    }

    private void OnPlayerAttack()
    {
        if (thirdPersonController.cameraState == LocomotionState.Free || thirdPersonController.GetPlayerState() == CharacterState.Attack) return;
        OnAttackStart();
        thirdPersonController.SetPlayerState(CharacterState.Attack);
        var weaponDirection = currentWeaponDirection;
        weaponTargetDir = new Vector2(weaponDirection.x, weaponDirection.y) * -1;
        DOVirtual.Vector3(weaponDirection, weaponTargetDir, swingTime, (returnType) =>
        {
            animator.SetFloat("StanceX", returnType.x);
            animator.SetFloat("StanceY", returnType.y);
        }).SetEase(Ease.OutCubic).onComplete += OnAttackEnd;

    }

    private void OnAttackStart()
    {
        weaponVFX.Play();
        weaponCollider.enabled = true;
    }

    private void OnAttackEnd()
    {
        weaponCollider.enabled = false;
        weaponVFX.Stop();
        currentWeaponDirection = weaponTargetDir;
        thirdPersonController.SetPlayerState(CharacterState.Locomotion);
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
        WeaponStance();
    }

    public void WeaponStagger()
    {
        OnWeaponCollision();
    }

    public void DamageStagger()
    {
        print("Player damage stagger");
    }

    void OnWeaponCollision()
    {
        CameraShaker.ShakeCamera(.2f);
    }
}
