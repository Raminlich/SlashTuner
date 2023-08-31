using UnityEngine;

public class WeaponController : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Collider>().enabled = false;
        if (other.gameObject.CompareTag("Weapon"))
        {
            print("Weapon collided");
            var character = other.GetComponentInParent<IStagger>();
            character.WeaponStagger();
        }
        else if(other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            var character = other.GetComponentInParent<IStagger>();
            character.DamageStagger();
        }
    }


}
