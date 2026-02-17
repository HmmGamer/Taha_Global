using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TahaGlobal.MsgBox
{
    [RequireComponent(typeof(CanvasGroup))]
    public class YesNoPanelController : MonoBehaviour
    {
        [Header("Attachments")]
        [SerializeField] Button _exitButton;
        [SerializeField] Button _cancelButton;
        [SerializeField] Button _confirmButton;
        [SerializeField] Text _title;
        [SerializeField] Text _description;

        CanvasGroup _canvasGroup;

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            _exitButton.onClick.AddListener(_CloseMenu);
            _cancelButton.onClick.AddListener(_CloseMenu);
        }
        public void _OpenMenu(string iTitle, string iDescription, UnityAction iYesActions)
        {
            _ActivateMenu(true);

            _title.text = iTitle;
            MLManager._instance._SetTextMeta(ref _title);

            _description.text = iDescription;
            MLManager._instance._SetTextMeta(ref _description);

            _confirmButton.onClick.RemoveAllListeners();

            _confirmButton.onClick.AddListener(iYesActions);
            _confirmButton.onClick.AddListener(_CloseMenu);
        }
        public void _CloseMenu()
        {
            _ActivateMenu(false);
        }
        private void _ActivateMenu(bool iActivation)
        {
            _canvasGroup.blocksRaycasts = iActivation;
            _canvasGroup.alpha = iActivation ? 1 : 0;
            _canvasGroup.interactable = iActivation;
        }
        public bool _IsActive()
        {
            return _canvasGroup.alpha != 0;
        }
    }
}