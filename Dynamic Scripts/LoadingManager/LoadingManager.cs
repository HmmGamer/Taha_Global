using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingManager : Singleton_Abs<LoadingManager>
{
    [Header("General Settings")]
    [SerializeField] bool _autoLoadNext = false;
    [SerializeField, ConditionalField(nameof(_autoLoadNext))] _AllScenes _nextScene;

    [Header("Attachments")]
    [SerializeField] Canvas _canvas;
    [SerializeField] Image _loadingImage;
    [SerializeField] Slider _loadingSlider;

    private void Start()
    {
        if (_autoLoadNext)
            _LoadNextScene();
    }
    public void _LoadNextScene()
    {
        StartCoroutine(_LoadAsync(SceneManager.GetActiveScene().buildIndex + 1));
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

            if (iOperation.progress >= 0.9f)
            {
                _loadingSlider.value = 1f;
                iOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        _canvas.gameObject.SetActive(false);
    }
}

// save the index as in the build settings
public enum _AllScenes
{
    Lobby, MainGame
}