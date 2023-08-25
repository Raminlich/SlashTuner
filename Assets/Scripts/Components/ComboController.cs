using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboController : MonoBehaviour
{
    [SerializeField] private bool isLocked;
    [SerializeField] private bool isExecuting;
    [SerializeField] private int currentComboFrames;
    private List<AttackCombo> currentCombo = new();
    private Action finalAction;
    private Action damageAction;
    private int currentComboDamage;

    public bool IsBusy()
    {
        return isLocked;
    }

    public void SetDamageAction(Action damageAction)
    {
        this.damageAction = damageAction;
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

    public int GetComboDamage()
    {
        return currentComboDamage;
    }

    private IEnumerator ComboAction(AttackCombo combo)
    {
        isExecuting = true;
        isLocked = true;
        var headFrames = combo.headFrames;
        var bodyFrames = combo.bodyFrames;
        currentComboFrames = combo.totalFrames;
        currentComboDamage = combo.damageAmount;
        headFrames += combo.frameOffset;
        bodyFrames += combo.frameOffset;
        StartCoroutine(DamageAction(combo.damageFrame));
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

    private IEnumerator DamageAction(int damageFrame)
    {
        while(damageFrame-- > 0)
        {
            damageFrame--;
            yield return new WaitForEndOfFrame();
        }
        damageAction.Invoke();
    }
}
