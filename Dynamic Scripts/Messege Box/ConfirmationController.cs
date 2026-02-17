using TahaGlobal.ML;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TahaGlobal.MsgBox
{
    public class ConfirmationController : MonoBehaviour
    {
        [Header("Attachments")]
        [SerializeField] GameObject _confirmationPanel;
        [SerializeField] Button _confirmButton;
        [SerializeField] TMP_Text _title;
        [SerializeField] TMP_Text _description;

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
            _confirmationPanel.SetActive(iActivation);
        }
        public bool _IsActive()
        {
            return _confirmationPanel.activeInHierarchy;
        }
    }
}