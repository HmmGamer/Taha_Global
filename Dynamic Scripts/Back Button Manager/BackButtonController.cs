using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BackButtonController : MonoBehaviour
{
    [SerializeField] int _priorityOrder;
    [SerializeField] UnityEvent _onOpenEvent;
    [SerializeField] UnityEvent _onCloseEvent;

    private void Start()
    {
        BackButtonManager._instance._RegisterPanel(gameObject, _priorityOrder, _onCloseEvent);
    }
    private void OnEnable()
    {
        _onOpenEvent.Invoke();
    }
    private void OnDestroy()
    {
        if (BackButtonManager._instance != null) // to avoid error on game exit
        {
            BackButtonManager._instance._UnRegisterPanel(gameObject);
        }
    }
    public void _ManualClose()
    {
        gameObject.SetActive(false);
        _onCloseEvent.Invoke();
    }
}