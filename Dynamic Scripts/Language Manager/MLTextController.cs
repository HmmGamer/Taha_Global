using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TahaGlobal.ML
{
    public class MLTextController : MonoBehaviour
    {
        private Text _legacyText;
        private TMP_Text _tmpText;
        private _AllLanguages _currentLanguage;
        private string _baseText;

        private void Awake()
        {
            _legacyText = GetComponent<Text>();
            _tmpText = GetComponent<TMP_Text>();
            if (_legacyText != null)
                _baseText = _legacyText.text;
            else if (_tmpText != null)
                _baseText = _tmpText.text;

            if (!_legacyText && !_tmpText)
            {
                Debug.Log(gameObject + "'s MLController needs to have text or TMP");
                return;
            }

            if (MLManager._instance == null)
            {
                Debug.LogError("You either dont have a MLManager in the scene," +
                    " or the MLManager's execution order needs to be higher than the" +
                    " MLController's execution order");
                return;
            }

            #region Editor Only
#if UNITY_EDITOR
            _TryAddToDb();
#endif
            #endregion
        }
        private void OnEnable()
        {
            MLManager._onLanguageChanged += _OnLanguageChange;
            if (MLManager._instance._IsLanguageChanged(_currentLanguage))
                _OnLanguageChange();
        }
        private void OnDisable()
        {
            MLManager._onLanguageChanged -= _OnLanguageChange;
        }
        private void _OnLanguageChange()
        {
            _currentLanguage = MLManager._instance._GetCurrentLanguage();
            _RefreshText();
            _UpdateFont();
        }
        private void _RefreshText()
        {
            if (_legacyText != null)
                _legacyText.text = MLManager._instance._GetTranslatedText(_baseText);
            else if (_tmpText != null)
                _tmpText.text = MLManager._instance._GetTranslatedText(_baseText);
        }
        private void _UpdateFont()
        {
            if (_legacyText != null)
                _legacyText.font = MLManager._instance._GetTranslatedFont();
            else if (_tmpText != null)
                _tmpText.font = MLManager._instance._GetTranslatedFontAsset();
        }

        /// <summary>
        /// this method helps you change the text dynamically in run time
        /// </summary>
        public void _ChangeText(string iText)
        {
            #region Editor Only
#if UNITY_EDITOR
            _TryAddToDb();
#endif
            #endregion

            if (_legacyText != null)
                _legacyText.text = MLManager._instance._GetTranslatedText(iText);
            else if (_tmpText != null)
                _tmpText.text = MLManager._instance._GetTranslatedText(iText);
        }

        /// <summary>
        /// this method helps you change a formatted text dynamically in run time
        /// 
        /// sample : _GetText("You have {0} coins", _coins)
        /// </summary>
        public void _ChangeText(string iText, params object[] iArgs)
        {
            #region Editor Only
#if UNITY_EDITOR
            _TryAddToDb();
#endif
            #endregion

            string localized = MLManager._instance._GetTranslatedText(iText, iArgs);
            if (_legacyText != null)
                _legacyText.text = localized;
            else if (_tmpText != null)
                _tmpText.text = localized;
        }

        #region Editor Only
#if UNITY_EDITOR

        public string _GetText()
        {
            if (!Application.isPlaying)
                Awake();

            if (_legacyText != null)
                return _legacyText.text;
            if (_tmpText != null)
                return _tmpText.text;

            return null;
        }
        public void _TryAddToDb()
        {
            if (!Application.isPlaying)
            {
                Debug.Log("this method only works in run time");
                return;
            }

            MLManager._instance._TryAddTextTranslationToDb(_GetText());
        }
#endif
        #endregion
    }
}