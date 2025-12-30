using UnityEngine;
using UnityEngine.UI;

namespace TahaGlobal.ML
{
    public class MLSpriteController : MonoBehaviour
    {
        [Header("Only Assign One Of Them")]
        [SerializeField] Image _uiImage;
        [SerializeField] SpriteRenderer _spriteRenderer;

        Sprite _keySprite;

        private void Awake()
        {
            _keySprite = _GetSprite();

            #region Editor Only
#if UNITY_EDITOR
            MLManager._instance._TryAddSpriteTranslationToDb(_keySprite);
#endif
            #endregion
        }
        private void OnEnable()
        {
            MLManager._onLanguageChanged += _RefreshSprite;
            _RefreshSprite();
        }
        private void OnDisable()
        {
            MLManager._onLanguageChanged -= _RefreshSprite;
        }
        private void _RefreshSprite()
        {
            Sprite sprite = MLManager._instance._GetTranslatedSprite(_keySprite);

            if (sprite != null)
            {
                // we show the warning for null in the
                _SetSprite(sprite);
            }
        }

        #region Sprite Get/Set
        /// <summary>
        /// method is to avoid hardcode, as we have 2 types of Image and SpriteRenderer
        /// </summary>
        private Sprite _GetSprite()
        {
            if (_uiImage != null)
                return _uiImage.sprite;
            else if (_spriteRenderer != null)
                return _spriteRenderer.sprite;
            else
                Debug.LogError("The controller needs an image/sprite renderer");

            return null;
        }
        private void _SetSprite(Sprite iSprite)
        {
            if (_uiImage != null)
                _uiImage.sprite = iSprite;
            else if (_spriteRenderer != null)
                _spriteRenderer.sprite = iSprite;
            else
                Debug.LogError("The controller needs an Image/SpriteRenderer");
        }
        #endregion
    }
}
