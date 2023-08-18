using System;
using System.Collections;
using UnityEngine;

public class GameplayHelper
{
    private int frameInterval;
    public IEnumerator FramedAction(int frames, Action action, Action callback = null)
    {
        frameInterval = frames;
        Debug.Log($"Starting Action with {frameInterval} frames");
        while (frameInterval-- > 0)
        {
            action?.Invoke();
            frameInterval--;
            yield return new WaitForFixedUpdate();
        }
        callback?.Invoke();
        Debug.Log("Action is Done");
    }

    public int GetCurrentInterval()
    {
        return frameInterval;
    }
}
