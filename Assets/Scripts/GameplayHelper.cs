using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayHelper
{

    public static IEnumerator FramedAction(Action action, int frameInterval)
    {
        while (frameInterval-- > 0)
        {
            action.Invoke();
            frameInterval--;
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Action is Done");
    }
}
