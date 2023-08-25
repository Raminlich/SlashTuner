using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public int targetFrame;
    private void Start()
    {
        Application.targetFrameRate = targetFrame;
    }
}
