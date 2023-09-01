using UnityEngine;

[RequireComponent(typeof(EntityStats))]
public class HealthController : MonoBehaviour
{
    private EntityStats _entityStats;
    private void Awake()
    {
        _entityStats = GetComponent<EntityStats>();
    }

    public virtual void TakeDamage(float value)
    {
        _entityStats.Health -= value;
        if( _entityStats.Health <= 0 )
            Death();
    }

    public virtual void HealDamage(float value)
    {
        _entityStats.Health += value;
    }

    private void Death()
    {
        print($"{transform.name} Died");
        Destroy(gameObject);
    }
}
