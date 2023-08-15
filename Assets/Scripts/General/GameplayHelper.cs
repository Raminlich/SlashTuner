using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayHelper
{

    public static IEnumerator FramedAction(int frameInterval, Action action,Action callback = null)
    {
        Debug.Log($"Starting Action with {frameInterval} frames");
        while (frameInterval-- > 0)
        {
            action.Invoke();
            frameInterval--;
            yield return new WaitForEndOfFrame();
        }
        callback?.Invoke();
        Debug.Log("Action is Done");
    }
}
