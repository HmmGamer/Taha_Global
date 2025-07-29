using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class LoadingManager : Singleton_Abs<LoadingManager>
{
    public static event UnityAction<_AllScenes> _onNewScene;

    [Header("General Settings")]
    [SerializeField] bool _autoLoadNext = false;
    [SerializeField, ConditionalField(nameof(_autoLoadNext))] _AllScenes _nextScene;

    [Header("Attachments")]
    [SerializeField] Canvas _canvas;
    [SerializeField] Image _loadingImage;
    [SerializeField] Slider _loadingSlider;

    [Header("Step Progress Settings")]
    [SerializeField] GameObject[] _steps; 

    private void Start()
    {
        if (_autoLoadNext)
            _LoadScene(_nextScene);
    }

    public void _LoadScene(_AllScenes iNextScene)
    {
        StartCoroutine(_LoadAsync((int)iNextScene));
    }

    private IEnumerator _LoadAsync(int iSceneIndex)
    {
        _canvas.gameObject.SetActive(true);
        AsyncOperation iOperation = SceneManager.LoadSceneAsync(iSceneIndex);
        iOperation.allowSceneActivation = false;

        while (!iOperation.isDone)
        {
            float iProgress = Mathf.Clamp01(iOperation.progress / 0.9f);
            _loadingSlider.value = iProgress;

            float stepThreshold = 1f / _steps.Length;

            for (int i = 0; i < _steps.Length; i++)
            {
                if (iProgress >= stepThreshold * (i + 1))
                    _steps[i].SetActive(true);
                else
                    _steps[i].SetActive(false);
            }

            if (iOperation.progress >= 0.9f)
            {
                _loadingSlider.value = 1f;

                for (int i = 0; i < _steps.Length; i++)
                {
                    _steps[i].SetActive(true);
                }

                iOperation.allowSceneActivation = true;
            }

            yield return null;
        }
        _onNewScene?.Invoke((_AllScenes)iSceneIndex);
        _canvas.gameObject.SetActive(false);
    }
}

// save the index as in the build settings
public enum _AllScenes
{
    Lobby, MainGame
}