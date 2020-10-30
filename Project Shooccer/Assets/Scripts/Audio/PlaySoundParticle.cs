using System;
using System.Collections;
using UnityEngine;

public class PlaySoundParticle : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    private void Awake()
    {
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }
}