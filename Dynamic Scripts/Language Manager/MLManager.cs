using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

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
    public static UnityAction _onLanguageChange;

    [SerializeField] MLData _dataBase;
    [Tooltip("The _defaultLanguage is the zero index in _AllLanguages enum")]
    [SerializeField, ReadOnly] _AllLanguages _projectLanguage;

    private MLData._LanguageMeta _currentLanguageMeta;
    private _AllLanguages _currentLanguage;
    private Dictionary<int, string> _translatedTexts;
    private Dictionary<string, int> _stringHashCache = new Dictionary<string, int>(1024);

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
        _onLanguageChange?.Invoke();
    }
    public void _TryAddTranslationToDb(string iKey)
    {
        if (string.IsNullOrEmpty(iKey) || _dataBase == null)
            return;

        int hash = Animator.StringToHash(iKey);

        if (_translatedTexts != null && _translatedTexts.ContainsKey(hash))
            return;

        int languageCount = System.Enum.GetValues(typeof(_AllLanguages)).Length;
        MLData._MLRecord newRecord = new MLData._MLRecord
        {
            #region Editor Only
#if UNITY_EDITOR
            _indexName = iKey,
#endif
            #endregion

            _translations = new string[languageCount],
        };
        newRecord._translations[0] = iKey;

        _dataBase._GetAllRecords().Add(newRecord);

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

    protected override void Awake()
    {
        base.Awake();

        _LoadLanguage(_projectLanguage);

        #region Unity Editor Error Detection
#if UNITY_EDITOR
        _dataBase._CheckForMissingTranslation();
#endif
        #endregion
    }
    private void _LoadLanguage(_AllLanguages iLanguage)
    {
        _currentLanguage = iLanguage;
        _currentLanguageMeta = _dataBase._GetLanguageMeta(_currentLanguage);
        _translatedTexts = _dataBase._BuildHashedDictionary(_currentLanguage);
    }
    public void _ChangeLanguage(_AllLanguages iLanguage)
    {
        if (_currentLanguage == iLanguage)
            return;

        _LoadLanguage(iLanguage);
        _onLanguageChange?.Invoke();
    }

    #region Text Getters Methods
    public string _GetText(string iNewText)
    {
        if (string.IsNullOrEmpty(iNewText))
            return string.Empty;
        int hash = _GetHash(iNewText);
        if (_translatedTexts != null && _translatedTexts.TryGetValue(hash, out string localized))
            return localized;

        return iNewText;
    }
    public string _GetText(string iNewText, params object[] iArgs)
    {
        string template = _GetText(iNewText);
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
    public Font _GetCurrentFont()
    {
        return _currentLanguageMeta != null ? _currentLanguageMeta._font : null;
    }
    public TMP_FontAsset _GetCurrentFontAsset()
    {
        return _currentLanguageMeta != null ? _currentLanguageMeta._fontAsset : null;
    }
    private int _GetHash(string iText)
    {
        if (_stringHashCache.TryGetValue(iText, out int h))
            return h;
        int hash = Animator.StringToHash(iText);
        _stringHashCache[iText] = hash;
        return hash;
    }
    #endregion
}