using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventController : MonoBehaviour
{
    [SerializeField] bool _invokeOnEnable;
    [SerializeField] bool _invokeOnDisable;

    [SerializeField, ConditionField(nameof(_invokeOnEnable))] float _enableDelay;
    [SerializeField, ConditionField(nameof(_invokeOnEnable))] UnityEvent _onEnableEvent;
    [SerializeField, ConditionField(nameof(_invokeOnDisable))] float _disableDelay;
    [SerializeField, ConditionField(nameof(_invokeOnDisable))] UnityEvent _onDisableEvent;

    private void OnEnable()
    {
        if (_invokeOnEnable)
            StartCoroutine(_InvokeEventCD(_onEnableEvent, _enableDelay));
    }
    private void OnDisable()
    {
        if (_invokeOnDisable)
            StartCoroutine(_InvokeEventCD(_onDisableEvent, _disableDelay));
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    public void _StartEnableEvent()
    {
        _onEnableEvent.Invoke();
    }
    public void _StartDisableEvent()
    {
        _onDisableEvent.Invoke();
    }
    IEnumerator _InvokeEventCD(UnityEvent iEvent, float iDelay)
    {
        yield return new WaitForSeconds(iDelay);
        iEvent.Invoke();
    }
}
