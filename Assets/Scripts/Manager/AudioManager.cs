using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    private static string  ASSET_AUDIO_PATH = "Audio/";
    [SerializeField] private AudioSource _audioSource;
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
    }

    public void PlayLevelAudio()
    {
        PlayMusic(Resources.Load(ASSET_AUDIO_PATH + _levelAudio) as AudioClip);
    }

    private void PlayMusic(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }

}
