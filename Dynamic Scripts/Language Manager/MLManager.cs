using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace TahaGlobal.ML
{
    /// <summary>
    /// TODO: add CSV file import/export
    /// 
    /// The MLManager (Multi-Language Manager) is the core translation system that handles
    /// text translation, font management, and runtime language switching for both UI Text 
    /// and TMP components.
    ///
    /// This system uses a hybrid structure combining readability and performance:
    /// - Each language stores text data in a simple list of entries.
    /// - Internally, all texts are hashed for better performance.
    ///
    /// Main Responsibilities:
    /// 1. Load the correct language dataset from MLData.
    /// 2. Manage a cached dictionary of hashed keys for fast text lookups.
    /// 3. Provide automatic text updates for all registered MLController components.
    /// 4. Handle font and TMP_FontAsset switching between languages.
    /// 5. Support both static and dynamic text updates (formatted and runtime).
    ///
    /// Key Features:
    /// - Lightweight and editor-friendly data structure.
    /// - Supports any number of languages via MLData asset.
    /// - logs missing translations (optional).
    /// - search system (inside editor)
    /// - Automatically updates all visible text elements when language changes.
    ///
    /// Integration Notes:
    /// - Select a language (like English) as the main language and only use that.
    /// - Attach an MLController to any Text or TMP_Text component to make it localizable.
    /// - In your settings, Use _ChangeLanguage() to change the active language.
    /// - Whenever you wanted translation, use _GetText() to translate the text.
    ///
    /// Important Notes:
    /// its strongly recommended to add this system after the main language texts are finalized,
    /// otherwise you may need to clean the database as some texts were removed or changed.
    /// 
    /// </summary>
    public class MLManager : Singleton_Abs<MLManager>
    {
        public static UnityAction _onLanguageChanged;

        [SerializeField] MLData _dataBase;
        [SerializeField] bool _showWarnings = true;

        [Tooltip("if true => the language is saved in disk when it changes")]
        [SerializeField] bool _autoSaveLastLanguage = true;

        [Tooltip("this is the default language you have at the start of the project, "
            + "this will become invalid when a change in language is saved")]
        [SerializeField] _AllLanguages _defaultStartingLanguage;

        [Tooltip("if true => the last saved language becomes invalid in editor")]
        [SerializeField] bool _forceDefaultInEditor = true;

        [SerializeField, ReadOnly] private _AllLanguages _currentLanguage;
        private MLData._FontMeta _currentLanguageMeta;
        private Dictionary<int, string> _translatedTexts;
        private Dictionary<int, Sprite> _translatedSprites;

        protected override void Awake()
        {
            base.Awake();

            _LoadCurrentLanguage();
            _LoadLanguage(_currentLanguage);

            #region Unity Editor Error Detection
#if UNITY_EDITOR
            if (_showWarnings)
                _dataBase._CheckForMissingTranslation();
#endif
            #endregion
        }

        private void _LoadLanguage(_AllLanguages iLanguage)
        {
            _currentLanguage = iLanguage;
            _currentLanguageMeta = _dataBase._GetLanguageMeta(_currentLanguage);

            _translatedTexts = MLHashManager._BuildTextHashedDictionary(_dataBase, _currentLanguage);
            _translatedSprites = MLHashManager._BuildSpriteHashedDictionary(_dataBase, _currentLanguage);
        }
        public void _ChangeLanguage(_AllLanguages iLanguage)
        {
            if (!_IsLanguageChanged(iLanguage))
                return;

            _LoadLanguage(iLanguage);
            _onLanguageChanged?.Invoke();

            if (_autoSaveLastLanguage)
                _SaveCurrentLanguage();
        }

        #region Text Getters Methods
        public string _GetTranslatedText(string iNewText)
        {
            int hash = MLHashManager._GetTextKey(iNewText);

            if (_translatedTexts != null && _translatedTexts.TryGetValue(hash, out string localized))
                return localized;

            return iNewText;
        }
        public string _GetTranslatedText(string iNewText, params object[] iArgs)
        {
            string template = _GetTranslatedText(iNewText);
            if (iArgs == null || iArgs.Length == 0)
                return template;

            try
            {
                return string.Format(template, iArgs);
            }
            catch
            {
                return template;
            }
        }
        public Font _GetTranslatedFont()
        {
            return _currentLanguageMeta != null ? _currentLanguageMeta._font : null;
        }
        public TMP_FontAsset _GetTranslatedFontAsset()
        {
            return _currentLanguageMeta != null ? _currentLanguageMeta._fontAsset : null;
        }
        #endregion

        #region Sprite Getter Methods
        public Sprite _GetTranslatedSprite(Sprite iKeySprite)
        {
            int hash = MLHashManager._GetSpriteKey(iKeySprite);

            if (_translatedSprites.TryGetValue(hash, out Sprite sprite))
                return sprite;

            return null;
        }
        #endregion

        #region Save\Load
        private void _LoadCurrentLanguage()
        {
            if (_forceDefaultInEditor)
            {
                _currentLanguage = _defaultStartingLanguage;
                return;
            }

            int languageIndex = PlayerPrefs.GetInt(A.DataKey.lastLanguage, -1);

            if (languageIndex == -1)
                _currentLanguage = _defaultStartingLanguage;
            else
                _currentLanguage = (_AllLanguages)languageIndex;
        }
        private void _SaveCurrentLanguage()
        {
            PlayerPrefs.SetInt(A.DataKey.lastLanguage, (int)_currentLanguage);
        }
        #endregion

        #region General Getters
        public bool _IsLanguageChanged(_AllLanguages iLanguage)
        {
            return _currentLanguage != iLanguage;
        }
        public _AllLanguages _GetCurrentLanguage()
        {
            return _currentLanguage;
        }
        #endregion

        #region Editor Only
#if UNITY_EDITOR

        [CreateMonoButton("Swap Language")]
        public void _SwapLanguageTesting()
        {
            _AllLanguages iLanguage;
            if ((int)_currentLanguage != 0)
                iLanguage = _AllLanguages.English;
            else
                iLanguage = _AllLanguages.Persian;

            _LoadLanguage(iLanguage);
            _onLanguageChanged?.Invoke();
        }
        public void _TryAddTextTranslationToDb(string iKey)
        {
            if (string.IsNullOrEmpty(iKey) || _dataBase == null)
                return;

            int hash = MLHashManager._GetTextKey(iKey);

            // Check if the text already exists in the DB
            var existingRecords = _dataBase._GetAllTextRecordsReference();
            for (int i = 0; i < existingRecords.Count; i++)
            {
                var record = existingRecords[i];
                if (record == null || record._translations == null || record._translations.Length == 0)
                    continue;

                int recordHash = MLHashManager._GetTextKey(record._translations[0]);
                if (recordHash == hash)
                    return; // Already exists, no need to add
            }

            // Not found, create new record
            int languageCount = System.Enum.GetValues(typeof(_AllLanguages)).Length;
            MLData._MLTextRecord newRecord = new MLData._MLTextRecord
            {
                _indexName = iKey,
                _translations = new string[languageCount],
            };
            newRecord._translations[0] = iKey;

            existingRecords.Add(newRecord);

            EditorApplication.delayCall += () =>
            {
                if (_dataBase != null)
                {
                    EditorUtility.SetDirty(_dataBase);
                    AssetDatabase.SaveAssets();
                }
            };
        }
        public void _TryAddSpriteTranslationToDb(Sprite iKeySprite)
        {
            if (iKeySprite == null || _dataBase == null)
                return;

            int hash = MLHashManager._GetSpriteKey(iKeySprite);

            // Check if the sprite is already in the database
            var existingRecords = _dataBase._GetAllSpriteRecordsReference();
            for (int i = 0; i < existingRecords.Count; i++)
            {
                var record = existingRecords[i];
                if (record == null || record._sprites == null || record._sprites.Length == 0)
                    continue;

                int recordHash = MLHashManager._GetSpriteKey(record._sprites[0]);
                if (recordHash == hash)
                    return; // Already exists, no need to add
            }

            // Not found, create new record
            int languageCount = System.Enum.GetValues(typeof(_AllLanguages)).Length;
            MLData._MLSpriteRecord newRecord = new MLData._MLSpriteRecord
            {
                _indexName = iKeySprite.name,
                _sprites = new Sprite[languageCount],
            };
            newRecord._sprites[0] = iKeySprite;

            existingRecords.Add(newRecord);

            EditorApplication.delayCall += () =>
            {
                if (_dataBase != null)
                {
                    EditorUtility.SetDirty(_dataBase);
                    AssetDatabase.SaveAssets();
                }
            };
        }

#endif
        #endregion
    }

    /// <summary>
    /// Warning: dont change the index in the enum, let it index it automatically
    /// 
    /// the first language (index 0) is the primary key, you can change it at will
    /// </summary>
    public enum _AllLanguages
    {
        English, Persian
    }
}