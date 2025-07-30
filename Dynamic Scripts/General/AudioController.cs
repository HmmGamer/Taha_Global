using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] _AudioType _audioType;
    [Tooltip("true => automatically adds _playAudio event to the button")]
    [SerializeField] bool _playOnButtonClick;
    [SerializeField] bool _useSavedAudio;

    [SerializeField, ConditionalField(nameof(_useSavedAudio))]
    SavedSounds _audioName;
    [SerializeField, ConditionalField(nameof(_useSavedAudio), true)]
    AudioClip _audio;

    private void Start()
    {
        if (_playOnButtonClick)
        {
            if (gameObject.TryGetComponent<Button>(out Button button))
                button.onClick.AddListener(_PlayAudio);
            else
                Debug.LogError("attach button component to " + 
                    gameObject.name + " or deactivate _playOnButtonClick boolean");
        }
            
    }
    public void _PlayAudio()
    {
        if (_useSavedAudio)
            AudioManager._instance._PlayAudio(_audioType, _audioName);
        else
            AudioManager._instance._PlayAudio(_audioType, _audio);
    }
    public void _StopAudio()
    {

    }
}
