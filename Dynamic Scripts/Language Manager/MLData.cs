using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Taha/Multi Language Data")]
public class MLData : ScriptableObject
{
    [Header("General Settings")]
    [SerializeField] bool _showWarnings = true;
    [SerializeField] List<_LanguageMeta> _languageSettings = new List<_LanguageMeta>();

    [Header("Translation DataBase")]
    [SerializeField] List<_MLRecord> _translationDb = new List<_MLRecord>();

    public _LanguageMeta _GetLanguageMeta(_AllLanguages iLanguage)
    {
        for (int i = 0; i < _languageSettings.Count; i++)
            if (_languageSettings[i]._language == iLanguage)
                return _languageSettings[i];
        return null;
    }
    public Dictionary<int, string> _BuildHashedDictionary(_AllLanguages iLanguage)
    {
        int recordCount = _translationDb != null ? _translationDb.Count : 0;
        var dict = new Dictionary<int, string>(recordCount);

        for (int r = 0; r < recordCount; r++)
        {
            var record = _translationDb[r];
            if (record == null || record._translations == null || record._translations.Length == 0)
                continue;

            string baseKey = record._translations[0]; // always primary (index 0)
            if (string.IsNullOrEmpty(baseKey))
                continue;

            int hash = Animator.StringToHash(baseKey);

            string translated = record._GetTextForLanguage(iLanguage);
            if (string.IsNullOrEmpty(translated))
                translated = baseKey;

            if (!dict.ContainsKey(hash))
                dict.Add(hash, translated);
        }
        return dict;
    }

    #region Editor Only
    [Header("Search System")]
    [SerializeField] string _searchName;

    [CreateSOButton("Search Database")]
    private void _SearchDb()
    {
        SearchTools._SearchList_Full(_translationDb, _searchName, i => i._indexName);
    }

    public void _CheckForMissingTranslation()
    {
        if (!_showWarnings) return;

        int expectedCount = System.Enum.GetValues(typeof(_AllLanguages)).Length;
        foreach (var record in _translationDb)
        {
            if (record._translations.Length != expectedCount)
            {
                Debug.Log("the record <" + record._translations[0] + 
                    "> does not have translation");
                continue;
            }

            foreach (string translation in record._translations)
            {
                if (string.IsNullOrEmpty(translation))
                {
                    Debug.Log("the record <" + record._translations[0] + "> does not have translation");
                    continue;
                }
            }
        }
    }
    public List<_MLRecord> _GetAllRecords()
    {
        return _translationDb;
    }
    #endregion

    #region Classes
    [System.Serializable]
    public class _MLRecord
    {
        #region Editor Only Property
#if UNITY_EDITOR
        [Tooltip("this only changes the index name in the list and helps searching it")]
        public string _indexName;
#endif
        #endregion

        public string[] _translations;

        public string _GetTextForLanguage(_AllLanguages iLanguageIndex)
        {
            int languageIndex = (int)iLanguageIndex;

            #region Input Validation
            if (_translations == null)
            {
                Debug.LogError("This key is not defined in the translations Db");
                return null;
            }
            if (languageIndex < 0 || languageIndex >= _translations.Length)
            {
                Debug.LogError("This translation is not defined in the translations Db");
                return null;
            }
            #endregion

            return _translations[languageIndex];
        }
    }
    [System.Serializable]
    public class _LanguageMeta
    {
        public _AllLanguages _language;
        public Font _font;
        public TMP_FontAsset _fontAsset;
    }
    #endregion
}
/// <summary>
/// Warning: dont change the index in the enum, let it index it automatically from zero
/// 
/// the first language (index 0) is the primary key, you can change it at will
/// </summary>
public enum _AllLanguages
{
    English, Persian
}