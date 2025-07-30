using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton_Abs<AudioManager>
{
    [SerializeField] AudioSource _audioChanelSFX1;
    [SerializeField] AudioSource _audioChanelSFX2;
    [SerializeField] AudioSource _audioChanelMusic;
    [SerializeField] AudioSource _audioChanelTalk;
    [SerializeField] _SavedAudios[] _allSavedAudios;

    public void _PlayAudio(_AudioType iType, AudioClip iClip)
    {
        if (iType == _AudioType.SFX1)
        {
            _audioChanelSFX1.clip = iClip;
            _audioChanelSFX1.Play();
        }
        else if (iType == _AudioType.SFX2)
        {
            _audioChanelSFX2.clip = iClip;
            _audioChanelSFX2.Play();
        }
        else if (iType == _AudioType.Music)
        {
            _audioChanelMusic.clip = iClip;
            _audioChanelMusic.Play();
        }
        else if (iType == _AudioType.Talk)
        {
            _audioChanelTalk.clip = iClip;
            _audioChanelTalk.Play();
        }
    }
    public void _PlayAudio(_AudioType iType, SavedSounds iClipName)
    {
        for (int i = 0; i < _allSavedAudios.Length; i++)
        {
            if (_allSavedAudios[i]._audioName == iClipName.ToString())
            {
                _PlayAudio(iType, _allSavedAudios[i]._audio);
            }
        }
    }
    public void _StopGame()
    {
        _audioChanelSFX1.Stop();
        _audioChanelSFX2.Stop();
        _audioChanelTalk.Stop();
    }
    public void _ContinueGame()
    {
        if (_audioChanelSFX1.clip != null)
            _audioChanelSFX1.Play();
        if (_audioChanelSFX2.clip != null)
            _audioChanelSFX2.Play();
        if (_audioChanelTalk.clip != null)
            _audioChanelTalk.Play();
    }
    [CreateMonoButton("Make Enum")]
    public void _MakeEnum()
    {
        foreach (var item in _allSavedAudios)
        {
            if (EnumGenerator.ContainsBannedSymbols(item._audioName))
                Debug.LogError("_audioName can't contain space or - symbols");
        }
        EnumGenerator.GenerateEnums("SavedSounds", _allSavedAudios, nameof(_SavedAudios._audioName));
        EnumGenerator.AddValue("SavedSounds", "None");
    }

    [System.Serializable]
    public class _SavedAudios
    {
        public string _audioName;
        public AudioClip _audio;
    }
}
public enum _AudioType
{
    SFX1, SFX2, Music, Talk
}
//comment when you want to activate audioManager
public enum SavedSounds
{

}