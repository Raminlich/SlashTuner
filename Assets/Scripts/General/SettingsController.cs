using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public int targetFrame;
    private void Start()
    {
        Application.targetFrameRate = targetFrame;
    }
}
