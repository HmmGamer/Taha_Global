using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TahaGlobal.MsgBox
{
    public class MsgBoxManager : Singleton_Abs<MsgBoxManager>
    {
        [SerializeField] YesNoPanelController _yesNoController;
        [SerializeField] NotificationController _NotificationController;
        [SerializeField] ConfirmationController _confirmationController;
        public Canvas _msgBoxCanvas;

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(transform.root);
        }

        #region Show Msg
        public void _ShowYesNoMessage(string iTitle, string iDescription, UnityAction iYesActions)
        {
            _yesNoController._OpenMenu(iTitle, iDescription, iYesActions);
        }
        public void _ShowConfirmationMessage(string iTitle, string iDescription, UnityAction iYesActions)
        {
            _confirmationController._OpenMenu(iTitle, iDescription, iYesActions);
        }
        public void _ShowNotificationMessage(string iTitle)
        {
            _NotificationController._ShowNotification(iTitle);
        }
        #endregion

        #region Cancel Msg
        public void _CancelYesNoMessage()
        {
            _yesNoController._CloseMenu();
        }
        public void _CancelConfirmationMessage()
        {
            _confirmationController._CloseMenu();
        }
        #endregion

        #region Others

        /// <summary>
        /// notifications are not included
        /// </summary>
        public bool _IsAnyMsgBoxActive()
        {
            if (_yesNoController._IsActive() || _confirmationController._IsActive())
                return true;
            return false;
        }
        #endregion
    }
}