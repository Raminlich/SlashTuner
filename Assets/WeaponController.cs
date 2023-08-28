using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public CinemachineCameraShaker CameraShaker;
   
    // Create a Cinemachine Virtual Camera.


    private void OnTriggerEnter(Collider other)
    {
        OnWeaponCollision();
        print(other.name);
    }

    void OnWeaponCollision()
    {

        CameraShaker.ShakeCamera(.2f);
    }
}
