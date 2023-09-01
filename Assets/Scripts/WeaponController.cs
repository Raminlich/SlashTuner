using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Collider>().enabled = false;
        if (other.gameObject.CompareTag("Weapon"))
        {
            print("Weapon collided");
            var stagger = other.GetComponentInParent<IStagger>();
            stagger.WeaponStagger();
        }
        else if(other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            var stagger = other.GetComponentInParent<IStagger>();
            var health = other.GetComponentInParent<HealthController>();
            health.TakeDamage(damage);
            stagger.DamageStagger();
        }
    }


}
