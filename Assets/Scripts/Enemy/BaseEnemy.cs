using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemy : MonoBehaviour
{
    public float moveSpeed;
    public float range;
    public float attackSpeed;
    public float stunDuration;
    [Range(0, 1)]
    public float RotationSmoothTime;

    protected NavMeshAgent agent;
    protected Rigidbody rigidbody;
    protected Animator animator;
    protected GameObject target;

    private float rotationVelocity;
    protected void Init()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        agent.speed = moveSpeed;
        agent.stoppingDistance = range - 0.3f;
    }

    protected virtual void GetInRange()
    {
        agent.SetDestination(target.transform.position);
    }

    protected virtual bool IsWithinRange()
    {
        var distance = Vector3.Distance(target.transform.position, transform.position);
        return distance <= range;
    }

    public bool IsPlayerWithinRadar()
    {
        Vector3 direction = target.transform.position - transform.position;
        Vector3 rayDirection = new Vector3(direction.x, 0, direction.z);
        Ray aimRay = new Ray(transform.position, rayDirection);
        RaycastHit aimHit;
        if (Physics.Raycast(aimRay, out aimHit, 30))
        {
            if (aimHit.transform.tag == "Player")
            {
                return true;
            }

        }
        Debug.DrawRay(aimRay.origin, new Vector3(direction.x, 0, direction.z) * 100, Color.red);

        return false;
    }

    public void AlwaysLookAt()
    {
        Vector3 lookAtRotation = Quaternion.LookRotation(target.transform.position - transform.position).eulerAngles;
        var rotationScaleValue = Vector3.Scale(lookAtRotation, new Vector3(0, 1, 0));
        var rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationScaleValue.y, ref rotationVelocity, RotationSmoothTime);
        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }

    protected abstract void Attack();


}
