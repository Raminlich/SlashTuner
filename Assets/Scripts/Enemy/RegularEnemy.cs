using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class RegularEnemy : BaseEnemy, IStagger
{
    public Transform sphereOverlapOrigin;
    private bool isAttacking;
    GameplayHelper gameplayHelper;
    public int attackFrame;
    public int damageFrame;
    public int pushForce;
    public Collider weaponCollider;
    public ParticleSystem attackIndicator;
    private void Start()
    {
        gameplayHelper = new GameplayHelper();
        Init();
    }
    private void Update()
    {
        if (IsPlayerWithinRadar())
        {
            AlwaysLookAt();
            GetInRange();
        }

        if (IsWithinRange())
            Attack();
    }

    public Vector3 CalculateDirection(string attacker)
    {
        var player = GameObject.FindGameObjectWithTag(attacker);
        return transform.position - player.transform.position;
    }

    public IEnumerator StunCheck()
    {
        yield return new WaitForSeconds(stunDuration);
        agent.enabled = true;
        rigidbody.isKinematic = true;
    }

    protected override void Attack()
    {
        if (!isAttacking)
        {
            StartCoroutine(gameplayHelper.FramedAction(attackFrame, null, () => isAttacking = false));
            //StartCoroutine(new GameplayHelper().FramedAction(damageFrame, null, () => weaponCollider.enabled = true));
            //StartCoroutine(new GameplayHelper().FramedAction(damageFrame, null, DoDamage));
            isAttacking = true;
            animator.SetTrigger("Attack");
        }
    }

    private void DoDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(sphereOverlapOrigin.position, .3f);
        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                print("Damaging the player....");
                collider.GetComponent<HealthController>().TakeDamage(20);
            }
        }
    }

    public void BasicAgentPush()
    {
        agent.enabled = false;
        StartCoroutine(new GameplayHelper().FramedAction(60, BasicPush, RegainControl));
    }

    private void BasicPush()
    {
        transform.Translate(Vector3.forward * -pushForce * Time.deltaTime);
    }

    private void RegainControl()
    {
        agent.enabled = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(sphereOverlapOrigin.transform.position, .3f);
    }

    public void WeaponStagger()
    {
        BasicAgentPush();
        animator.SetTrigger("Stagger");
    }

    public void DamageStagger()
    {
        print("Enemy damage stagger");
    }
    
    public void IndicatorVFX()
    {
        attackIndicator.Play();
    }

    public void EnableWeaponCollider()
    {
        weaponCollider.enabled = true;
    }

    public void DisableWeaponCollider()
    {
        weaponCollider.enabled = false;
    }
}
