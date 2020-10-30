using System.Collections.Generic;
using UnityEngine;

public class AudioBackgroundManager : MonoBehaviour
{
    private static AudioBackgroundManager instance = null;
    private AudioSourceCrossfade _crossfade;
    [SerializeField] private AudioSource uiAudioSource;

    // Game Instance Singleton
    public static AudioBackgroundManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        _crossfade = GetComponent<AudioSourceCrossfade>();
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayBackgroundMusic(AudioClip audioClip)
    {
        _crossfade.Play(audioClip);
    }

    public void PlayUI(AudioClip audioClip)
    {
        uiAudioSource.clip = audioClip;
        uiAudioSource.Play();
    }
}