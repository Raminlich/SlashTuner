using System.Collections;
using UnityEditor;
using UnityEngine;

public class RegularEnemy : BaseEnemy
{
    public Transform sphereOverlapOrigin;
    private bool isAttacking;
    GameplayHelper gameplayHelper;
    public int attackFrame;
    public int damageFrame;
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
        yield return new WaitForSeconds(stunDuaration);
        agent.enabled = true;
        rigidbody.isKinematic = true;
    }

    protected override void Attack()
    {
        if (!isAttacking)
        {
            StartCoroutine(gameplayHelper.FramedAction(attackFrame, null, () => isAttacking = false));
            StartCoroutine(new GameplayHelper().FramedAction(damageFrame, null, DoDamage));
            isAttacking = true;
            animator.SetTrigger("Attack");
        }
    }

    private void DoDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(sphereOverlapOrigin.transform.position, .3f);
        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                print("Damaging the player....");
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(sphereOverlapOrigin.transform.position, .3f);
    }
}
