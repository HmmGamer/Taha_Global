using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MessageBoxController : MonoBehaviour
{
    [SerializeField] _AllMsgTypes _messageType;

    [Header("Message Info")]
    [SerializeField] string _title;

    [SerializeField, ConditionalEnum(nameof(_messageType), (int)_AllMsgTypes.yesNo)]
    string _description;

    [SerializeField, ConditionalEnum(nameof(_messageType), (int)_AllMsgTypes.yesNo)]
    UnityEvent _confirmEvent;

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
    public enum _AllMsgTypes
    {
        notification, yesNo
    }
}
