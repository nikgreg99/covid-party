using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static string  ASSET_AUDIO_LEVEL_PATH = "Audio/Level/";
    [SerializeField] private AudioSource _audioSource;
    private AudioClip _audioClip;
    [SerializeField] private string _levelAudio;


    public static AudioManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("AudioManager is NULL");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
         initLevelAudio();
    }

    private void initLevelAudio()
    {
        _audioClip = Resources.Load(ASSET_AUDIO_LEVEL_PATH + _levelAudio) as AudioClip;
        _audioSource.clip = _audioClip;
    }

    public void PlayLevelAudio()
    {
        _audioSource.Play();
    }

 
    public void PauseLevelAudio()
    {
        _audioSource.Pause();
    }

}
