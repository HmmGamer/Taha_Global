using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BackButtonController : MonoBehaviour
{
    [SerializeField] int _priorityOrder;
    [SerializeField] UnityEvent _optionalEvent;
    private void Start()
    {
        BackButtonManager._instance._RegisterPanel(gameObject, _priorityOrder, _optionalEvent);
    }
    private void OnDestroy()
    {
        if (BackButtonManager._instance != null) // to avoid error on game exit
        {
            BackButtonManager._instance._UnRegisterPanel(gameObject);
        }
    }
}