using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MessageBoxManager : MonoBehaviour
{
    public static MessageBoxManager Instance;

    [SerializeField] YesNoPanelController _yesNoController;
    [SerializeField] NotificationController _NotificationController;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(transform.root);
    }
    public void _ShowYesNoMessage(string iTitle, string iDescription, params UnityAction[] iYesActions)
    {
        _yesNoController._OpenMenu(iTitle, iDescription, iYesActions);
    }
    public void _ShowNotificationMessage(string iTitle)
    {
        _NotificationController._ShowNotification(iTitle);
    }
}
