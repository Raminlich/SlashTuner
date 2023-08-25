using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboController : MonoBehaviour
{
    [SerializeField] private bool isLocked;
    [SerializeField] private bool isExecuting;
    private List<AttackCombo> currentCombo = new();
    private Action finalAction;

    public bool IsBusy()
    {
        return isLocked;
    }

    public void SetFinalAction(Action finalAction)
    {
        this.finalAction = finalAction;
    }

    public void AddCombo(AttackCombo combo)
    {
        currentCombo.Add(combo);
        if (!isExecuting)
            ExecuteNextCombo();
    }

    public void RemoveCombo(AttackCombo combo)
    {
        currentCombo.Remove(combo);
    }

    private void ExecuteNextCombo()
    {
        StartCoroutine(ComboAction(currentCombo[0]));
    }

    private IEnumerator ComboAction(AttackCombo combo)
    {
        isExecuting = true;
        isLocked = true;
        var headFrames = combo.headFrames.x + combo.headFrames.y;
        var bodyFrames = combo.bodyFrames.y - combo.bodyFrames.x;
        headFrames += combo.frameOffset;
        bodyFrames += combo.frameOffset;
        while (headFrames-- > 0)
        {
            headFrames--;
            yield return new WaitForEndOfFrame();
        }
        isLocked = false;
        while (bodyFrames-- > 0)
        {
            bodyFrames--;
            yield return new WaitForEndOfFrame();
        }
        RemoveCombo(combo);
        if (currentCombo.Count == 0) { finalAction.Invoke(); isExecuting = false; }
        else ExecuteNextCombo();
    }
}
