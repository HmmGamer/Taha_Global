using TahaGlobal.ML;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TahaGlobal.MsgBox
{
    public class YesNoPanelController : MonoBehaviour
    {
        [Header("Attachments")]
        [SerializeField] GameObject _yesNoPanel;
        [SerializeField] Button _exitButton;
        [SerializeField] Button _cancelButton;
        [SerializeField] Button _confirmButton;
        [SerializeField] TMP_Text _title;
        [SerializeField] TMP_Text _description;

        private void Start()
        {
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
            _yesNoPanel.SetActive(iActivation);
        }
        public bool _IsActive()
        {
            return _yesNoPanel.activeInHierarchy;
        }
    }
}