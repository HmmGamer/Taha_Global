using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TahaGlobal.ML
{
    /// <summary>
    /// TODO: fix the hardcoded parts (Persian conversion)
    /// </summary>
    public class MLTextController : MonoBehaviour
    {
        // Dev note:
        // we have dynamic text changes, so we cant use the saved data on this script.
        // but as it was hard and complicated to check the DB every time a new record
        // was made, we made a _data field so the DB be seen and edited more easily.

        [Header("Function Settings")]
        [Tooltip("True => disables the warning for null _keyId, used for" +
            " dynamic texts without a fixed _keyId")]
        [SerializeField] bool _isDynamicText;

        [Header("Only Assign One Of Them")]
        [Tooltip("True => automatically attach's the text component on start")]
        [SerializeField] bool _autoTextAttachment = true;

        [SerializeField, ConditionalField(nameof(_autoTextAttachment), true)]
        Text _legacyText;

        [SerializeField, ConditionalField(nameof(_autoTextAttachment), true)]
        TMP_Text _tmpText;

        [Header("In Database Record")]
        [Tooltip("translation is not based on this record. this is for easier control, " +
            "if you dont save the record to DB it be overwritten from the DB silently")]
        [SerializeField] MLData._MLTextRecord _data;

        private object[] _formatArgs;
        private bool _useFormatArgs;

        #region Starter Methods
        private void Start()
        {
            if (_autoTextAttachment)
            {
                _legacyText = GetComponent<Text>();
                _tmpText = GetComponent<TMP_Text>();
            }
            if (!_legacyText && !_tmpText)
                Debug.LogError("The MLController needs to have a legacy text or tmp", this);

            #region Editor Only (Error/Null Detection)
#if UNITY_EDITOR
            if (MLManager._instance == null)
            {
                Debug.LogError("You either dont have a MLManager in the scene," +
                    " or the MLManager's execution order needs to be higher than the" +
                    " MLController's execution order");
                return;
            }
            if (string.IsNullOrWhiteSpace(_data._keyId) && !_isDynamicText)
                Debug.Log("the keyId is empty", this);
#endif
            #endregion
        }
        private void OnEnable()
        {
            MLManager._onLanguageChanged += _OnLanguageChange;
            _OnLanguageChange();
        }
        private void OnDisable()
        {
            MLManager._onLanguageChanged -= _OnLanguageChange;
        }
        #endregion

        #region Fuctional Methods (private)
        private void _OnLanguageChange()
        {
            if (string.IsNullOrWhiteSpace(_data._keyId) && !_isDynamicText)
                return;

            StartCoroutine(_StartTranslation());
        }
        private void _RefreshText()
        {
            if (_useFormatArgs)
                _SetText(MLManager._instance._GetTranslatedText(_data._keyId, _formatArgs));
            else
                _SetText(MLManager._instance._GetTranslatedText(_data._keyId));
        }
        private void _UpdateFont()
        {
            if (_legacyText != null)
                MLManager._instance._SetTextMeta(ref _legacyText);
            else if (_tmpText != null)
                MLManager._instance._SetTextMeta(ref _tmpText);
        }
        private IEnumerator _StartTranslation()
        {
            // to avoid problems of another script changing the text OnEnable after this
            yield return new WaitForEndOfFrame();
            _RefreshText();
            _UpdateFont();
        }
        #endregion

        #region Public Setters
        /// <summary>
        /// this method helps you change the text dynamically in run time
        /// </summary>
        public void _ChangeText(string iKey)
        {
            _data._keyId = iKey;

            if (_legacyText != null)
                _legacyText.text = MLManager._instance._GetTranslatedText(iKey);
            else if (_tmpText != null)
                _tmpText.text = MLManager._instance._GetTranslatedText(iKey);
        }

        /// <summary>
        /// this method helps you change a formatted text dynamically in run time
        /// </summary>
        public void _ChangeText(string iKey, params object[] iArgs)
        {
            _data._keyId = iKey;

            _formatArgs = iArgs;
            _useFormatArgs = iArgs != null && iArgs.Length > 0;

            _SetText(MLManager._instance._GetTranslatedText(iKey, iArgs));
        }

        #endregion

        #region Private Getters
        private void _SetText(string iNewText)
        {
            if (_legacyText != null)
                _legacyText.text = iNewText;
            else if (_tmpText != null)
                _tmpText.text = iNewText;
            else
                Debug.LogError("The MLController needs to have a text or tmp", gameObject);
        }
        private string _GetText()
        {
            #region Editor Only
#if UNITY_EDITOR
            if (!Application.isPlaying)
                Start();
#endif
            #endregion

            if (_legacyText != null)
                return _legacyText.text;
            else if (_tmpText != null)
                return _tmpText.text;

            Debug.LogError("The MLController needs to have a text or tmp", this);
            return null;
        }
        #endregion

        #region Editor Only
#if UNITY_EDITOR

        MLManager _MLManager;

        [CreateMonoButton("Try Load From DB")]
        private void _TryLoadFromDB()
        {
            if (!_CheckMLManagerLoaded()) return;
            // check if current _keyId exists, then load the _data from DB
            if (!string.IsNullOrEmpty(_data._keyId))
            {
                MLData._MLTextRecord temp = null;
                temp = _MLManager._GetTextRecordFromDB(_data._keyId);

                if (temp != null)
                {
                    _data = temp;
                    Debug.Log("successes, The record for the <keyId = " + _data._keyId
                        + "> was loaded from the DB");
                }
                else
                {
                    Debug.Log("failed, the record with the <keyId = " + _data._keyId
                        + "> was not found in the DB");
                }
            }
        }

        [CreateMonoButton("Save To DB")]
        public void _TryAddToDb()
        {
            if (!_CheckMLManagerLoaded()) return;

            _MLManager._AddOrReplaceTextRecordToDb(_data);
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

        /// <summary>
        /// TODO: this is a half hardcoded part, fix this later
        /// </summary>
        [SerializeField, OnStringChanges_Mono("_ConvertPersianFont")]
        string _rawPersianText;
        private void _ConvertPersianFont()
        {
            _data._translations[(int)_AllLanguages.Persian] =
                FontTools._ConvertToPersian(_rawPersianText);

            _data._faFormatted = FontTools._ConvertToPersian_Visual(_rawPersianText);
        }
#endif
        #endregion
    }
}