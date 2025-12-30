using System.Collections.Generic;
using UnityEngine;

namespace TahaGlobal.ML
{
    public static class MLHashManager
    {
        private static Dictionary<string, int> _textHashCache = new Dictionary<string, int>(1024);
        private static Dictionary<_AllLanguages, Dictionary<int, string>> _textDictionaryCache = new Dictionary<_AllLanguages, Dictionary<int, string>>();
        private static Dictionary<_AllLanguages, Dictionary<int, Sprite>> _spriteDictionaryCache = new Dictionary<_AllLanguages, Dictionary<int, Sprite>>();

        public static int _GetTextKey(string iText)
        {
            if (string.IsNullOrEmpty(iText))
                return 0;

            if (_textHashCache.TryGetValue(iText, out int hash))
                return hash;

            hash = Animator.StringToHash(iText);
            _textHashCache[iText] = hash;
            return hash;
        }
        public static Dictionary<int, string> _BuildTextHashedDictionary(MLData iData, _AllLanguages iLanguage)
        {
            if (_textDictionaryCache.TryGetValue(iLanguage, out var cachedDict))
                return cachedDict;

            int recordCount = iData._GetAllTextRecordsReference()?.Count ?? 0;
            var dict = new Dictionary<int, string>(recordCount);

            for (int r = 0; r < recordCount; r++)
            {
                var record = iData._GetAllTextRecordsReference()[r];
                if (record == null || record._translations == null || record._translations.Length == 0)
                    continue;

                string baseKey = record._translations[0];
                if (string.IsNullOrEmpty(baseKey))
                    continue;

                int hash = _GetTextKey(baseKey);

                string translated = record._GetTextForLanguage(iLanguage);
                if (string.IsNullOrEmpty(translated))
                    translated = baseKey;

                if (!dict.ContainsKey(hash))
                    dict.Add(hash, translated);
            }

            _textDictionaryCache[iLanguage] = dict;
            return dict;
        }

        public static int _GetSpriteKey(Sprite iSprite)
        {
            if (iSprite == null)
                return 0;

            return Animator.StringToHash(iSprite.name);
        }
        public static Dictionary<int, Sprite> _BuildSpriteHashedDictionary(MLData iData, _AllLanguages iLanguage)
        {
            if (_spriteDictionaryCache.TryGetValue(iLanguage, out var cachedDict))
                return cachedDict;

            int recordCount = iData._GetAllSpriteRecordsReference()?.Count ?? 0;
            var dict = new Dictionary<int, Sprite>(recordCount);

            var records = iData._GetAllSpriteRecordsReference();

            for (int i = 0; i < recordCount; i++)
            {
                var record = records[i];
                if (record == null || record._sprites == null || record._sprites.Length == 0)
                    continue;

                Sprite baseSprite = record._sprites[0];
                Sprite translatedSprite = record._GetSpriteForLanguage(iLanguage);

                if (baseSprite == null || translatedSprite == null)
                    continue;

                int hash = _GetSpriteKey(baseSprite);

                if (!dict.ContainsKey(hash))
                    dict.Add(hash, translatedSprite);
            }

            _spriteDictionaryCache[iLanguage] = dict;
            return dict;
        }
    }
}
