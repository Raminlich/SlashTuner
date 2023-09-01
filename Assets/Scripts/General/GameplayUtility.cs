using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class GameplayUtility
{
    public static Transform SphereOverlapClosestObject(Transform current, float targetFindRadius, string tag)
    {
        Collider[] targetsArray = Physics.OverlapSphere(current.position, targetFindRadius);
        var targetsList = targetsArray.Where(t => t.gameObject.CompareTag(tag));
        if (targetsList.Count() == 0)
        {
            Debug.Log($"No Objects with tag '{tag}' found! returning null");
            return null;
        }
        var target = targetsList.OrderBy(t => Vector3.Distance(current.position, t.transform.position)).First();
        return target.transform;
    }

    public static List<Collider> SphereOverlapAllObject(Transform current, float targetFindRadius, string tag)
    {
        Collider[] targetsArray = Physics.OverlapSphere(current.position, targetFindRadius);
        var targetsList = targetsArray.Where(t => t.gameObject.CompareTag(tag));
        if (targetsList.Count() == 0)
        {
            Debug.Log($"No Objects with tag '{tag}' found! returning null");
            return null;
        }
        targetsList = targetsList.OrderBy(t => Vector3.Distance(current.position, t.transform.position));
        return targetsList.ToList();
    }
}
