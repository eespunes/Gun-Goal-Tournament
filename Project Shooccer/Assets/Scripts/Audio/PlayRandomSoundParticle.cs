using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlayRandomSoundParticle : MonoBehaviour
{
    [SerializeField] private AudioClip[] clip;

    private void Start()
    {
        var index = Random.Range(0, clip.Length);
        GetComponent<AudioSource>().clip = clip[index];
        GetComponent<AudioSource>().Play();
    }
}