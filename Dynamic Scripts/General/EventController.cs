using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventController : MonoBehaviour
{
    [SerializeField] bool _invokeOnEnable;
    [SerializeField] bool _invokeOnDisable;

    [SerializeField, ConditionalField(nameof(_invokeOnEnable))] UnityEvent _onEnableEvent;
    [SerializeField, ConditionalField(nameof(_invokeOnDisable))] UnityEvent _onDisableEvent;

    private void OnEnable()
    {
        if (_invokeOnEnable)
            _onEnableEvent.Invoke();
    }
    private void OnDisable()
    {
        if (_invokeOnDisable)
            _onEnableEvent.Invoke();
    }
}
