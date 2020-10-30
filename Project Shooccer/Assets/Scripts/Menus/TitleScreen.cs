using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TitleScreen : MonoBehaviour
{
    public AudioClip[] AudioClips;
    public AudioClip ui;

    private void Start()
    {
        AudioBackgroundManager.Instance.PlayBackgroundMusic(AudioClips[Random.Range(0, AudioClips.Length)]);
        PlayerPrefs.SetFloat("Sensibility", .5f);
    }

    public void ChoosePlayer()
    {
        AudioBackgroundManager.Instance.PlayUI(ui);
        SceneManager.LoadScene(1);
    }
}