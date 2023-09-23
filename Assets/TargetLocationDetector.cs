using StarterAssets;
using UnityEngine;

public class TargetLocationDetector : MonoBehaviour
{
    public bool isRight;

    private ThirdPersonController thirdPersonController;

    private void Start()
    {
        thirdPersonController = GetComponentInParent<ThirdPersonController>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy"))
        {
            if (isRight)
                thirdPersonController.allRightTargets.Add(other);
            else
                thirdPersonController.allLeftTargets.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (isRight)
                thirdPersonController.allRightTargets.Remove(other);
            else
                thirdPersonController.allLeftTargets.Remove(other);
        }
    }
}
