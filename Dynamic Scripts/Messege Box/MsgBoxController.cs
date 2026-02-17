using TahaGlobal.ML;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TahaGlobal.MsgBox
{
    /// <summary>
    /// TODO: add DRY for msg sending
    /// </summary>
    public class MsgBoxController : MonoBehaviour
    {
        [SerializeField] _AllMsgTypes _messageType;

        [Header("Functions")]
        [SerializeField] bool _autoAddToButtons;
        [SerializeField, ConditionalField(nameof(_autoAddToButtons))]
        Button _msgButton;

        [Header("Optional ML Translation")]

        [Tooltip("if !null => it will automatically translate this msg for you")]
        [SerializeField, OnStringChanges_Mono("_CheckIfExistsInDB")] 
        string _titleMLKey;
        [SerializeField, ConditionalEnum(nameof(_messageType), (int)_AllMsgTypes.yesNo, (int)_AllMsgTypes.confirmation)
            ,OnStringChanges_Mono("_CheckIfExistsInDB")]
        string _descriptionMLKey;

        [SerializeField, ReadOnly] bool _titleFoundInDb; // visual purpose only

        [SerializeField, ReadOnly, ConditionalEnum(nameof(_messageType), (int)_AllMsgTypes.yesNo, (int)_AllMsgTypes.confirmation)]
        bool _descFoundInDb; // visual purpose only

        [Space(10f)]
        [Header("Message Info")]
        [SerializeField] string _title;

        [SerializeField, ConditionalEnum(nameof(_messageType), (int)_AllMsgTypes.yesNo, (int)_AllMsgTypes.confirmation), TextArea]
        string _description;

        [SerializeField, ConditionalEnum(nameof(_messageType), (int)_AllMsgTypes.yesNo, (int)_AllMsgTypes.confirmation)]
        UnityEvent _confirmEvent;

        private void Start()
        {
            if (_autoAddToButtons)
                _msgButton.onClick.AddListener(_StartMsg);
        }

        /// <summary>
        /// with this method you can call a predesigned msgBox in the code or the editor.
        /// </summary>
        public void _StartMsg()
        {
            if (_CheckForErrors())
                return;

            UnityAction ConfirmInvoke = () => _confirmEvent.Invoke();

            if (_messageType == _AllMsgTypes.notification)
            {
                MsgBoxManager._instance._ShowNotificationMessage(_title,_titleMLKey);
            }
            else if (_messageType == _AllMsgTypes.yesNo)
            {
                MsgBoxManager._instance._ShowYesNoMessage(_title, _description, _titleMLKey,_descriptionMLKey, ConfirmInvoke);
            }
            else if (_messageType == _AllMsgTypes.confirmation)
            {
                MsgBoxManager._instance._ShowConfirmationMessage(_title, _description, _titleMLKey, _descriptionMLKey, ConfirmInvoke);
            }
        }

        #region Confirmation Msg

        /// <summary>
        /// with this method you can start a msg with a one-time event
        /// you can set iAddControllerEvents to true to add this script events as well
        /// 
        /// note: this only changes the event of the next msg
        /// </summary>
        public void _StartAfterConfirmation(UnityEvent iConfirmationEvent, bool iAddControllerEvents = false)
        {
            if (_CheckForErrors())
                return;

            UnityAction ConfirmInvoke;

            if (iAddControllerEvents == false)
            {
                ConfirmInvoke = () => iConfirmationEvent.Invoke();
            }
            else
            {
                ConfirmInvoke = () =>
                {
                    iConfirmationEvent.Invoke();
                    _confirmEvent.Invoke();
                };
            }

            if (_messageType == _AllMsgTypes.notification)
            {
                Debug.LogError("You cant call this method with _messageType == notification");
                return;
            }
            else if (_messageType == _AllMsgTypes.yesNo)
            {
                MsgBoxManager._instance._ShowYesNoMessage(_title, _description, _titleMLKey, _descriptionMLKey, ConfirmInvoke);
            }
            else if (_messageType == _AllMsgTypes.confirmation)
            {
                MsgBoxManager._instance._ShowConfirmationMessage(_title, _description, _titleMLKey, _descriptionMLKey, ConfirmInvoke);
            }
        }
        public void _StartAfterConfirmation(UnityAction iConfirmationEvent, bool iAddControllerEvents = false)
        {
            if (_CheckForErrors())
                return;

            UnityAction ConfirmInvoke;

            if (iAddControllerEvents == false)
            {
                ConfirmInvoke = iConfirmationEvent;
            }
            else
            {
                ConfirmInvoke = () =>
                {
                    iConfirmationEvent.Invoke();
                    _confirmEvent.Invoke();
                };
            }

            if (_messageType == _AllMsgTypes.notification)
            {
                Debug.LogError("You cant call this method with _messageType == notification");
                return;
            }
            else if (_messageType == _AllMsgTypes.yesNo)
            {
                MsgBoxManager._instance._ShowYesNoMessage(_title, _description, _titleMLKey, _descriptionMLKey, ConfirmInvoke);
            }
            else if (_messageType == _AllMsgTypes.confirmation)
            {
                MsgBoxManager._instance._ShowConfirmationMessage(_title, _description, _titleMLKey, _descriptionMLKey, ConfirmInvoke);
            }
        }
        #endregion

        #region Event Changing

        /// <summary>
        /// with this method you can add a new action to the _confirmEvent
        /// with iRemoveOtherEvents you can overwrite the event in the unity editor
        /// note: this changes the event permanently (in game session)
        /// </summary>
        public void _ChangeEvent(UnityAction iAction, bool iRemoveOtherEvents = false)
        {
            if (iRemoveOtherEvents) _confirmEvent.RemoveAllListeners();

            _confirmEvent.AddListener(iAction);
        }
        public void _ChangeEvent(UnityEvent iAction, bool iRemoveOtherEvents = false)
        {
            if (iRemoveOtherEvents) _confirmEvent.RemoveAllListeners();

            _confirmEvent.AddListener(() => iAction.Invoke());
        }
        #endregion

        #region Error Detection
        private bool _CheckForErrors()
        {
            #region Editor Only
#if UNITY_EDITOR
            if (MsgBoxManager._instance == null)
            {
                Debug.LogError("There is no MsgBoxManager in the scene");
                return true;
            }

            if (MsgBoxManager._instance._IsMsgBoxActive(_messageType))
            {
                //Debug.LogError("There is another open MsgBox in the scene!");
                //return true;
            }
#endif
            return false;
            #endregion
        }
        #endregion

        #region Editor Only
#if UNITY_EDITOR

        MLManager _MLManager;

        private void _CheckIfExistsInDB()
        {
            if (!_CheckMLManagerLoaded()) return;

            if (!string.IsNullOrEmpty(_titleMLKey))
            {
                var temp = _MLManager._GetTextRecordFromDB(_titleMLKey);

                _titleFoundInDb = temp != null;
            }
            if (!string.IsNullOrEmpty(_descriptionMLKey))
            {
                var temp = _MLManager._GetTextRecordFromDB(_descriptionMLKey);

                _descFoundInDb = temp != null;
            }
        }
        private bool _CheckMLManagerLoaded()
        {
            // already found, return true
            if (_MLManager != null)
                return true;

            // try finding it
            if (Application.isPlaying)
            {
                _MLManager = MLManager._instance;
            }
            else
            {
                _MLManager = GameObject.FindFirstObjectByType<MLManager>();
            }

            // not found, give the error
            if (_MLManager == null)
            {
                Debug.LogError("the MLManager is missing in the scene or " +
                    "it's execution order is lower than the MLController's order");
            }

            return _MLManager != null;
        }
#endif
        #endregion
    }
}
