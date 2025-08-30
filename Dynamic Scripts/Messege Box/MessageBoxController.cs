using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MessageBoxController : MonoBehaviour
{
    [SerializeField] _AllMsgTypes _messageType;

    [Header("Functions")]
    [SerializeField] bool _autoAddToButtons = false;
    [SerializeField, ConditionalField(nameof(_autoAddToButtons))]
    UnityEngine.UI.Button _msgButton;

    [Header("Message Info")]
    [SerializeField] string _title;

    [SerializeField, ConditionalEnum(nameof(_messageType), (int)_AllMsgTypes.yesNo)]
    string _description;

    [SerializeField, ConditionalEnum(nameof(_messageType), (int)_AllMsgTypes.yesNo)]
    UnityEvent _confirmEvent;

    private void Start()
    {
        if (_autoAddToButtons)
            _msgButton.onClick.AddListener(_StartMsg);
    }
    public void _StartMsg()
    {
        if (_messageType == _AllMsgTypes.notification)
            MessageBoxManager.Instance._ShowNotificationMessage(_title);
        else if (_messageType == _AllMsgTypes.yesNo)
            MessageBoxManager.Instance._ShowYesNoMessage(_title, _description, _confirmEvent.Invoke);
    }
    public void _AddEvent(UnityAction iAction, bool iRemoveOtherEvents = false)
    {
        if (iRemoveOtherEvents) _confirmEvent.RemoveAllListeners();

        _confirmEvent.AddListener(iAction);
    }
    public void _ChangeEvent(UnityEvent iAction, bool iRemoveOtherEvents = false)
    {
        if (iRemoveOtherEvents) _confirmEvent.RemoveAllListeners();

        _confirmEvent.AddListener(iAction.Invoke);
    }
    public void _AskForConfirmation(UnityEvent iEvent)
    {
        _confirmEvent.AddListener(iEvent.Invoke);
        _confirmEvent.Invoke();
        _confirmEvent.RemoveListener(iEvent.Invoke);
    }
    public void _AskForConfirmation(UnityAction iEvent)
    {
        _confirmEvent.AddListener(iEvent);
        _confirmEvent.Invoke();
        _confirmEvent.RemoveListener(iEvent);
    }
    public enum _AllMsgTypes
    {
        notification, yesNo
    }
}
