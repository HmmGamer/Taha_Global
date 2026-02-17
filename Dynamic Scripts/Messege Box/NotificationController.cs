using UnityEngine;
using TMPro;
using System.Collections;
using TahaGlobal.ML;

namespace TahaGlobal.MsgBox
{
    public class NotificationController : MonoBehaviour
    {
        [SerializeField] GameObject _notificationPanel;
        [SerializeField] TMP_Text _titleText;
        [SerializeField] Animator _anim;

        [Tooltip("the hide animation duration is not included")]
        [SerializeField] float _duration = 2.5f;

        private Coroutine _disableCoroutine;

        public void _ShowNotification(string iTitle)
        {
            _titleText.text = iTitle;
            MLManager._instance._SetTextMeta(ref _titleText);

            _PanelActivation();

            if (_disableCoroutine != null)
                StopCoroutine(_disableCoroutine);

            _disableCoroutine = StartCoroutine(_DisableCoolDown());
        }

        /// <summary>
        /// remember to check the animator so additional triggers dont cause bugs
        /// the animation needs to disable the panel
        /// </summary>
        private void _PanelActivation()
        {
            _anim.ResetTrigger(A.Anim.t_showNotification);
            _anim.SetTrigger(A.Anim.t_showNotification);
        }
        private IEnumerator _DisableCoolDown()
        {
            yield return new WaitForSeconds(_duration);

            _anim.ResetTrigger(A.Anim.t_showNotification);
            _anim.SetTrigger(A.Anim.t_hideNotification);
        }

        public bool _IsActive()
        {
            return _notificationPanel.activeInHierarchy;
        }
    }
}