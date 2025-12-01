using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MsgBoxController : MonoBehaviour
{
    [SerializeField] _AllMsgTypes _messageType;

    [Header("Functions")]
    [SerializeField] bool _autoAddToButtons = false;
    [SerializeField, ConditionalField(nameof(_autoAddToButtons))]
    UnityEngine.UI.Button _msgButton;

    [Header("Message Info")]
    [SerializeField] string _title;

    [SerializeField, ConditionalEnum(nameof(_messageType), (int)_AllMsgTypes.yesNo), TextArea]
    string _description;

    [SerializeField, ConditionalEnum(nameof(_messageType), (int)_AllMsgTypes.yesNo)]
    UnityEvent _confirmEvent;
    UnityAction _lastAddedAction;

    private void Start()
    {
        if (_autoAddToButtons)
            _msgButton.onClick.AddListener(_StartMsg);
    }
    private void OnDisable()
    {
        try
        {
            _confirmEvent.RemoveListener(_lastAddedAction);
            _lastAddedAction = null;
        }
        catch
        {

        }
    }
    public void _StartMsg()
    {
        #region Editor Only
#if UNITY_EDITOR
        if (MsgBoxManager._instance == null)
        {
            Debug.LogError("There is no MsgBoxManager in the scene");
            return;
        }
#endif
        #endregion

        if (_messageType == _AllMsgTypes.notification)
            MsgBoxManager._instance._ShowNotificationMessage(_title);
        else if (_messageType == _AllMsgTypes.yesNo)
            MsgBoxManager._instance._ShowYesNoMessage(_title, _description, _confirmEvent.Invoke);
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
        _lastAddedAction = iEvent.Invoke;
        _confirmEvent.AddListener(_lastAddedAction);

        _StartMsg();
    }
    public void _AskForConfirmation(UnityAction iEvent)
    {
        _lastAddedAction = iEvent;
        _confirmEvent.AddListener(_lastAddedAction);

        _StartMsg();
    }
    public enum _AllMsgTypes
    {
        notification, yesNo
    }
}
