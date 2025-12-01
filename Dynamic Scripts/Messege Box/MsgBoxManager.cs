using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MsgBoxManager : Singleton_Abs<MsgBoxManager>
{
    [SerializeField] YesNoPanelController _yesNoController;
    [SerializeField] NotificationController _NotificationController;
    public Canvas _msgBoxCanvas;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(transform.root);
    }
    public void _ShowYesNoMessage(string iTitle, string iDescription, params UnityAction[] iYesActions)
    {
        _yesNoController._OpenMenu(iTitle, iDescription, iYesActions);

#if UNITY_EDITOR
        if (EventSystem.current == null)
            Debug.LogError("There is no event system in the scene");
#endif
    }
    public void _ShowNotificationMessage(string iTitle)
    {
        _NotificationController._ShowNotification(iTitle);
    }
    public bool _IsMsgBoxActive()
    {
        if (_yesNoController._IsActive() || _NotificationController._IsActive())
            return true;
        return false;
    }
}
