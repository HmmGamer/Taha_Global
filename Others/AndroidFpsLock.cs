using System.Collections;
using UnityEngine;

public class AndroidFpsLock : MonoBehaviour
{
    public bool _workInEverywhere = true;
    const int _fpsLock = 60;
    void Start()
    {
        if (_workInEverywhere)
            Application.targetFrameRate = _fpsLock;
        else
        {
#if UNITY_ANDROID
            Application.targetFrameRate = _fpsLock;
#endif
        }
    }
}
